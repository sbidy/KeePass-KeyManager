using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace KeyManagerUI
{
    class Keyring
    {
        public void SerializeKeyring(X509Certificate2Collection collection, string filename)
        {
            List<string> certlist = new List<string>();

            foreach (X509Certificate2 cert in collection)
            {
                certlist.Add(Convert.ToBase64String(cert.RawData));
            }

            XmlSerializer serializer = new XmlSerializer(typeof(List<string>));
            StringWriter sw = new StringWriter();
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();

            serializer.Serialize(sw, certlist, ns);
            File.WriteAllText(filename, sw.ToString());
        }
        public X509Certificate2Collection DeserializeKeyring(string filename)
        {
            string KeyringXML = File.ReadAllText(filename);
            X509Certificate2Collection collection = new X509Certificate2Collection();
            XmlSerializer xs = new XmlSerializer(typeof(List<string>));

            StringReader sr = new StringReader(KeyringXML);
            List<string> templist = (List<string>)xs.Deserialize(sr);

            foreach (string rawdata in templist)
            {
                collection.Add(new X509Certificate2(Convert.FromBase64String(rawdata)));
            }
            return collection;
        }
    }
}
