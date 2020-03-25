using System;
using System.Collections.Generic;
using BFPoc.Storage;
using BFPoc.Storage.Hashing;
using log4net;
using NUnit.Framework;

namespace BFPoc.Tests
{
    public class SimpleBloomFilterTests
    {
        
        
        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public void SingleHasherShouldReturnTrueForSameInsertedWordTest()
        {
            var filter = CreateFilter(2, new[] {new DJB2Hasher()});
            var word1 = "dog";
            var word2 = "cat";

            filter.Insert(word1, word2);
            
            Assert.AreEqual(filter.Query(word1),true);
            Assert.AreEqual(filter.Query(word2), true);
        }

        [Test]
        public void SingleHasherShouldReturnTrueFalseForSingleInsertTest()
        {
            var filter = CreateFilter(24, new[] {new DJB2Hasher()});
            var word1 = "dog";
            var word2 = "cat";
            
            filter.Insert(word1);
            Assert.AreEqual(filter.Query(word1),true);
            Assert.AreEqual(filter.Query(word2), false);
        }


        [Test]
        public void DoubleHasherShouldReturnForSameInsertWordTest()
        {
            var filter = CreateFilter(24, new IHasher[] {new DJB2Hasher(), new SDBMHasher()});
            var word1 = "dog";
            var word2 = "cat";

            filter.Insert(word1, word2);
            
            Assert.AreEqual(filter.Query(word1),true);
            Assert.AreEqual(filter.Query(word2), true);
        }
        
        [Test]
        public void DoubleHasherShouldReturnTrueFalseForSingleInsertTest()
        {
            var filter = CreateFilter(24, new IHasher[] {new DJB2Hasher(), new SDBMHasher()});
            var word1 = "dog";
            var word2 = "cat";

            filter.Insert(word1);
            
            Assert.AreEqual(filter.Query(word1),true);
            Assert.AreEqual(filter.Query(word2), false);
        }

        [Test]
        public void EmptyHashFunctionsShouldErrorTest()
        {
            var filter = CreateFilter(2, new List<IHasher>(0));

            Assert.Throws<ArgumentNullException>(() => filter.Insert("word1"));
        }

        [Test]
        public void DoubleHasherFalsePositiveShouldReturnTrueTest()
        {    
            var filter = CreateFilter(5, new IHasher[] {new PrimeOddHasher(11,9), new PrimeOddHasher(17,15)  });
            
            filter.Insert("hello", "elloh", "llohe", "lohel", "ohelo");
            
            Assert.AreEqual(filter.Query("This wasn't inserted!"),true);
        }
        

        private SimpleBloomFilter CreateFilter(int size, ICollection<IHasher> hashers)
        {
            var logger = LogManager.GetLogger(typeof(SimpleBloomFilterTests));
            return new SimpleBloomFilter(size, hashers, logger);
        }
        
    }
}