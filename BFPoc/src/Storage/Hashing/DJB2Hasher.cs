namespace BFPoc.Storage.Hashing
{
    public class DJB2Hasher : IHasher
    {
        public int Hash(string value)
        {
            uint hash = 5381;

            for (var i = 0; i < value.Length; i++)
            {
                hash = ((hash << 5) + hash) + ((byte)value[i]);
            }

            return (int) hash;
        }
        
    }
}