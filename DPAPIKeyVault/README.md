# DPAPI Local Key Vault

Giải pháp mã hóa Key local sử dụng DPAPI (Data Protection API) của Windows. **Không cần Cloud account, hoàn toàn offline và bảo mật.**

## 🎯 Tính Năng

✅ **Mã hóa/Giải mã local** - Sử dụng Windows DPAPI  
✅ **Lưu trữ Config** - JSON file với encrypted key  
✅ **Multi-user safe** - Mỗi user có key encryption khác nhau  
✅ **Không cần Cloud** - Chạy offline, không phụ thuộc dịch vụ bên ngoài  
✅ **Simple API** - Dễ sử dụng, ít dependencies  

## 🚀 Cách Sử Dụng

### 1. Mã hóa một Key
```csharp
string plainKey = "my-secret-api-key";
string encrypted = KeyVaultLocal.EncryptKey(plainKey);
Console.WriteLine(encrypted); // AgAA...
```

### 2. Giải mã Key
```csharp
string decrypted = KeyVaultLocal.DecryptKey(encrypted);
Console.WriteLine(decrypted); // my-secret-api-key
```

### 3. Lưu Key vào Config File
```csharp
var config = new ConfigManager("keyvault.json");
config.SaveEncryptedKey("my-secret-key", "API_KEY", "Production API Key");
```

### 4. Đọc Key từ Config
```csharp
var config = new ConfigManager("keyvault.json");
string key = config.LoadDecryptedKey();
```

## 📋 Project Structure

```
DPAPIKeyVault/
├── src/
│   ├── KeyVaultLocal.cs      # Main encryption/decryption
│   ├── ConfigManager.cs      # File management
│   └── Program.cs            # Demo application
├── DPAPIKeyVault.csproj      # Project file
├── README.md                 # This file
└── keyvault.json            # Generated config (after running)
```

## 🔐 DPAPI Scopes

### CurrentUser (Khuyên dùng)
```csharp
KeyVaultLocal.EncryptKey(key)        // Encrypt for current user
KeyVaultLocal.DecryptKey(encrypted)  // Decrypt for current user
```
✓ Mỗi user khác nhau = key khác nhau  
✓ Rất bảo mật  

### LocalMachine
```csharp
KeyVaultLocal.EncryptKeyMachine(key)        // Encrypt for machine
KeyVaultLocal.DecryptKeyMachine(encrypted)  // Decrypt for machine
```
✓ Tất cả user trên machine có thể giải mã  
✓ Dùng cho service/application accounts  

## 🛠️ Build & Run

```bash
dotnet build
dotnet run
```

Kết quả:
```
✓ Encrypted key: AgAA...
✓ Decrypted key: my-secret-api-key
✓ Config saved: keyvault.json
```

## ⚠️ Bảo Mật - Điều Cần Biết

| Điều | Chi Tiết |
|-----|---------|
| **Người dùng** | Chỉ user đó có thể decrypt |
| **Machine** | Chỉ machine đó có thể decrypt (nếu dùng LocalMachine) |
| **Plaintext** | Key không bao giờ lưu plaintext trong file |
| **Memory** | Key tồn tại plaintext khi sử dụng - bình thường |
| **Git** | ✅ SAFE lưu encrypted key vào git |
| **DLL** | ✅ SAFE - key được giải mã runtime |

## 📦 Integration vào Project Khác

1. Copy `KeyVaultLocal.cs` + `ConfigManager.cs` vào project của bạn
2. Install package: `System.Security.Cryptography.ProtectedData`
3. Sử dụng:

```csharp
// Lần đầu (mã hóa)
var mgr = new ConfigManager("config/keyvault.json");
mgr.SaveEncryptedKey("your-secret-key", "MY_KEY");

// Sử dụng trong app
var key = mgr.LoadDecryptedKey();
```

## 🔄 So Sánh với Các Giải Pháp Khác

| Giải Pháp | Local | Bảo Mật | Phức Tạp | Git Safe |
|-----------|-------|---------|---------|----------|
| **DPAPI** | ✅ | ⭐⭐⭐⭐⭐ | ⭐ | ✅ |
| Environment Vars | ❌ | ⭐⭐ | ⭐ | ❌ |
| Azure Key Vault | ❌ | ⭐⭐⭐⭐⭐ | ⭐⭐⭐ | ✅ |
| Plaintext Config | ✅ | ⭐ | ⭐ | ❌ |

## 📝 Ví Dụ Thực Tế

### Setup lần đầu (Dev Machine)
```csharp
// appsettings.json không có key
{
  "database": {
    "encryptedConnectionString": "AgAA..."
  }
}

// Startup code
var config = new ConfigManager("appsettings.json");
string connectionString = config.LoadDecryptedKey();
var db = new SqlConnection(connectionString);
```

### Multiple Keys
```csharp
var vault = new ConfigManager();

// Lưu multiple keys
vault.SaveEncryptedKey("db_password_123", "DB_PASSWORD");
vault.SaveEncryptedKey("api_key_xyz", "API_KEY");

// Đọc
string dbPass = vault.LoadDecryptedKey(); // Load từ file
```

## ❓ FAQ

**Q: Có bị dò tìm key khi ứng dụng chạy?**  
A: Có thể dump memory để lấy plaintext key (bình thường - DPAPI chỉ bảo vệ file storage)

**Q: Có thể dùng trên Production?**  
A: Có, nhưng CurrentUser scope chỉ tốt cho single-user machines. Dùng `LocalMachine` cho service.

**Q: Nếu đổi password Windows?**  
A: Key DPAPI vẫn hoạt động (dùng hardware key bên dưới)

**Q: Portable qua máy khác được không?**  
A: Không - encrypted key chỉ dùng được trên machine/user đó (điều này là điều tốt!)

## 📚 Tài Liệu

- [DPAPI Documentation](https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.protecteddata)
- [Data Protection API](https://learn.microsoft.com/en-us/dotnet/standard/security/encrypting-data)

## 📄 License

MIT License - Tự do sử dụng
