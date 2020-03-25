using System.Collections.Generic;

namespace BFPoc.Storage.SourceData
{
    public interface ISourceDataReader
    {
        IEnumerable<string> ReadData();
    }
}