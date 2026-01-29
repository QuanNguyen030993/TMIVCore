using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace TMIVHashing
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
        public static string EncryptKey(string plainKey, string aesKey32, string salt32)
        {
            if (string.IsNullOrEmpty(plainKey))
                throw new ArgumentException("Key không được để trống", nameof(plainKey));

            try
            {
                byte[] dataToEncrypt = Encoding.UTF8.GetBytes(plainKey);
                byte[] entropy = Encoding.UTF8.GetBytes(salt32); // salt/entropy
                byte[] key = SHA256.HashData(Encoding.UTF8.GetBytes(aesKey32)); // 32 bytes
                byte[] iv = new byte[16];                 // IV 16 bytes

                using var aes = Aes.Create();
                aes.Key = key;
                aes.IV = iv;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                byte[] ct;
                using (var enc = aes.CreateEncryptor())
                    ct = enc.TransformFinalBlock(dataToEncrypt, 0, dataToEncrypt.Length);

                // pack: [iv][cipher]
                byte[] blob = new byte[iv.Length + ct.Length];
                Buffer.BlockCopy(iv, 0, blob, 0, iv.Length);
                Buffer.BlockCopy(ct, 0, blob, iv.Length, ct.Length);
                byte[] encryptedData = ProtectedData.Protect(blob, entropy, DataProtectionScope.CurrentUser);
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
        public static string DecryptKey(string cipherB64, string aesKey32, string salt32)
        {
            if (string.IsNullOrEmpty(cipherB64))
                throw new ArgumentException("Encrypted key không được để trống", nameof(cipherB64));

            try
            {
                byte[] key = SHA256.HashData(Encoding.UTF8.GetBytes(aesKey32));
                byte[] entropy = Encoding.UTF8.GetBytes(salt32);

                byte[] dp = Convert.FromBase64String(cipherB64);
                byte[] blob = ProtectedData.Unprotect(dp, entropy, DataProtectionScope.CurrentUser);

                // unpack
                byte[] iv = blob.Take(16).ToArray();
                byte[] ct = blob.Skip(16).ToArray();

                using var aes = Aes.Create();
                aes.Key = key;
                aes.IV = iv;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                byte[] decryptedData;
                using (var dec = aes.CreateDecryptor())
                    decryptedData = dec.TransformFinalBlock(ct, 0, ct.Length);

                return Encoding.UTF8.GetString(decryptedData);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi giải mã key", ex);
            }
        }

       
    }
}
