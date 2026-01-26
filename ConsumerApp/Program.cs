using System;
using System.Collections.Generic;
using System.IO;
using DPAPIKeyVault;



string databasePassword = "P@ssw0rd123!DatabaseSecret";
var vault = new ConfigManager("app-config.json");

try
{

    vault.SaveEncryptedKey(databasePassword, "DB_PASSWORD", "Database connection password");
}
catch (Exception ex)
{

}


try
{
    var vaultReader = new ConfigManager("app-config.json");
    string decryptedPassword = vaultReader.LoadDecryptedKey();

}
catch (Exception ex)
{

}


var credentials = new Dictionary<string, (string password, string description)>
{
    { "database_prod.json", ("DbProd@2026!Secure", "Production Database") },
    { "api_keys.json", ("sk_live_abc123def456", "Payment API Key") },
    { "jwt_secret.json", ("jwt_secret_xyz_789_long_key", "JWT Secret") }
};

foreach (var cred in credentials)
{
    try
    {
        var mgr = new ConfigManager(cred.Key);
        mgr.SaveEncryptedKey(cred.Value.password, Path.GetFileNameWithoutExtension(cred.Key), cred.Value.description);

    }
    catch (Exception ex)
    {

    }
}


try
{
    var configMgr = new ConfigManager("app-config.json");
    var config = configMgr.GetConfig();


}
catch (Exception ex)
{

}







