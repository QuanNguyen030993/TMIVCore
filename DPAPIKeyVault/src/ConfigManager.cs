using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DPAPIKeyVault
{
    /// <summary>
    /// Config model cho Key Vault
    /// </summary>
    public class KeyVaultConfig
    {
        [JsonPropertyName("encryptedKey")]
        public string? EncryptedKey { get; set; }

        [JsonPropertyName("keyName")]
        public string? KeyName { get; set; }

        [JsonPropertyName("scope")]
        public string? Scope { get; set; } = "CurrentUser";

        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }
    }

    /// <summary>
    /// Quản lý lưu trữ encrypted key vào JSON config file
    /// </summary>
    public class ConfigManager
    {
        private readonly string _configPath;

        public ConfigManager(string configPath = "keyvault.json")
        {
            _configPath = configPath;
        }

        /// <summary>
        /// Lưu encrypted key vào file config
        /// </summary>
        public void SaveEncryptedKey(string plainKey, string keyName, string? description = null)
        {
            var encryptedKey = KeyVaultLocal.EncryptKey(plainKey);
            var config = new KeyVaultConfig
            {
                EncryptedKey = encryptedKey,
                KeyName = keyName,
                Scope = "CurrentUser",
                CreatedAt = DateTime.Now,
                Description = description
            };

            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(config, options);

            File.WriteAllText(_configPath, json);
            Console.WriteLine($"✓ Đã lưu encrypted key vào: {Path.GetFullPath(_configPath)}");
        }

        /// <summary>
        /// Đọc và giải mã key từ config file
        /// </summary>
        public string LoadDecryptedKey()
        {
            if (!File.Exists(_configPath))
                throw new FileNotFoundException($"Config file không tồn tại: {_configPath}");

            string json = File.ReadAllText(_configPath);
            var options = new JsonSerializerOptions();
            var config = JsonSerializer.Deserialize<KeyVaultConfig>(json, options);

            if (config?.EncryptedKey == null)
                throw new InvalidOperationException("EncryptedKey không tìm thấy trong config file");

            var decryptedKey = KeyVaultLocal.DecryptKey(config.EncryptedKey);
            return decryptedKey;
        }

        /// <summary>
        /// Lấy thông tin chi tiết config
        /// </summary>
        public KeyVaultConfig GetConfig()
        {
            if (!File.Exists(_configPath))
                throw new FileNotFoundException($"Config file không tồn tại: {_configPath}");

            string json = File.ReadAllText(_configPath);
            var options = new JsonSerializerOptions();
            var config = JsonSerializer.Deserialize<KeyVaultConfig>(json, options);

            return config ?? throw new InvalidOperationException("Không thể parse config file");
        }
    }
}
