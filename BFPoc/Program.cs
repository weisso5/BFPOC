using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
        
        static void Main(string[] args)
        {
            ConfigureLogger();
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(Execute)
                .WithNotParsed(Error);
        }

        private static void Error(IEnumerable<Error> obj)
        {
            foreach (var error in obj)
            {
                Console.Error.WriteLine(error.ToString());
            }
        }


        private static void Execute(Options options)
        {
            RootLogger.Info("Starting Execution with preloading data");
            //TODO - this isn't the best for console apps as it runs every single time
            var textFile = Path.Combine(AppContext.BaseDirectory, "src","SourceData","words_alpha.txt");
            var textReader = new TextFileSourceDataReader(textFile, RootLogger);
            var preloadedData = textReader.ReadData().ToArray();
            RootLogger.Info($"Preload Completed with {preloadedData.Length} item");
            
            RootLogger.Info("Checking against Filter");
            var filter = new SimpleBloomFilter(preloadedData.Length,
                new List<IHasher>() {new DJB2Hasher(), new SDBMHasher()}, RootLogger);
            
            var spellChecker = new PreloadedSpellChecker(filter, preloadedData);
            var result = spellChecker.CheckSpelling(options.Word);
            RootLogger.Info($"Spell Checking Result: {result}");
        }
        
        private static void ConfigureLogger()
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.ConfigureAndWatch(logRepository,new FileInfo("log4net.config") );
            RootLogger = LogManager.GetLogger(typeof(Program));
        }
    }
}