using System;
using System.Collections.Generic;
using System.IO;
using DPAPIKeyVault;

Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
Console.WriteLine("║     Consumer App - Sử dụng DPAPIKeyVault Library            ║");
Console.WriteLine("║     Đọc encrypted password từ config file                  ║");
Console.WriteLine("╚════════════════════════════════════════════════════════════╝\n");

// === Scenario 1: Lần đầu tiên - Mã hóa password ===
Console.WriteLine("📌 SCENARIO 1: Setup lần đầu (Mã hóa password)");
Console.WriteLine("─".PadRight(60, '─'));

string databasePassword = "P@ssw0rd123!DatabaseSecret";
var vault = new ConfigManager("app-config.json");

try
{
    Console.WriteLine($"Password gốc: {databasePassword}");
    
    // Lưu encrypted password
    vault.SaveEncryptedKey(databasePassword, "DB_PASSWORD", "Database connection password");
    Console.WriteLine("✓ Password đã được mã hóa và lưu vào: app-config.json\n");
}
catch (Exception ex)
{
    Console.WriteLine($"✗ Lỗi: {ex.Message}\n");
}

// === Scenario 2: Ứng dụng chạy lần tiếp theo - Đọc password ===
Console.WriteLine("📌 SCENARIO 2: Ứng dụng chạy (Đọc password từ config)");
Console.WriteLine("─".PadRight(60, '─'));

try
{
    var vaultReader = new ConfigManager("app-config.json");
    string decryptedPassword = vaultReader.LoadDecryptedKey();
    
    Console.WriteLine("✓ Password được giải mã từ config");
    Console.WriteLine($"Decrypted: {decryptedPassword}");
    Console.WriteLine($"Match gốc: {decryptedPassword == databasePassword}\n");
    
    // Giả lập sử dụng password để connect database
    Console.WriteLine("💾 Đang kết nối database với password...");
    Console.WriteLine($"   Connection String: Server=localhost;Password={decryptedPassword};");
    Console.WriteLine("✓ Kết nối thành công!\n");
}
catch (Exception ex)
{
    Console.WriteLine($"✗ Lỗi: {ex.Message}\n");
}

// === Scenario 3: Multiple credentials ===
Console.WriteLine("📌 SCENARIO 3: Quản lý Multiple Credentials");
Console.WriteLine("─".PadRight(60, '─'));

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
        Console.WriteLine($"✓ {cred.Key} - Saved");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"✗ {cred.Key} - Error: {ex.Message}");
    }
}

Console.WriteLine();

// === Scenario 4: Đọc config details ===
Console.WriteLine("📌 SCENARIO 4: Xem Chi tiết Config");
Console.WriteLine("─".PadRight(60, '─'));

try
{
    var configMgr = new ConfigManager("app-config.json");
    var config = configMgr.GetConfig();
    
    Console.WriteLine($"Key Name: {config.KeyName}");
    Console.WriteLine($"Scope: {config.Scope}");
    Console.WriteLine($"Created: {config.CreatedAt:yyyy-MM-dd HH:mm:ss}");
    Console.WriteLine($"Description: {config.Description}");
    Console.WriteLine($"Encrypted (first 40 chars): {config.EncryptedKey?.Substring(0, 40)}...");
}
catch (Exception ex)
{
    Console.WriteLine($"✗ Lỗi: {ex.Message}");
}

Console.WriteLine("\n✓ Demo hoàn thành!");
Console.WriteLine("\n💡 Ghi chú:");
Console.WriteLine("  • Mỗi file config có 1 encrypted key");
Console.WriteLine("  • Password được giải mã khi load from config");
Console.WriteLine("  • Chỉ user/machine đó mới giải mã được");
Console.WriteLine("  • Không cần lưu password trong source code");
