using System.Collections.Generic;

namespace BFPoc.Storage
{
    /// <summary>
    /// Generic interface for creating Bloom filter implementations
    /// </summary>
    public interface IBloomFilter
    {
        void Insert(params string[] values);
        
        bool this[int index] { get; }
        bool Query(string value);
    }
}