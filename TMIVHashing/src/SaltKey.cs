using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TMIVHashing
{
    /// <summary>
    /// </summary>
    public class SaltKey
    {
        public static string GenerateSalt32_Hex()
        {
            byte[] bytes = new byte[16]; 
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
            }
            var sb = new StringBuilder(32);
            foreach (var b in bytes)
                sb.Append(b.ToString("x2"));
            return sb.ToString(); 
        }
        public static byte[] BuildKey24(string keyText)
        {
            using var sha256 = SHA256.Create();
            var hash32 = sha256.ComputeHash(Encoding.UTF8.GetBytes(keyText)); // 32 bytes

            var key24 = new byte[24];
            Buffer.BlockCopy(hash32, 0, key24, 0, 24);

            // tránh weak key (TripleDES có danh sách weak keys)
            if (TripleDES.IsWeakKey(key24))
                throw new CryptographicException("Key rơi vào weak key của TripleDES. Hãy đổi keyText khác.");

            return key24;
        }

        public static string EncryptECB(string plainText, string keyText)
        {
            using var tdes = TripleDES.Create();
            tdes.Key = BuildKey24(keyText);
            tdes.Mode = CipherMode.ECB;              // không IV
            tdes.Padding = PaddingMode.PKCS7;

            var data = Encoding.UTF8.GetBytes(plainText);
            using var enc = tdes.CreateEncryptor();
            return Convert.ToBase64String(enc.TransformFinalBlock(data, 0, data.Length));
        }

        public static string DecryptECB(string cipherBase64, string keyText)
        {
            using var tdes = TripleDES.Create();
            tdes.Key = BuildKey24(keyText);
            tdes.Mode = CipherMode.ECB;              // không IV
            tdes.Padding = PaddingMode.PKCS7;

            var data = Convert.FromBase64String(cipherBase64);
            using var dec = tdes.CreateDecryptor();
            return Encoding.UTF8.GetString(dec.TransformFinalBlock(data, 0, data.Length));
        }
    }

}

   
