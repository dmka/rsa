using System;
using System.Numerics;
using System.Security.Cryptography;

namespace RSALib
{
    public class PrimeNumberGenerator
    {
        private readonly RandomNumberGenerator _randomNumGenerator;

        public PrimeNumberGenerator(RandomNumberGenerator randomNumGenerator)
        {
            _randomNumGenerator = randomNumGenerator;
        }

        public PrimeNumberGenerator()
        : this(new RNGCryptoServiceProvider())
        {

        }

        public BigInteger GetRandomPrime(int bitLength, int min)
        {
            if (bitLength < 8)
                throw new ArgumentOutOfRangeException(nameof(bitLength));

            if (min < 3)
                throw new ArgumentOutOfRangeException(nameof(min));


            var buffer = new byte[bitLength / 8 + (bitLength % 8 > 0 ? 1 : 0)];
            var max = BigInteger.One << bitLength;
            BigInteger n;

            do
            {
                _randomNumGenerator.GetBytes(buffer);
                n = BigInteger.Abs(new BigInteger(buffer));
                if (n.IsEven) n--;
            }
            while (n < min || n > max);

            while (true)
            {
                if (IsProbablePrime(n, steps: 3000))
                {
                    return n;
                }

                n -= 2;
            }
        }

        // Miller–Rabin primality test
        // https://en.wikipedia.org/wiki/Miller%E2%80%93Rabin_primality_test
        private bool IsProbablePrime(BigInteger n, int steps)
        {
            if (n.Sign < 0)
                throw new ArgumentOutOfRangeException(nameof(n));

            if (steps < 1)
                throw new ArgumentOutOfRangeException(nameof(steps));

            if (n == 2 || n == 3)
                return true;
            if (n < 2 || n.IsEven)
                return false;

            BigInteger nMinusOne = n - 1;
            BigInteger d = nMinusOne;
            int s = 0;
            while (d.IsEven)
            {
                d >>= 1;
                s += 1;
            }

            var buffer = new byte[d.ToByteArray().Length];

            for (int i = 0; i < steps; i++)
            {
                BigInteger a;

                while (true)
                {
                    _randomNumGenerator.GetBytes(buffer);
                    a = BigInteger.Abs(new BigInteger(buffer));
                    if (a > 1 && a < nMinusOne)
                        break;
                }

                BigInteger x = BigInteger.ModPow(a, d, n);
                if (x == 1 || x == nMinusOne)
                    continue;

                for (int r = 1; r < s; r++)
                {
                    x = BigInteger.ModPow(x, 2, n);
                    if (x == 1)
                        return false;
                    if (x == nMinusOne)
                        break;
                }

                if (x != nMinusOne)
                    return false;
            }
            return true;
        }
    }
}
