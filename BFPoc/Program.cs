using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using BFPoc.Api;
using BFPoc.Storage;
using BFPoc.Storage.Hashing;
using BFPoc.Storage.SourceData;
using CommandLine;
using log4net;
using log4net.Config;

namespace BFPoc
{
    class Program
    {
        private static ILog RootLogger;

        /// <summary>
        /// simplified regex to check/remove symbols
        /// TODO - should be provided by a logical decision
        /// </summary>
        private static readonly Regex _charsToCheck = new Regex(@"\W|_", RegexOptions.Compiled); 


        static void Main(string[] args)
        {
            ConfigureLogger();
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(Execute)
                .WithNotParsed(Error);
        }

        

        #region Execute

        /// <summary>
        /// Main Entry Point
        /// </summary>
        /// <param name="options"></param>
        private static void Execute(Options options)
        {
            if (SkippedCharsDetected(options.Word))
            {
                RootLogger.Warn("Input contains symbol characters that will be skipped(replaced)");
            }

            var sourceWord = CleanString(options.Word);

            RootLogger.Info("Starting Execution with preloading data");
            var preloadedData = GetPreloadedData();

            RootLogger.Info($"Preload Completed with {preloadedData.Length} item");

            
            RootLogger.Info("Checking against Filter");
            var filter = GetFilter(preloadedData.Length);
            
            var spellChecker = new PreloadedSpellChecker(filter, preloadedData, RootLogger);
            var result = spellChecker.CheckSpelling(sourceWord);

            RootLogger.Info($"Spell Checking Result: {result}");
        }

        private static string[] GetPreloadedData()
        {
            //TODO - this isn't the best for console apps as it runs every single time
            //TODO - Dataset should come from a more dynamic source, sample set for POC
            var textFile = Path.Combine(AppContext.BaseDirectory, "src","Storage", "SourceData", "words_alpha.txt");
            var textReader = new TextFileSourceDataReader(textFile, RootLogger);
            var preloadedData = textReader.ReadData().ToArray();
            return preloadedData;
        }
        
        private static SimpleBloomFilter GetFilter(int length)
        {
            return new SimpleBloomFilter(length,
                new List<IHasher>() {new DJB2Hasher(), new SDBMHasher()}, RootLogger);
        }

        private static bool SkippedCharsDetected(string optionsWord)
        {
            return _charsToCheck.Match(optionsWord).Groups.Count > 0;
        }

        private static string CleanString(string optionsWord)
        {
            return _charsToCheck.Replace(optionsWord, "");
        }

        #endregion

        #region Program Flow

        private static void ConfigureLogger()
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.ConfigureAndWatch(logRepository,new FileInfo("log4net.config") );
            RootLogger = LogManager.GetLogger(typeof(Program));
        }

        private static void Error(IEnumerable<Error> obj)
        {
            foreach (var error in obj)
            {
                Console.Error.WriteLine(error.ToString());
            }
        }

        #endregion
    }
}