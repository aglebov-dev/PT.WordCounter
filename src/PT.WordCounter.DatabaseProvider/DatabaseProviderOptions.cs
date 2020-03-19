namespace PT.WordCounter.DatabaseProvider
{
    public class DatabaseProviderOptions
    {
        public string ConnectionStrings { get; }
        public string Table { get; }
        public string Column { get; }

        public DatabaseProviderOptions(string connectionStrings, string table, string column)
        {
            ConnectionStrings = connectionStrings;
            Table = table;
            Column = column;
        }
    }
}
