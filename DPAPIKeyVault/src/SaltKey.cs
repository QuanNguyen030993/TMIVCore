using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DPAPIKeyVault
{
    /// <summary>
    /// Config model cho Key Vault
    /// </summary>
    public class SaltKey
    {
        public static string GenerateSalt32_Hex()
        {
            byte[] bytes = new byte[16];
            var sb = new StringBuilder(32);
            foreach (var b in bytes)
                sb.Append(b.ToString("x2"));
            return sb.ToString();
        }
    }

   
}
