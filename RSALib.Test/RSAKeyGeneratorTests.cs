using System;
using System.Numerics;
using NUnit.Framework;

namespace RSALib.Test
{
    public class RSAKeyGeneratorTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void CanGenerateValidKey()
        {
            var keyGen = new RSAKeyGenerator();

            var key = keyGen.Generate(512);

            var plainText = new BigInteger(123456789);
            var cipherText = BigInteger.ModPow(plainText, key.E, key.N);
            var actualPlainText = BigInteger.ModPow(cipherText, key.D, key.N);

            Assert.AreEqual(plainText, actualPlainText);
        }
    }
}