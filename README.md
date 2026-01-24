# TMIVCore Solution

Giải pháp toàn bộ cho việc mã hóa và quản lý Key/Password sử dụng DPAPI local.

## 📁 Solution Structure

```
TMIVCore/
├── TMIVCore.sln                  # Solution file (Visual Studio 2022)
├── DPAPIKeyVault/                # Core library project
│   ├── src/
│   │   ├── KeyVaultLocal.cs      # Encryption/Decryption
│   │   ├── ConfigManager.cs      # File management
│   │   └── Program.cs            # Demo
│   ├── DPAPIKeyVault.csproj
│   └── README.md
├── ConsumerApp/                  # Consumer project (sử dụng DLL)
│   ├── Program.cs                # Demo usage
│   ├── ConsumerApp.csproj        # Reference DLL
│   └── README.md
└── README.md                     # This file
```

## 🚀 Quick Start

### Mở Solution trong Visual Studio 2022

1. **Mở file:** `TMIVCore.sln`
2. **Solution Explorer:** Thấy 2 projects
   - DPAPIKeyVault (Core Library)
   - ConsumerApp (Consumer)
3. **Build All:** `Ctrl+Shift+B`
4. **Run:** Chọn project → `Ctrl+F5`

### Build & Run từ CLI

```bash
# Build cả 2 projects
dotnet build

# Chạy DPAPIKeyVault (demo library)
cd DPAPIKeyVault
dotnet run

# Chạy ConsumerApp (demo usage)
cd ..\ConsumerApp
dotnet run
```

## 📚 Projects

### 1. DPAPIKeyVault (Core Library)
- **Mục đích:** Cung cấp API mã hóa/giải mã sử dụng DPAPI
- **Exports:** `KeyVaultLocal`, `ConfigManager`
- **Output:** DLL + EXE

**Main Classes:**
```csharp
// Mã hóa/Giải mã
KeyVaultLocal.EncryptKey(string)
KeyVaultLocal.DecryptKey(string)

// Quản lý config file
ConfigManager.SaveEncryptedKey(string key, string name, string description)
ConfigManager.LoadDecryptedKey() : string
```

### 2. ConsumerApp (Demo/Consumer)
- **Mục đích:** Show cách sử dụng DPAPIKeyVault DLL
- **References:** DPAPIKeyVault.dll
- **Demo Scenarios:**
  1. Mã hóa password lần đầu
  2. Đọc password từ config
  3. Quản lý multiple credentials
  4. Xem chi tiết config

## 🔐 Tính Năng

✅ **Encryption** - DPAPI (Windows native)  
✅ **Local Storage** - JSON config files  
✅ **CurrentUser Scope** - Per-user encryption  
✅ **LocalMachine Scope** - Machine-wide (optional)  
✅ **Multiple Keys** - Quản lý nhiều credentials  
✅ **No Cloud Required** - Hoàn toàn offline  

## 📖 Workflow

### Setup (Lần đầu)
```
1. Gọi ConfigManager.SaveEncryptedKey()
   ↓
2. Password được mã hóa bằng DPAPI
   ↓
3. Encrypted key lưu vào JSON file
   ↓
4. File safe để commit vào git
```

### Runtime (Khi ứng dụng chạy)
```
1. Gọi ConfigManager.LoadDecryptedKey()
   ↓
2. Đọc encrypted key từ JSON file
   ↓
3. Giải mã bằng DPAPI
   ↓
4. Return plaintext password
   ↓
5. Sử dụng password (database, API, etc.)
```

## 🛠️ Phát Triển

### Thêm Feature Mới

Sửa trong project **DPAPIKeyVault**:
- Thêm method mới trong `KeyVaultLocal.cs`
- Thêm helper trong `ConfigManager.cs`
- Test trong `Program.cs`

**Sau đó:**
```bash
cd DPAPIKeyVault
dotnet build
# ConsumerApp sẽ tự động reference updated DLL
```

### Debug

1. **Set Breakpoint** trong DPAPIKeyVault
2. **Right-click ConsumerApp** → Set as Startup Project
3. **F5** để debug

## 📋 Dependencies

| Package | Version | Purpose |
|---------|---------|---------|
| System.Security.Cryptography.ProtectedData | 4.7.0 | DPAPI |
| System.Text.Json | 4.7.1 | JSON handling |

Auto installed via NuGet.

## 🔄 Git Workflow

### .gitignore
```
bin/
obj/
*.dll
*.pdb
*.exe
app-config.json  # (hoặc để encrypted files?)
```

### Safe to Commit
✅ Source code  
✅ `.csproj` files  
✅ `README.md`  
✅ Config **STRUCTURE** (nếu file template)  

### NOT Safe to Commit
❌ `bin/` folder  
❌ `obj/` folder  
❌ User passwords (plaintext)  

### Production Files
```
# Encrypted files CÓ THỂ commit vì:
app-config.json         # ← Encrypted, safe
database_prod.json      # ← Encrypted, safe
api_keys.json           # ← Encrypted, safe

# Chỉ production user/machine mới decrypt được
```

## ❓ FAQ

**Q: Tại sao cần DLL?**  
A: Tách concerns - Library riêng, consumer riêng. Reusable across projects.

**Q: Có thể edit `.sln` file?**  
A: Có, nhưng VS 2022 sẽ tự động maintain. Để VS quản lý.

**Q: Build output ở đâu?**  
A: Mỗi project có folder `bin/Debug/net8.0/`

**Q: Release build?**  
A: `dotnet build -c Release` hoặc VS → Build → Release Config

## 📱 Next Steps

1. ✅ Open `TMIVCore.sln` trong Visual Studio 2022
2. ✅ Build all projects (`Ctrl+Shift+B`)
3. ✅ Run DPAPIKeyVault demo
4. ✅ Run ConsumerApp demo
5. ✅ Integrate vào project thực của bạn
6. ✅ Replace `ConsumerApp` với project của bạn

## 📞 Support

- [DPAPIKeyVault README](./DPAPIKeyVault/README.md)
- [ConsumerApp README](./ConsumerApp/README.md)
- Visual Studio 2022 Solution Help

---

**Ready to manage passwords securely with DPAPI! 🔐**
