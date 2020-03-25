using CommandLine;

namespace BFPoc.Api
{
    
    public class Options
    {
        [Option("word", Required = true, HelpText = "Enter English Word to Spell Check")]
        public string Word { get; set; }
    }
}