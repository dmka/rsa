using System;
using System.Numerics;
using System.Security.Cryptography;

namespace RSALib
{
    public class RSAKeyGenerator
    {
        private PrimeGenerator _primeGenerator;


        public RSAKeyGenerator()
        {
            _primeGenerator = new PrimeGenerator(new RNGCryptoServiceProvider());
        }

        public RSAKey Generate(int keyBitLength)
        {
            if (keyBitLength < 0)
                throw new ArgumentOutOfRangeException(nameof(keyBitLength));

            var p = _primeGenerator.GetRandomPrime(keyBitLength / 2, min: 3);
            var q = _primeGenerator.GetRandomPrime(keyBitLength / 2, min: 3);

            var n = p * q;

            // lcm(n) = |(p-1)*(p-1)| / gcd(p-1, p-1)
            var tn = ((p - 1) * (q - 1)) / BigInteger.GreatestCommonDivisor(p - 1, q - 1);

            BigInteger e = 41;

            var d = ModInverse(e, tn);
            if (d == -1)
                return Generate(keyBitLength);

            return new RSAKey { N = n, E = e, D = d, P = p, Q = q };
        }

        // using https://en.wikipedia.org/wiki/Extended_Euclidean_algorithm
        private static BigInteger ModInverse(BigInteger a, BigInteger b)
        {
            BigInteger t, nt, r, nr, q;
            if (b < 0) b = -b;
            if (a < 0) a = b - (-a % b);
            t = 0; nt = 1; r = b; nr = a % b;
            while (nr != 0)
            {
                q = r / nr;
                (t, nt) = (nt, t - q * nt);
                (r, nr) = (nr, r - q * nr);
            }
            if (r > 1) return BigInteger.MinusOne;  // No inverse
            if (t < 0) t += b;

            return t;
        }
    }
}

