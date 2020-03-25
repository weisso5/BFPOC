using System.Collections.Generic;

namespace BFPoc.Storage
{
    /// <summary>
    /// Generic interface for creating Bloom filter implementations
    /// </summary>
    public interface IBloomFilter
    {
        /// <summary>
        /// Add to Filter
        /// </summary>
        /// <param name="values"></param>
        void Insert(params string[] values);
        
        /// <summary>
        /// Retrieve Filter Item by Hash
        /// </summary>
        /// <param name="hash">Int</param>
        /// <returns></returns>
        bool this[int hash] { get; }

        /// <summary>
        /// Test for Existence in Filter
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        bool Query(string value);

        /// <summary>
        /// Helper Method for getting hash values to retrieve items from Filter, <seealso cref="this[int index]"/>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        IEnumerable<int> ComputeHashes(string value);
    }
}