# KeePass-KeyManager
The manager provides a crypto. authentication provider for the KeePass and also a GUI for the management. Based of the usage of X509-Certificates one KeePass-Database can encrypted for multiple users.

It allows KeePass to use an AES-Key (master key), which is encrypted with the X509-Certificate (use RSA-Keys) from the Windows certificate store and flat key files (*.crt) of one or more users stored in a 7mkey-file, as a master key source.

After the selection of a X509-Certificate, it search through the 7mkey-file for the certificates subject, gets the respective, encrypted AES-Key and decrypt it with the certificate. KeePass will use the returned decrypted AES-Key along with the other given credentials (like password, keyfile) for open the database.

The GUI helps you to manage the certificates and the keys.

For more information about KeePass Security, please have a look at the KeePass Security Page.

Technics and idea based on the plugins form Dirk Heitzmann, creative-webdesign.de and Mark Buchler, https://github.com/markbott/CertKeyProvider

# Installation
Link to the PLGX file --> [Download](https://github.com/sbidy/KeePass-KeyManager/releases/download/1.3/KeyManagerUI.plgx)

Copy the PLGX file to the plugin folder where KeePass lives.  Next time you start KeePass, KeePass should load the plugin.

To uninstall, remove the PLGX file and consult KeePass documentation.

For Yubikey users: Please install the latest MiniDriver for the Yubikey! --> [Website](https://www.yubico.com/products/services-software/download/smart-card-drivers-tools/)

# Usage
When creating a new database, select "Key file / provider" and choose "KeePassX509Provider" from the drop-down list.

This key provider can be combined with other key sources (master password, Windows user account).

You will then be prompted to select the certificates via the KeyManagerUI for which the key should be encrypted.  The certificates that are presented were retrieved from the Windows certificate (CAPI) store or you import a flat file (.crt).

![Screen shot](/Capture.PNG?raw=true "Screen shot")

Encrypting the key for multiple certificates is useful in various scenarios -- you have two different keys and certificates on different machines, you want to share the password database with a group of people (e.g. in SharePoint, git) etc.

IMPORTANT:  Make sure you have access to at least one private key corresponding to the certificates.  As well, be sure to back up those private keys or ensure that they are recoverable in some way.  You will not be able to open the database without it.

When opening the database, select the "KeePassX509Provider" key provider again.  The system will automatically find a private key to decrypt the database.  If a key is not found, you will see an error.

## Creat a new DB
![Create_DB](https://github.com/sbidy/KeePass-KeyManager/blob/master/Create_DB.gif)

## Add user
![Add_User](https://github.com/sbidy/KeePass-KeyManager/blob/master/Add_User.gif)

## Open a DB
![Open_DB](https://github.com/sbidy/KeePass-KeyManager/blob/master/Open_DB.gif)
