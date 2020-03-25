using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using BFPoc.Storage;
using BFPoc.Storage.Hashing;
using BFPoc.Util;
using NUnit.Framework;

namespace BFPoc.Tests
{
    
    public class SimpleBloomFilterPerformanceTests
    {
        private ManagementObjectSearcher _wmiObject;
        
        [SetUp]
        public void Setup()
        {
            _wmiObject = new ManagementObjectSearcher("select * from Win32_OperatingSystem");
        }

        /// <summary>
        /// Simple Test to measure memory usage
        /// TODO - The test should compare previous results to see if there is significant deviation
        /// </summary>
        [Test]
        public void SingleHasherDJB2PerformanceAnalyzer()
        {
            var initialMemory = GetMemoryUsedPercentage();
            var data = GetTestData();
            var filter = CreateFilter(data.Length, new IHasher[] {new DJB2Hasher()});
            foreach (var s in data) filter.Insert(s);
            var loadedMemory = GetMemoryUsedPercentage();

            var queryMem = new List<double>();
            using var timer = new CodeTimer();
            
            
            foreach (var test in data)
            {
                var query = filter.Query(test);
                queryMem.Add(GetMemoryUsedPercentage());
                Assert.AreEqual(true,query);    
            }
            Console.WriteLine($"Total Query Time: {timer}");

            var endMemory = GetMemoryUsedPercentage();

            var memUsed = new[] {initialMemory, loadedMemory}.Union(queryMem);
            Console.WriteLine($"Avg Memory Usage: {memUsed.Average()}, Starting Value: {initialMemory}, After Insert: {loadedMemory}, After Query: {endMemory}, Query Average: {queryMem.Average()}");
        }
        
        private IBloomFilter CreateFilter(int size, ICollection<IHasher> hashers)
        {
            return new SimpleBloomFilter(size, hashers, null);
        }

        /// <summary>
        /// 4000 Sample Words
        /// </summary>
        /// <returns></returns>
        private string[] GetTestData()
        {
            var path = Path.Combine(AppContext.BaseDirectory, "performance_test_data.txt");
            return File.ReadAllLines(path, Encoding.UTF8);
        }

        /// <summary>
        /// Simple Memory Percentage using FreePhysicalMemory and TotalVisibleMemorySize
        /// TODO - this should be more complex (smarter) to know about heap and underlying storage mechanism of the filter impl
        /// </summary>
        /// <returns>double</returns>
        private double GetMemoryUsedPercentage()
        {
            var memoryValues = _wmiObject.Get().Cast<ManagementObject>().Select(mo => new {
                FreePhysicalMemory = Double.Parse(mo["FreePhysicalMemory"].ToString()),
                TotalVisibleMemorySize = Double.Parse(mo["TotalVisibleMemorySize"].ToString())
            }).FirstOrDefault();

            return ((memoryValues.TotalVisibleMemorySize - memoryValues.FreePhysicalMemory) / memoryValues.TotalVisibleMemorySize) * 100;
        }
    }
}