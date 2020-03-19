using CommandLine;

namespace PT.WordCounter.Support
{
    [Verb("db")]
    public class DatabaseCommand : ICommand
    {
        [Option("connection-string", Required = true)]
        public string ConnectionString { get; set; }

        [Option("table", Required = true)]
        public string Table { get; set; }

        [Option("column", Required = true)]
        public string Column { get; set; }
    }
}
