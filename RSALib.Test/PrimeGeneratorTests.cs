using System;
using NUnit.Framework;

namespace RSALib.Test
{
    public class PrimeGeneratorTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        [TestCase(8)]
        [TestCase(16)]
        public void CanGetRandomPrime(int bitLength)
        {
            var pg = new PrimeGenerator();

            for (int i = 0; i < 3; i++)
            {
                var prime = pg.GetRandomPrime(bitLength, 3);
                var isPrime = IsPrime((long)prime);
                Assert.True(isPrime);
            }
        }

        private static bool IsPrime(long n)
        {
            if (n <= 1) return false;
            if (n == 2) return true;
            if (n % 2 == 0) return false;

            var b = (long)Math.Sqrt(n);

            for (long i = 3; i <= b; i += 2)
                if (n % i == 0)
                    return false;

            return true;
        }
    }
}