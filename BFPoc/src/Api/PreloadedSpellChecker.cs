using System.Collections.Generic;
using System.Linq;
using BFPoc.Contracts;
using BFPoc.Extensibility;
using BFPoc.Models;
using BFPoc.Storage;
using log4net;

namespace BFPoc.Api
{
    public class PreloadedSpellChecker : ISpellChecker
    {
        private readonly IBloomFilter _filter;
        private readonly ILog _rootLogger;
        private readonly AlternativeWordGenerator _generator = new AlternativeWordGenerator();

        public PreloadedSpellChecker(IBloomFilter filter, string[] preloadedData, ILog rootLogger)
        {
            _filter = filter;
            _rootLogger = rootLogger;
            InitFilter(preloadedData);
        }

        private void InitFilter(string[] preloadedData)
        {
            _rootLogger.Debug($"Preloading Checker with {preloadedData.Length} items");
            foreach (var str in preloadedData)
            {
                _filter.Insert(str);
            }
        }

        public SpellCheckResult CheckSpelling(string toCheck)
        {
            _rootLogger.Debug($"Checking Spelling for {toCheck}");
            if (_filter.Query(toCheck))
            {
                return new SpellCheckResult(ResultStatus.Found,new[]{toCheck});
            }

            var grams = _generator.Combination2(toCheck);
            var suggestions = grams.Where(g => _filter.Query(g)).Select(g => g).ToArray();
            if(suggestions.Any())
                return new SpellCheckResult(ResultStatus.FoundCloseMatch, suggestions);
            return new SpellCheckResult(ResultStatus.NotFound,new List<string>(0));
        }
    }
}