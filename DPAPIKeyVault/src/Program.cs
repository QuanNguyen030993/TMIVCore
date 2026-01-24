using System;
using System.Collections.Generic;
using DPAPIKeyVault;

Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
Console.WriteLine("║     DPAPI Local Key Vault - Demo Application               ║");
Console.WriteLine("║     Mã hóa/Giải mã Key không dùng Cloud                    ║");
Console.WriteLine("╚════════════════════════════════════════════════════════════╝\n");

// === Demo 1: Basic Encryption/Decryption ===
Console.WriteLine("📌 DEMO 1: Mã hóa và Giải mã Key cơ bản");
Console.WriteLine("─".PadRight(60, '─'));

string originalKey = "my-secret-api-key-xyz-12345";
Console.WriteLine($"Plain Key: {originalKey}");

// Mã hóa
string encryptedKey = KeyVaultLocal.EncryptKey(originalKey);
Console.WriteLine($"Encrypted: {encryptedKey.Substring(0, 50)}...");

// Giải mã
string decryptedKey = KeyVaultLocal.DecryptKey(encryptedKey);
Console.WriteLine($"Decrypted: {decryptedKey}");
Console.WriteLine($"✓ Match: {decryptedKey == originalKey}\n");

// === Demo 2: Config File Management ===
Console.WriteLine("📌 DEMO 2: Lưu trữ Key vào Config File");
Console.WriteLine("─".PadRight(60, '─'));

var configManager = new ConfigManager("keyvault.json");
string apiKey = "sk-1234567890abcdef";

try
{
    configManager.SaveEncryptedKey(apiKey, "API_KEY", "API key cho authentication");
    Console.WriteLine();
}
catch (Exception ex)
{
    Console.WriteLine($"✗ Lỗi: {ex.Message}");
}

// === Demo 3: Load từ Config File ===
Console.WriteLine("📌 DEMO 3: Đọc Key từ Config File");
Console.WriteLine("─".PadRight(60, '─'));

try
{
    var loadedKey = configManager.LoadDecryptedKey();
    Console.WriteLine($"Loaded Key: {loadedKey}");
    Console.WriteLine($"✓ Match: {loadedKey == apiKey}\n");
}
catch (Exception ex)
{
    Console.WriteLine($"✗ Lỗi: {ex.Message}\n");
}

// === Demo 4: Show Config Details ===
Console.WriteLine("📌 DEMO 4: Chi tiết Config");
Console.WriteLine("─".PadRight(60, '─'));

try
{
    var config = configManager.GetConfig();
    Console.WriteLine($"Key Name: {config.KeyName}");
    Console.WriteLine($"Scope: {config.Scope}");
    Console.WriteLine($"Created: {config.CreatedAt:yyyy-MM-dd HH:mm:ss}");
    Console.WriteLine($"Description: {config.Description}");
    Console.WriteLine($"Encrypted (truncated): {config.EncryptedKey?.Substring(0, 30)}...\n");
}
catch (Exception ex)
{
    Console.WriteLine($"✗ Lỗi: {ex.Message}\n");
}

// === Demo 5: Multiple Keys ===
Console.WriteLine("📌 DEMO 5: Mã hóa Multiple Keys");
Console.WriteLine("─".PadRight(60, '─'));

var keys = new Dictionary<string, string>
{
    { "DB_PASSWORD", "dbpass123456" },
    { "JWT_SECRET", "jwt_secret_xyz" },
    { "API_TOKEN", "token_abc123" }
};

foreach (var kvp in keys)
{
    string encrypted = KeyVaultLocal.EncryptKey(kvp.Value);
    Console.WriteLine($"{kvp.Key}: {encrypted.Substring(0, 40)}...");
}

Console.WriteLine("\n✓ Demo hoàn thành!");
Console.WriteLine("\n💡 Ghi chú:");
Console.WriteLine("  • Mỗi user/machine có key khác nhau");
Console.WriteLine("  • Chỉ user/machine đó mới có thể giải mã");
Console.WriteLine("  • Safe cho local development");
Console.WriteLine("  • Không nên lưu key trong source code");
