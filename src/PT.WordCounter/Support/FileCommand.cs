using CommandLine;

namespace PT.WordCounter.Support
{
    [Verb("file")]
    public class FileCommand: ICommand
    {
        [Option("source", Required = true)]
        public string SourceFile { get; set; }

        [Option("target", Required = true)]
        public string TargetFile { get; set; }

        [Option("utf8", Required = false)]
        public bool UTF8 { get; set; }

        [Option("win1251", Required = false)]
        public bool Win1251 { get; set; }
    }
}
