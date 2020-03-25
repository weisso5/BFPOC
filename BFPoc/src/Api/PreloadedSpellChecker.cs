using BFPoc.Contracts;
using BFPoc.Storage;

namespace BFPoc.Api
{
    public class PreloadedSpellChecker : ISpellChecker
    {
        private readonly IBloomFilter _filter;

        public PreloadedSpellChecker(IBloomFilter filter, string[] preloadedData)
        {
            _filter = filter;
            InitFilter(preloadedData);
        }

        private void InitFilter(string[] preloadedData)
        {
            foreach (var str in preloadedData)
            {
                _filter.Insert(str);
            }
        }

        public bool CheckSpelling(string toCheck)
        {
            return _filter.Query(toCheck);
        }
    }
}