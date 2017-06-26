# KeePass-KeyManager
The manager provides a crpyto authentication provider for the Keepass and also a GUI for the management.
Based of the usage of X509-Certificates one KeePass-Database can encrypted for mutlible users.

It allows KeePass to use an AES-Key (master key), which is encrypted with the X509-Certificate (use RSA-Keys) from the Windows certificate store and flat key files (*.crt)
of one or more users stored in a 7mkey-file, as a master key source.

After the selection of a X509-Certificate, it searchs the 7mkey-file for the certificates subject, gets the respective,
encrpyted AES-Key and decrypt it with the certificate. KeePass will use the returned decrypted AES-Key along with the other
given credentials (like password, keyfile) for open the database. 

The GUI helps you to manage the certifcats and the keys.

For more information about KeePass Security, please have a look at the KeePass Security Page.

Technics and idea based on the plugins form Dirk Heitzmann, creative-webdesign.de and Mark Buchler, https://github.com/markbott/CertKeyProvider

# Installation

Copy the PLGX file to the same folder where KeePass lives.  Next time you start KeePass, KeePass should load the plugin.

To uninstall, remove the PLGX file and consult KeePass documentation.

# Usage
TbD
