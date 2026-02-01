using System;
using System.Collections.Generic;
using System.IO;
using TMIVHashing;

string randomKey = TMIVHashing.SaltKey.GenerateSalt32_Hex();
//string saltKey = "afb6de3ec164c2fbfa6d236c1be16bc1"; //Place
string encryptedSecretKey = "f415a1301a747478d6e39bde5f8c0e2f"; // Environment System.Environment.GetEnvironmentVariable("USERNAME");

string localKey = System.Environment.GetEnvironmentVariable("ApplicationSecretKey", EnvironmentVariableTarget.Machine);
string saltKey = System.Environment.GetEnvironmentVariable("ApplicationSaltKey", EnvironmentVariableTarget.Machine);
string localUserKey = System.Environment.GetEnvironmentVariable("ApplicationUserSecretKey", EnvironmentVariableTarget.User);
System.IO.File.WriteAllText("D:\\key.txt", localKey + localUserKey);
try
{
               
    var key = "9A19103F16F74668BE549A1E7A4F75"; // Project ID
     var enc = SaltKey.EncryptECB(randomKey, key);
    //3PqjezPvHdw3leptkuoIe9dSEnsETX1eBkh6QrELbB50ruy5fUtw2A==
    //var saltKey = SaltKey.DecryptECB(enc, key);
    string encryptedKeySimple = KeyVaultLocal.EncryptKey("password@123", localKey, saltKey);
    string passwordSimple = KeyVaultLocal.DecryptKey(encryptedKeySimple, localKey, saltKey);

    string encryptedKey = KeyVaultLocal.EncryptConnectionStringPassword("password@123", "ApplicationSecretKey", "ApplicationSaltKey", 10);
    string passwordSimpleFail = KeyVaultLocal.DecryptKey(encryptedKey, localKey, saltKey);
    string password = KeyVaultLocal.DecryptConnectionStringPassword(encryptedKey, "ApplicationSecretKey", "ApplicationSaltKey", 10);


    //// Thiết lập env (ví dụ)
    //Environment.SetEnvironmentVariable("ApplicationSecretKey", secretPlain, EnvironmentVariableTarget.Machine);
    //Environment.SetEnvironmentVariable("ApplicationSaltKey", saltPlain, EnvironmentVariableTarget.Machine);

    //// Simple (encrypt bằng key nguyên thủy) => có thể giải được bằng key nguyên thủy
    //var simple = KeyVaultLocal.EncryptKey("password@123", secretPlain, saltPlain);
    //var ok = KeyVaultLocal.DecryptKey(simple, secretPlain, saltPlain); // success

    //// Loop (encrypt bằng khóa đã biến đổi) => KHÔNG giải được bằng key nguyên thủy
    //var loop = KeyVaultLocal.EncryptConnectionStringPassword("password@123", "ApplicationSecretKey", "ApplicationSaltKey", 10);
    //var fail = KeyVaultLocal.DecryptKey(loop, secretPlain, saltPlain); // sẽ sai/throw
    //var okLoop = KeyVaultLocal.DecryptConnectionStringPassword(loop, "ApplicationSecretKey", "ApplicationSaltKey", 10); // success
}
catch (Exception ex)
{

}









