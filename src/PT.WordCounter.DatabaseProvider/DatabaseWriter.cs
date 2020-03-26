using System;
using System.Linq;
using System.Threading;
using PT.WordCounter.Contracts;
using PT.WordCounter.DatabaseProvider.DataAccess;

namespace PT.WordCounter.DatabaseProvider
{
    public class DatabaseWriter : IWriter
    {
        private readonly DatabaseProviderOptions _options;
        private readonly DatabaseContext _context;

        public DatabaseWriter(DatabaseContext context, DatabaseProviderOptions options)
        {
            _options = options;
            _context = context;
        }

        public void Write(TreeNode tree, CancellationToken token)
        {
            //TODO write to database
            var words = tree.GetWords().OrderByDescending(x => x).ToArray();
            for (int i = 0; i < words.Length; i++)
            {
                if (token.IsCancellationRequested)
                {
                    break;
                }

                var word = words[i];
                Console.WriteLine("{0},{1}", word.AsString(), word.Count);
            }
        }
    }
}
