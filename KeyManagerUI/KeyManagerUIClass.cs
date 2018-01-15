using System;
using KeePass.Plugins;
using System.Windows.Forms;
using KeePassLib.Keys;
using KeePassLib.Serialization;
using System.IO;
using KeePassLib.Utility;
using KeePass.UI;

// Build : & 'C:\Program Files (x86)\KeePass Password Safe 2\KeePass.exe' --plgx-create "C:\Users\TraubS\Documents\GitHub\KeePass-KeyManager\KeyManagerUI"

namespace KeyManagerUI
{
    /// <summary>
    /// The class to initiate the plugin and the key provider - don't rename !! 
    /// </summary>
    public class KeyManagerUIExt : Plugin
    {
        private IPluginHost m_host = null;
        private ToolStripSeparator m_tsSeparator = null;
        private ToolStripMenuItem m_tsmiMenuItem = null;
        private CertBasedKeyProvider m_prov = new CertBasedKeyProvider();

       public override string UpdateUrl
        {
            get
            {
                return "https://techsupport.audius.de/keepass/pluginversion";
            }
        }

        public override bool Initialize(IPluginHost host)
        {
            m_host = host;

            // Add provider
            m_host.KeyProviderPool.Add(m_prov);
            
            // Get a reference to the 'Tools' menu item container
            ToolStripItemCollection tsMenu = m_host.MainWindow.ToolsMenu.DropDownItems;

            // Add a separator at the bottom
            m_tsSeparator = new ToolStripSeparator();
            tsMenu.Add(m_tsSeparator);

            // Add menu item 'Do Something'
            m_tsmiMenuItem = new ToolStripMenuItem();
            m_tsmiMenuItem.Text = "Key Manager UI";
            m_tsmiMenuItem.Click += this.OnMenuDoSomething;
            tsMenu.Add(m_tsmiMenuItem);

            return true;
        }

        public override void Terminate()
        {
            // Remove all of our menu items
            ToolStripItemCollection tsMenu = m_host.MainWindow.ToolsMenu.DropDownItems;
            m_tsmiMenuItem.Click -= this.OnMenuDoSomething;
            tsMenu.Remove(m_tsmiMenuItem);
            tsMenu.Remove(m_tsSeparator);

            // Remove key provider
            m_host.KeyProviderPool.Remove(m_prov);
        }

        private void OnMenuDoSomething(object sender, EventArgs e)
        {
            KeyManagerUIForm form = new KeyManagerUIForm();
            form.Show();
        }
    }

    /// <summary>
    /// A key provider plugin for KeePass that, for new databases, generates a key and encrypts it using 
    /// a set of public keys contained in encryption certificates.  When opening a database, the key is
    /// decrypted using a private key.
    /// </summary>
    public sealed class CertBasedKeyProvider : KeyProvider
    {
        private static int MAX_KEY_FILE_LENGTH = 1024 * 1024; // 1 MB ought to be way too much
        public static string CertProtKeyFileExtension = "p7mkey";
        public override string Name
        {
            get { return "KeePassX509Provider"; }
        }
        /// <summary>
        /// Get a key for the database.
        /// </summary>
        /// <param name="ctx">Context - queried for whether a new key should be created, and the database path</param>
        /// <returns>A byte array with the key, or null if an error occurs.  If an error occurs, user is
        /// notified of the error.</returns>
        public override byte[] GetKey(KeyProviderQueryContext ctx)
        {
            return ctx.CreatingNewKey ? GetNewKey() : GetExistingKey(ctx.DatabaseIOInfo);
        }

        byte[] GetNewKey()
        {
            
            KeyManagerUIForm form = new KeyManagerUIForm();
            try
            {
                form.ShowDialog();
                return form.priKey;
            }
            catch (NullReferenceException null_ex)
            {
                MessageBox.Show("No key was generated!", "Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
        /// <summary>
        /// Get a key for an existing database.  First, the key file is located, either because its location
        /// and filename are the same as the database path (with the exception of the extension), or the user
        /// is asked.  Then, the key file is decrypted using a private key.
        /// </summary>
        /// <param name="strPath">Full filename of the database file.</param>
        /// <returns>A byte array with the key, or null if an error occurs.  If an error occurs, user is
        /// notified of the error.</returns>
        byte[] GetExistingKey(IOConnectionInfo ioc)
        {
            Stream stream = null;
            try
            {
                string newpath = UrlUtil.StripExtension(ioc.Path) + "." + CertProtKeyFileExtension;
                IOConnectionInfo keyIoc = ioc.CloneDeep();
                keyIoc.Path = newpath;
                stream = IOConnection.OpenRead(keyIoc);
            }
            catch (Exception e)
            {
                // strPath may be a URL (even if IsLocalFile returns true?), 
                // whatever the reason, fall through and the user can pick a 
                // local file as the key file
            }

            if (stream == null || !stream.CanRead)
            {
                // fall back on opening a local file
                // FUTURE ENHANCEMENT: allow user to enter a URL and name/pwd as well

                OpenFileDialog ofd = UIUtil.CreateOpenFileDialog("KeePassX509Provider", UIUtil.CreateFileTypeFilter(CertProtKeyFileExtension, "x05KeyFile", true), 1, CertProtKeyFileExtension, false /* multi-select */, true);

                if (ofd.ShowDialog() != DialogResult.OK)
                {
                    return null;
                }
                stream = IOConnection.OpenRead(IOConnectionInfo.FromPath(ofd.FileName));
            }
            try
            {
                BinaryReader reader = new BinaryReader(stream);
                byte[] p7m = reader.ReadBytes(MAX_KEY_FILE_LENGTH);
                // URL streams don't support seeking, and so Position doesn't work
                //bool tooBig = stream.Position >= MAX_KEY_FILE_LENGTH;
                bool tooBig = p7m.Length >= MAX_KEY_FILE_LENGTH;
                reader.Close();

                if (tooBig)
                {
                    MessageBox.Show("Kes File ist to big");
                    return null;
                }
                Certmanager cert_mgr = new Certmanager();
                return cert_mgr.DecryptMsg(p7m);
            }
            catch (SystemException ex)  // covers IOException and CryptographicException
            {
                MessageBox.Show("Error at encryption or IO error!\nIf you used a smart card for encryption, please provide/plugin fist!");
                return null;
            }
        }
    }
}
