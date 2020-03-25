using System;
using BFPoc.Storage.Hashing;
using NUnit.Framework;

namespace BFPoc.Tests
{
    
    public class HasherTests
    {
        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public void DJB2HasherIsCorrect()
        {
            var hasher = new DJB2Hasher();
            var hash = hasher.Hash("hello");
            Assert.AreEqual(261238937,hash);
        }

        [Test]
        public void SDBMHasherIsCorrect()
        {
            var hasher = new SDBMHasher();
            var hash = hasher.Hash("hello");
            Assert.AreEqual(684824882, hash);
        }
    }
}