using System;
using System.Numerics;

namespace RSALib
{
    public class RSA
    {
        private readonly RSAKey _key;

        public RSA(RSAKey key)
        {
            _key = key;
        }

        public  byte[] Encrypt(byte[] data)
        {
            var c = BigInteger.ModPow(new BigInteger(data), _key.E, _key.N);
            return c.ToByteArray();
        }

        public byte[] Decrypt(byte[] data)
        {
            var c = BigInteger.ModPow(new BigInteger(data), _key.D, _key.N);
            return c.ToByteArray();
        }
    }
}
