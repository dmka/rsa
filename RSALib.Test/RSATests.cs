using System.Numerics;
using NUnit.Framework;
using System.Text;
namespace RSALib.Test
{
    public class RSATests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void CanEncryptDecryptMessage()
        {
            var keyGen = new RSAKeyGenerator();
            var key = keyGen.Generate(512);

            var plainText = "the quick brown fox jumped over the lazy dog";
            var data = new BigInteger(Encoding.UTF8.GetBytes(plainText)).ToByteArray();

            var rsa = new RSA(key);

            var cipherText = rsa.Encrypt(data);
            var plainTextBytes = rsa.Decrypt(cipherText);

            var actualPlainText = Encoding.UTF8.GetString(plainTextBytes);

            Assert.AreEqual(plainText, actualPlainText);
        }
    }
}