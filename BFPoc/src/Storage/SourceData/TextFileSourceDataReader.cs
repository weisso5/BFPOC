using System.Collections.Generic;
using System.IO;
using log4net;

namespace BFPoc.Storage.SourceData
{
    public class TextFileSourceDataReader : ISourceDataReader
    {
        private readonly string _filePath;

        public TextFileSourceDataReader(string filePath, ILog rootLogger)
        {
            _filePath = filePath;
        }
        
        public IEnumerable<string> ReadData()
        {
            foreach (var line in File.ReadLines(_filePath))
            {
                yield return line;
            }
        }
    }
}