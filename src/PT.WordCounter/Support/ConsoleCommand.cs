using CommandLine;

namespace PT.WordCounter.Support
{
    [Verb("console")]
    public class ConsoleCommand : ICommand
    {
        [Option("text", Required = true)]
        public string Text { get; set; }
    }
}
