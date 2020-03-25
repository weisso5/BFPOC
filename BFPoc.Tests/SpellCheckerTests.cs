using System;
using System.Collections.Generic;
using System.Text;
using BFPoc.Api;
using BFPoc.Contracts;
using BFPoc.Models;
using BFPoc.Storage;
using BFPoc.Storage.Hashing;
using log4net;
using NUnit.Framework;

namespace BFPoc.Tests
{
    public class SpellCheckerTests
    {

        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void SpellCheckerReturnsMatchForSingleWord()
        {
            var checker = CreateSpellChecker(new[] {"Hello"});
            var result = checker.CheckSpelling("Hello");
            Assert.AreEqual(ResultStatus.Found, result.Status);
        }

        [Test]
        public void SpellCheckerReturnsCloseMatchForDerivedWord()
        {
            var checker = CreateSpellChecker(new[] {"Cat"});
            var result = checker.CheckSpelling("Cats");
            Assert.AreEqual(ResultStatus.FoundCloseMatch, result.Status);
        }

        [Test]
        public void SpellCheckerReturnsNoMatchForRandomWord()
        {
            var checker = CreateSpellChecker(new[] { "Hello" });
            var result = checker.CheckSpelling("Kerblam");
            Assert.AreEqual(ResultStatus.NotFound, result.Status);
        }


        private ISpellChecker CreateSpellChecker(string[] testData)
        {
            var logger = LogManager.GetLogger(typeof(SpellCheckerTests));
            var simpleBF = new SimpleBloomFilter(testData.Length + 23, new IHasher[] { new DJB2Hasher(), new SDBMHasher() }, logger);
            return new PreloadedSpellChecker(simpleBF,testData, logger);
        }
    }
}
