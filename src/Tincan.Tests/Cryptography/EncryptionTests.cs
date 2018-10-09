using System.Linq;
using Tincan.Cryptography;
using Xunit;

namespace Tincan.Tests.Cryptography
{
    public class EncryptionTests
    {
        [Fact]
        public void EncryptDecrypt_ReturnsSameContent()
        {
            var content = "super secret content to encrypt";
            var key = "very secret key";

            var encryptedContent = Encryption.Encrypt(content, key);
            Encryption.TryDecrypt(encryptedContent, key, out var decryptedContent);

            Assert.Equal(content, decryptedContent);
        }

        [Fact]
        public void EncryptDecrypt_MustBeSameKey()
        {
            var content = "super secret content to encrypt";
            var encryptedContent = Encryption.Encrypt(content, "1");

            Assert.True(Encryption.TryDecrypt(encryptedContent, "1", out var d1) && content.Equals(d1));
            Assert.False(Encryption.TryDecrypt(encryptedContent, "11", out var d2) && content.Equals(d2));
            Assert.False(Encryption.TryDecrypt(encryptedContent, "2", out var d3) && content.Equals(d3));
        }

        [Fact]
        public void EncryptDecrypt_EncryptSameContent_DifferentIvResult()
        {
            var content = "super secret content to encrypt";
            var entries = new[]
            {
                Encryption.Encrypt(content, "1"),
                Encryption.Encrypt(content, "1"),
                Encryption.Encrypt(content, "1"),
                Encryption.Encrypt(content, "1"),
                Encryption.Encrypt(content, "1")
            };

            Assert.True(entries.Distinct().Count() == entries.Length);
        }

        [Fact]
        public void GenerateRandomBytes_ReturnsCorrectSize()
        {
            Encryption.GenerateRandomBytes(16, out var bytes);
            Assert.True(bytes.Length == 16);
        }
    }
}
