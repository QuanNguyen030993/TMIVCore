using System;
using System.Collections.Generic;
using DPAPIKeyVault;

string originalKey = "my-secret-api-key-xyz-12345";
string encryptedKey = KeyVaultLocal.EncryptKey(originalKey);
string decryptedKey = KeyVaultLocal.DecryptKey(encryptedKey);

var configManager = new ConfigManager("keyvault.json");
string apiKey = "sk-1234567890abcdef";

try
{
    configManager.SaveEncryptedKey(apiKey, "API_KEY", "API key cho authentication");

}
catch (Exception ex)
{

}

try
{
    var loadedKey = configManager.LoadDecryptedKey();

}
catch (Exception ex)
{

}


try
{
    var config = configManager.GetConfig();
}
catch (Exception ex)
{

}


var keys = new Dictionary<string, string>
{
    { "DB_PASSWORD", "dbpass123456" },
    { "JWT_SECRET", "jwt_secret_xyz" },
    { "API_TOKEN", "token_abc123" }
};

foreach (var kvp in keys)
{
    string encrypted = KeyVaultLocal.EncryptKey(kvp.Value);

}







