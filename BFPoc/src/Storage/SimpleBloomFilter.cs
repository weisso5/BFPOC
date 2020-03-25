using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BFPoc.Storage.Hashing;
using BFPoc.Util;
using log4net;

namespace BFPoc.Storage
{
    /// <summary>
    /// Simple Bloom filter implemented with an <see cref="BitArray"/>
    /// </summary>
    public class SimpleBloomFilter : IBloomFilter
    {
        
        private readonly int _size;
        private readonly ICollection<IHasher> _hashFunctions;
        private readonly ILog _rootLogger;
        
        /// <summary>
        /// TODO - this is fine for POC but should be spread across multiple storage to maximize heap usage
        /// </summary>
        private readonly BitArray _bitArray;

        public SimpleBloomFilter(int size, ICollection<IHasher> hashFunctions, ILog rootLogger)
        {
            _size = size;
            _hashFunctions = hashFunctions;
            _rootLogger = rootLogger;
            _bitArray = new BitArray(size);
        }
        
        public void Insert(params string[] values)
        {
            if(_hashFunctions.Count == 0)
                throw new ArgumentNullException("Hash Functions is empty!");

            using var timer = new CodeTimer();
            foreach (var value in values)
            foreach (var computeHash in ComputeHashes(value))
            {
                _rootLogger.Debug($"Inserting hashvalue: {computeHash} for {value}");
                _bitArray[computeHash] = true;
            }
            timer.Stop();
            _rootLogger.Debug($"Insert of {values.Length} took {timer}");
        }

        public bool Query(string value)
        {
            _rootLogger.Debug($"Query for {value}");
            using var timer = new CodeTimer();
            var hashes = ComputeHashes(value);
            var results = hashes.Select(hash => SafeAccess(hash));

            timer.Stop();
            _rootLogger.Debug($"Query for {value} took {timer}"); //TODO - Expose Timing Metrics to Result
            return results.Aggregate(true, (b, b1) => b && b1);
        }

        public bool this[int index] => _bitArray[index];

        private bool SafeAccess(int hash)
        {
            try
            {
                _rootLogger.Debug($"Looking for hash {hash}");
                return _bitArray[hash];
            }
            catch (KeyNotFoundException)
            {
                return false;
            }
        }

        public IEnumerable<int> ComputeHashes(string value)
        {
            return _hashFunctions.Select(hf => Math.Abs(hf.Hash(value)) % _size).ToArray();
        }
    }
}