using System.Numerics;

namespace RSALib
{
    public struct RSAKey
    {
        public BigInteger E;
        public BigInteger D;
        public BigInteger P;
        public BigInteger Q;
        public BigInteger N;
    }
}
