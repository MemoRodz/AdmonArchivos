using ArchivosMan.BLL.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace ArchivosMan.BLL.Implementacion
{
    public class CryptoService : ICryptoService
    {
        private readonly byte[] _key;

        private static byte[] DerivarKey(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new InvalidOperationException("Crypto:Key no está configurada.");
            using var sha256 = SHA256.Create();
            return sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

        }
        public CryptoService(IConfiguration config)
        {
            var password = config["Crypto:Key"];
            _key = DerivarKey(password!);
        }
        public string Encrypt(string plainText)
        {
            using var aes = Aes.Create();
            aes.KeySize = 256;
            aes.Key = _key;
            aes.GenerateIV();
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            var plainBytes = Encoding.UTF8.GetBytes(plainText);
            var cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
            var resultBytes = aes.IV.Concat(cipherBytes).ToArray();
            return Convert.ToBase64String(resultBytes);
        }
        public string Decrypt(string cipherText)
        {
            var full = Convert.FromBase64String(cipherText);
            var iv = full[..16];
            var cipherBytes = full[16..];

            using var aes = Aes.Create();
            aes.KeySize = 256;
            aes.Key = _key;
            aes.IV = iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            var plainBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
            return Encoding.UTF8.GetString(plainBytes);
        }

    }
}
