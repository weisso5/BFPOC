using System.Runtime.InteropServices;

namespace BFPoc.Storage.Hashing
{
    public class SDBMHasher : IHasher
    {
        public int Hash(string value)
        {
            uint hash = 0;

            foreach (var t in value)
            {
                hash = ((byte)t) + (hash << 6) + (hash << 16) - hash;
            }

            return (int) hash;
        }
    }
}