using System;
using System.Security.Cryptography;
using System.Text;

namespace DPAPIKeyVault
{
    /// <summary>
    /// Local Key Vault sử dụng DPAPI (Data Protection API) của Windows
    /// Mã hóa/giải mã key mà không cần cloud service
    /// </summary>
    public class KeyVaultLocal
    {
        /// <summary>
        /// Mã hóa key bằng DPAPI (CurrentUser scope)
        /// </summary>
        public static string EncryptKey(string plainKey)
        {
            if (string.IsNullOrEmpty(plainKey))
                throw new ArgumentException("Key không được để trống", nameof(plainKey));

            try
            {
                byte[] dataToEncrypt = Encoding.UTF8.GetBytes(plainKey);
                byte[] encryptedData = ProtectedData.Protect(
                    dataToEncrypt,
                    null,
                    DataProtectionScope.CurrentUser
                );
                return Convert.ToBase64String(encryptedData);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi mã hóa key", ex);
            }
        }

        /// <summary>
        /// Giải mã key từ DPAPI
        /// </summary>
        public static string DecryptKey(string encryptedKey)
        {
            if (string.IsNullOrEmpty(encryptedKey))
                throw new ArgumentException("Encrypted key không được để trống", nameof(encryptedKey));

            try
            {
                byte[] encryptedData = Convert.FromBase64String(encryptedKey);
                byte[] decryptedData = ProtectedData.Unprotect(
                    encryptedData,
                    null,
                    DataProtectionScope.CurrentUser
                );
                return Encoding.UTF8.GetString(decryptedData);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi giải mã key", ex);
            }
        }

        /// <summary>
        /// Mã hóa key với LocalMachine scope (cho service/application account)
        /// </summary>
        public static string EncryptKeyMachine(string plainKey)
        {
            if (string.IsNullOrEmpty(plainKey))
                throw new ArgumentException("Key không được để trống", nameof(plainKey));

            try
            {
                byte[] dataToEncrypt = Encoding.UTF8.GetBytes(plainKey);
                byte[] encryptedData = ProtectedData.Protect(
                    dataToEncrypt,
                    null,
                    DataProtectionScope.LocalMachine
                );
                return Convert.ToBase64String(encryptedData);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi mã hóa key (Machine scope)", ex);
            }
        }

        /// <summary>
        /// Giải mã key từ LocalMachine scope
        /// </summary>
        public static string DecryptKeyMachine(string encryptedKey)
        {
            if (string.IsNullOrEmpty(encryptedKey))
                throw new ArgumentException("Encrypted key không được để trống", nameof(encryptedKey));

            try
            {
                byte[] encryptedData = Convert.FromBase64String(encryptedKey);
                byte[] decryptedData = ProtectedData.Unprotect(
                    encryptedData,
                    null,
                    DataProtectionScope.LocalMachine
                );
                return Encoding.UTF8.GetString(decryptedData);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi giải mã key (Machine scope)", ex);
            }
        }
    }
}
