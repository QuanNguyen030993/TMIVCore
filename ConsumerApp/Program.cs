using System;
using System.Collections.Generic;
using System.IO;
using TMIVHashing;

string randomKey = TMIVHashing.SaltKey.GenerateSalt32_Hex();
//string saltKey = "afb6de3ec164c2fbfa6d236c1be16bc1"; //Place
string encryptedSecretKey = "f415a1301a747478d6e39bde5f8c0e2f"; // Environment System.Environment.GetEnvironmentVariable("USERNAME");

string localKey = System.Environment.GetEnvironmentVariable("ApplicationSecretKey", EnvironmentVariableTarget.Machine);
string localUserKey = System.Environment.GetEnvironmentVariable("ApplicationUserSecretKey", EnvironmentVariableTarget.User);
System.IO.File.WriteAllText("D:\\key.txt", localKey + localUserKey);
try
{
               
    var key = "9A19103F16F74668BE549A1E7A4F75"; // Project ID
     var enc = SaltKey.EncryptECB(randomKey, key);
    //3PqjezPvHdw3leptkuoIe9dSEnsETX1eBkh6QrELbB50ruy5fUtw2A==
    var saltKey = SaltKey.DecryptECB(enc, key);
    string encryptedKey = KeyVaultLocal.EncryptKey("password@123", encryptedSecretKey, saltKey);
    string password = KeyVaultLocal.DecryptKey(encryptedKey, encryptedSecretKey, saltKey);
}
catch (Exception ex)
{

}









