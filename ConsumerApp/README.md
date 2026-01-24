# Consumer App - Sử dụng DPAPIKeyVault Library

Ứng dụng demo show cách **add DLL từ DPAPIKeyVault** vào project khác và sử dụng để quản lý encrypted passwords.

## 📁 Structure

```
ConsumerApp/
├── Program.cs              # Demo application
├── ConsumerApp.csproj      # Project file (reference DLL)
├── README.md               # This file
├── app-config.json         # Generated (encrypted password)
├── database_prod.json      # Generated
├── api_keys.json           # Generated
└── jwt_secret.json         # Generated
```

## 🔗 Cách Reference DLL

Trong file `.csproj`:

```xml
<ItemGroup>
  <Reference Include="DPAPIKeyVault">
    <HintPath>..\DPAPIKeyVault\bin\Debug\net8.0\DPAPIKeyVault.dll</HintPath>
  </Reference>
</ItemGroup>
```

## 🚀 Cách Sử Dụng

### 1. Lần đầu - Mã hóa password
```csharp
using DPAPIKeyVault;

var vault = new ConfigManager("app-config.json");
vault.SaveEncryptedKey("MyPassword123", "DB_PASSWORD", "Database password");
```

**Output:** `app-config.json` được tạo với encrypted password

### 2. Ứng dụng chạy - Đọc password
```csharp
var vault = new ConfigManager("app-config.json");
string password = vault.LoadDecryptedKey();

// Sử dụng password
var connection = new SqlConnection($"Server=localhost;Password={password};");
connection.Open();
```

### 3. Multiple Credentials
```csharp
// Mã hóa nhiều passwords
var vault1 = new ConfigManager("db-config.json");
vault1.SaveEncryptedKey("DbPassword", "DB");

var vault2 = new ConfigManager("api-config.json");
vault2.SaveEncryptedKey("ApiKey", "API");

// Đọc chúng
string dbPass = new ConfigManager("db-config.json").LoadDecryptedKey();
string apiKey = new ConfigManager("api-config.json").LoadDecryptedKey();
```

## 🏗️ Build & Run

```bash
# Đảm bảo DPAPIKeyVault đã build
cd ..\DPAPIKeyVault
dotnet build

# Build ConsumerApp
cd ..\ConsumerApp
dotnet build

# Run
dotnet run
```

## 💾 Config File Format

```json
{
  "encryptedKey": "AQAAANCMnd8BFdERjHoAwE/Cl+sBAAA...",
  "keyName": "DB_PASSWORD",
  "scope": "CurrentUser",
  "createdAt": "2026-01-25T02:30:00",
  "description": "Database connection password"
}
```

## 🔐 Bảo Mật Notes

| Điều | Chi Tiết |
|-----|---------|
| **File lưu trữ** | Encrypted key safe lưu vào git |
| **Decrypt** | Chỉ user/machine đó mới giải mã |
| **Memory** | Password ở plaintext khi chạy (bình thường) |
| **Source Code** | Không lưu password trong code |
| **Transport** | Mã hóa config file khi transmit |

## 📊 Use Cases

### Web Application
```csharp
// Startup
var dbConfig = new ConfigManager("config/database.json");
string connectionString = dbConfig.LoadDecryptedKey();
services.AddDbContext<AppDbContext>(opt => 
    opt.UseSqlServer(connectionString)
);
```

### Windows Service
```csharp
// Service startup code
var vault = new ConfigManager("C:\\AppConfig\\vault.json");
string apiKey = vault.LoadDecryptedKey();
var client = new HttpClient();
client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
```

### Multi-environment
```csharp
// Dev
var devVault = new ConfigManager("config/dev.json");
var devKey = devVault.LoadDecryptedKey();

// Prod (khác file)
var prodVault = new ConfigManager("config/prod.json");
var prodKey = prodVault.LoadDecryptedKey();
```

## ❓ FAQ

**Q: Liệu có cần copy System.Security.Cryptography.ProtectedData.dll?**  
A: NuGet sẽ tự động resolve dependency khi build.

**Q: Có thể share config file qua git?**  
A: Có, encrypted key safe share. Nhưng chỉ owner user/machine mới decrypt được.

**Q: Nếu lost app-config.json?**  
A: Mất config = mất password (không recover được). Nên backup.

**Q: Có thể change password?**  
A: Xóa file cũ, tạo config mới với password mới:
```csharp
File.Delete("app-config.json");
var newVault = new ConfigManager("app-config.json");
newVault.SaveEncryptedKey("NewPassword123", "DB_PASSWORD");
```

## 📚 Related Projects

- **DPAPIKeyVault** - Core library (parent project)
- Documentation: [README.md](../DPAPIKeyVault/README.md)

---

✅ Ready to use! Just add reference and load passwords securely.
