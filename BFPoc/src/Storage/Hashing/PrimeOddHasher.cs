using System;

namespace BFPoc.Storage.Hashing
{
    //TODO not stable
    public class PrimeOddHasher : IHasher
    {
        private const int _bitset = 6400;
        private readonly long _prime;
        private readonly long _odd;

        public PrimeOddHasher(long prime, long odd)
        {
            _prime = prime;
            _odd = odd;
        }
        
        public int Hash(string value)
        {
            var hc = value.GetHashCode();
            if (hc < 0)
                hc = Math.Abs(hc);
            
            return (int)((((hc % _bitset) * _prime) % _bitset) * _odd) % _bitset;
        }
    }
}