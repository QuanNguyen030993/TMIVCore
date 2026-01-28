using System;
using System.Collections.Generic;
using System.IO;
using DPAPIKeyVault;



string databasePassword = "P@ssw0rd123!DatabaseSecret";
//string saltKey = DPAPIKeyVault.SaltKey.GenerateSalt32_Hex();
string saltKey = "afb6de3ec164c2fbfa6d236c1be16bc1";
string encryptedSecretKey = "Tmiv#1234";
var text = System.Environment.GetEnvironmentVariable("USERNAME");
try
{
    string encryptedKey = KeyVaultLocal.EncryptKey("password@123", encryptedSecretKey, saltKey);
    string password = KeyVaultLocal.DecryptKey(encryptedKey, encryptedSecretKey, saltKey);
}
catch (Exception ex)
{

}













