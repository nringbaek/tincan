using System;
using System.IO;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Tincan.Cryptography
{
    public static class Encryption
    {
        public static string Encrypt(string content, string key)
        {
            using (var aes = Aes.Create())
            {
                if (aes == null)
                    throw new NullReferenceException("Failed to create AES object.");

                using (var encryptor = aes.CreateEncryptor(Hash(key, aes.KeySize / 8), aes.IV))
                using (var memoryStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    using (var writerStream = new StreamWriter(cryptoStream))
                    {
                        memoryStream.Write(aes.IV);
                        writerStream.Write(content);
                    }

                    return Convert.ToBase64String(memoryStream.ToArray());
                }
            }
        }

        public static bool TryDecrypt(string cipherContent, string key, out string decryptedContent)
        {
            try
            {
                using (var aes = Aes.Create())
                {
                    if (aes == null)
                        throw new NullReferenceException("Failed to create AES object.");

                    var cipherBytes = Convert.FromBase64String(cipherContent).AsSpan();
                    var ivBytes = cipherBytes.Slice(0, aes.IV.Length).ToArray();
                    var contentBytes = cipherBytes.Slice(aes.IV.Length).ToArray();

                    var keyS = Hash(key, aes.KeySize / 8);
                    using (var decryptor = aes.CreateDecryptor(keyS, ivBytes))
                    using (var resultStream = new MemoryStream(contentBytes))
                    using (var cryptoStream = new CryptoStream(resultStream, decryptor, CryptoStreamMode.Read))
                    using (var writer = new StreamReader(cryptoStream))
                    {
                        decryptedContent = writer.ReadToEnd();
                        return true;
                    }
                }
            }
            catch
            {
                decryptedContent = "";
                return false;
            }
        }

        public static void GenerateRandomBytes(int count, out byte[] bytes)
        {
            bytes = new byte[count];
            RandomNumberGenerator.Fill(bytes);
        }

        private static byte[] Hash(string value, int byteLength, byte[] salt = null)
        {
            if (salt == null)
                salt = new byte[0];

            return KeyDerivation.Pbkdf2(
                password: value,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: byteLength
            );
        }
    }
}
