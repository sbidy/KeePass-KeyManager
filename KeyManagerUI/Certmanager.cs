using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Windows.Forms;

namespace KeyManagerUI
{
    class Certmanager
    {
        public X509Certificate2Collection applied_certs;
        public X509Certificate2Collection keyring_certs;
        private HashSet<string> _not_applied_certs = new HashSet<string>();
        public HashSet<string> not_applied_certs
        {
            get { return _not_applied_certs; }
        }
        public Certmanager ()
        {
            applied_certs = new X509Certificate2Collection();
            keyring_certs = new X509Certificate2Collection();
        }

        /// <summary>
        /// Encrypt data for the specified set of certificates.  
        /// Adapted from http://msdn.microsoft.com/en-us/library/bb924547.aspx
        /// </summary>
        /// <param name="msg">Data to encrypt</param>
        /// <param name="recipientCerts">Certificates to encrypt for</param>
        /// <returns>Encrypted blob</returns>
        public byte[] EncryptMsg(Byte[] msg, X509Certificate2Collection recipientCerts)
        {
            //  Place the message in a ContentInfo object.
            //  This is required to build an EnvelopedCms object.
            ContentInfo contentInfo = new ContentInfo(msg);

            recipientCerts = checkCerts(recipientCerts);

            if (recipientCerts != null)
            {
                //  Instantiate an EnvelopedCms object with the ContentInfo
                //  above.
                //  Has default SubjectIdentifierType IssuerAndSerialNumber.
                //  Force usage of AES256 instead of 3DES
                Oid aes256 = Oid.FromFriendlyName("aes256", OidGroup.EncryptionAlgorithm);
                EnvelopedCms envelopedCms = new EnvelopedCms(contentInfo, new AlgorithmIdentifier(aes256));

                //  Formulate a CmsRecipient object collection that
                //  represent information about the recipients 
                //  to encrypt the message for.
                if (recipientCerts.Count > 0)
                {
                    CmsRecipientCollection recips = new CmsRecipientCollection(SubjectIdentifierType.IssuerAndSerialNumber, recipientCerts);

                    //  Encrypt the message for the recipient.
                    envelopedCms.Encrypt(recips);

                    //  The encoded EnvelopedCms message contains the message
                    //  ciphertext and the information about each recipient 
                    //  that the message was enveloped for.
                    return envelopedCms.Encode();
                }
            }
            else
            {
                throw new CryptographicException();
            }
            return null;
        }
        /// <summary>
        /// Decrypt a message using a private key available on the system.
        /// </summary>
        /// <param name="encodedEnvelopedCms">Encrypted blob</param>
        /// <returns>Decrypted data, or null if there was an error</returns>
        public byte[] DecryptMsg(byte[] encodedEnvelopedCms)
        {
            //  Prepare object in which to decode and decrypt.
            EnvelopedCms envelopedCms = new EnvelopedCms();

            //  Decode the message.
            envelopedCms.Decode(encodedEnvelopedCms);

            X509Store myStore = new X509Store(StoreName.My);
            myStore.Open(OpenFlags.ReadOnly);
            envelopedCms.Decrypt(myStore.Certificates);
            myStore.Close();

            //  The decrypted message occupies the ContentInfo property
            //  after the Decrypt method is invoked.
            return envelopedCms.ContentInfo.Content;
        }
        /// <summary>
        /// Decrypt a message using a private key available on the system.
        /// </summary>
        /// <param name="encodedEnvelopedCms">Encrypted blob</param>
        /// <returns>Decrypted data, or null if there was an error</returns>
        public byte[] DecryptMsg2(byte[] encodedEnvelopedCms)
        {
            //  Prepare object in which to decode and decrypt.
            EnvelopedCms envelopedCms = new EnvelopedCms();

            //  Decode the message.
            envelopedCms.Decode(encodedEnvelopedCms);
            envelopedCms.Decrypt();
      
            //  The decrypted message occupies the ContentInfo property
            //  after the Decrypt method is invoked.
            return envelopedCms.ContentInfo.Content;
        }
        public void getRecipient(byte[] encodedEnvelopedCms)
        {
            _not_applied_certs = new HashSet<string>();
            // Prepare object in which to decode and decrypt.
            EnvelopedCms envelopedCms = new EnvelopedCms();
            //  Decode the message.
            envelopedCms.Decode(encodedEnvelopedCms);

            RecipientInfoCollection recips = envelopedCms.RecipientInfos;

            foreach (RecipientInfo info in recips)
            {
                X509IssuerSerial serial = (X509IssuerSerial)info.RecipientIdentifier.Value;
                X509Certificate2Collection found_certs = FindCerts(serial.SerialNumber.ToString());
                found_certs.AddRange(applied_certs);

                if (keyring_certs.Count > 0)
                    found_certs = keyring_certs.Find(X509FindType.FindBySerialNumber, serial.SerialNumber.ToString(), true);

                if (found_certs.Count == 0)
                    _not_applied_certs.Add(serial.SerialNumber.ToString());

                applied_certs.AddRange(found_certs);
            }
            applied_certs = removeDuplicates(applied_certs);
        }
        /// <summary>
        /// Search for certificates in the local cert stores
        /// </summary>
        /// <param name="serialNumber">The certificate serial number</param>
        /// <returns>A collection of X509 certificates</returns>
        public X509Certificate2Collection FindCerts(string serialNumber)
        {
            X509Store addrBookStore = new X509Store(StoreName.AddressBook, StoreLocation.CurrentUser);
            addrBookStore.Open(OpenFlags.ReadOnly);

            X509Store myStore = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            myStore.Open(OpenFlags.ReadOnly);

            X509Certificate2Collection allCerts = addrBookStore.Certificates;
            allCerts.AddRange(myStore.Certificates);

            addrBookStore.Close();
            myStore.Close();

            var matchingCertificates = allCerts.Find(X509FindType.FindBySerialNumber, serialNumber, true);

            return matchingCertificates;
        }
        /// <summary>
        /// Deletes the duplicates entry's in a X509Certifacte2Collection
        /// </summary>
        /// <param name="source">The input collection</param>
        /// <returns>Output collection without duplicates</returns>
        public X509Certificate2Collection removeDuplicates(X509Certificate2Collection source)
        {
            X509Certificate2Collection output = new X509Certificate2Collection();
            HashSet<string> serials = new HashSet<string>();

            foreach (X509Certificate2 cert in source)
            {
                serials.Add(cert.SerialNumber);
            }

            foreach (string sn in serials)
            {
                foreach (X509Certificate2 cert in source)
                {
                    if (cert.SerialNumber == sn && (output.Contains(cert) == false))
                    {
                        output.Add(cert);
                    }
                }
            }
            return output;
        }
        /// <summary>
        /// Shows the certificate UI and adds the selected certificates to the applied cert collection
        /// </summary>
        public void addFromStore()
        {
            X509Store addrBookStore = new X509Store(StoreName.AddressBook, StoreLocation.CurrentUser);
            addrBookStore.Open(OpenFlags.ReadOnly);

            X509Store myStore = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            myStore.Open(OpenFlags.ReadOnly);

            X509Certificate2Collection allCerts = addrBookStore.Certificates;
            allCerts.AddRange(myStore.Certificates);

            addrBookStore.Close();
            myStore.Close();

            X509Certificate2Collection fcollection = allCerts; //.Find(X509FindType.FindByTimeValid, DateTime.Now, false);
            fcollection = fcollection.Find(X509FindType.FindByKeyUsage, X509KeyUsageFlags.KeyEncipherment, false);
            X509Certificate2Collection store_certs = X509Certificate2UI.SelectFromCollection(fcollection, "Select certificate", "Select a certificate from local Microsoft Windows store:", X509SelectionFlag.MultiSelection);

            applied_certs.AddRange(store_certs);
        }
        /// <summary>
        /// Checks the certificate for revocation and validity --- DEPRECATED: NOT USED ????!!!!
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private X509Certificate2Collection checkCerts(X509Certificate2Collection certs_to_check)
        {
            X509Certificate2Collection scollection = new X509Certificate2Collection();
            scollection.AddRange(certs_to_check);

            if (scollection == null || scollection.Count < 1)
            {
                return null;
            }

            // validate certificates
            X509Chain chain = new X509Chain();
            chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;

            X509Certificate2Collection toRemove = new X509Certificate2Collection();
            foreach (X509Certificate2 cert in scollection)
            {
                bool chainRc = false;
                chainRc = chain.Build(cert);

                if (!chainRc)
                {
                    // certificate is invalid ... keep it?
                    StringBuilder reason = new StringBuilder();
                    for (int index = 0; index < chain.ChainStatus.Length; index++)
                    {
                        reason.AppendLine(chain.ChainStatus[index].StatusInformation);
                    }
                    DialogResult decision = MessageBox.Show("Certificate:\n"+cert.SubjectName.Name+ "\n\ncan't validated and maybe not applicable -  try anyway?\nReasons: " + reason, "Error",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
                    if (decision == DialogResult.No)
                    {
                        toRemove.Insert(0, cert);
                    }
                }
                scollection = removeDuplicates(scollection);
            }

            foreach (X509Certificate2 cert in toRemove)
            {
                scollection.Remove(cert);
            }

            if (scollection.Count < 1)
            {
                return null;
            }

            return scollection;
        }
        /// <summary>
        /// Checks if one of the pub keys are matched to a user private key
        /// </summary>
        /// <param name="collection"></param>
        /// <returns>Returns true if a pub-priv key match exists</returns>
        public bool checkIfPrivKeyExists(X509Certificate2Collection collection)
        {
            bool havePrivateKey = false;
            foreach (X509Certificate2 cert in collection)
            {
                havePrivateKey |= cert.HasPrivateKey;
            }
            return havePrivateKey;
        }
    }
}
