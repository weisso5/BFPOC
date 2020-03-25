using System;
using System.Collections.Generic;
using System.Reflection;
using BFPoc.Storage;
using BFPoc.Storage.Hashing;
using log4net;
using NUnit.Framework;

namespace BFPoc.Tests
{
 
    /// <summary>
    /// TODO - These tests should be derived from a calculated FP acceptance rate
    /// </summary>
    public class SimpleBloomFilterAccuracyTests
    {
        
        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public void PrimeOddHasherAccuracyTest()
        {
            var filter = new SimpleBloomFilter(GetSample().Count,new IHasher[] { new PrimeOddHasher(11,9)  }, null);
            
            foreach (var s in GetSample()) filter.Insert(s);

            var total = GetSample().Count;
            var correct = 0;
            var incorrect = 0;
            
            foreach (var sample in GetSample())
            {
                var result = filter.Query(sample);
                if (result)
                {
                    correct++;
                }
                else
                {
                    incorrect++;
                }
            }
            
            Console.WriteLine($"Total: {total}, Correct: {correct}, Incorrect: {incorrect}");
            Assert.AreEqual(total, correct);
        }
        
        [Test]
        public void DJB2HasherAccuracyTest()
        {
            var filter = new SimpleBloomFilter(GetSample().Count,new IHasher[] { new DJB2Hasher()  }, null);
            
            foreach (var s in GetSample()) filter.Insert(s);

            var total = GetSample().Count;
            var correct = 0;
            var incorrect = 0;
            
            foreach (var sample in GetSample())
            {
                var result = filter.Query(sample);
                if (result)
                {
                    correct++;
                }
                else
                {
                    incorrect++;
                }
            }
            
            Console.WriteLine($"Total: {total}, Correct: {correct}, Incorrect: {incorrect}");
            Assert.AreEqual(total, correct);
        }
        
        [Test]
        public void SDBMHasherAccuracyTest()
        {
            var filter = new SimpleBloomFilter(GetSample().Count,new IHasher[] { new SDBMHasher()  }, null);
            
            foreach (var s in GetSample()) filter.Insert(s);

            var total = GetSample().Count;
            var correct = 0;
            var incorrect = 0;
            
            foreach (var sample in GetSample())
            {
                var result = filter.Query(sample);
                if (result)
                {
                    correct++;
                }
                else
                {
                    incorrect++;
                }
            }
            
            Console.WriteLine($"Total: {total}, Correct: {correct}, Incorrect: {incorrect}");
            Assert.AreEqual(total, correct);
        }

        private ICollection<string> GetSample()
        {
            return new[]
            {
                "the", "and", "have", "that", "for", "you", "with", "say", "this", "they", "but", "his", "from", "not",
                "she", "as", "what", "their", "can", "who", "get", "would", "her", "all", "make", "about", "know",
                "will", "one", "time", "there", "year", "think", "when", "which", "them", "some", "people", "take",
                "out", "into", "just", "see", "him", "your", "come", "could", "now", "than", "like", "other", "how",
                "then", "its", "our", "two", "more", "these", "want", "way", "look", "first", "also", "new", "because",
                "day", "use", "man", "find", "here", "thing", "give", "many", "well", "only", "those", "tell", "very",
                "even", "back", "any", "good", "woman", "through", "life", "child", "work", "down", "may", "after",
                "should", "call", "world", "over", "school", "still", "try", "last", "ask", "need", "too", "feel",
                "three", "state", "never", "become", "between", "high", "really", "something", "most", "another",
                "much", "family", "own", "leave", "put", "old", "while", "mean", "keep", "student", "why", "let",
                "great", "same", "big", "group", "begin", "seem", "country", "help", "talk", "where", "turn", "problem",
                "every", "start", "hand", "might", "American", "show", "part", "against", "place", "such", "again",
                "few", "case", "week", "company", "system", "each", "right", "program", "hear", "question", "during",
                "play", "government", "run", "small"
            };
        }
        
        private SimpleBloomFilter CreateFilter(int size, ICollection<IHasher> hashers)
        {
            var logger = LogManager.GetLogger(typeof(SimpleBloomFilterAccuracyTests));
            return new SimpleBloomFilter(size, hashers, logger);
        }
    }
}