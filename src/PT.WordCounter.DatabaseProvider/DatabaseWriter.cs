using System;
using System.Linq;
using System.Text;
using System.Threading;
using PT.WordCounter.Contracts;
using PT.WordCounter.DatabaseProvider.DataAccess;

namespace PT.WordCounter.DatabaseProvider
{
    internal class DatabaseWriter : IWriter
    {
        private readonly DatabaseProviderOptions _options;
        private readonly CancellationToken _token;
        private readonly DatabaseContext _context;
        private readonly Encoding _encoding;

        public DatabaseWriter(DataAccess.DatabaseContext context, DatabaseProviderOptions options, CancellationToken token)
        {
            _options = options;
            _token = token;
            _context = context;
        }

        public void Write(TreeNode tree)
        {
            //TODO write to database
            var words = tree.GetWords().OrderByDescending(x => x).ToArray();
            for (int i = 0; i < words.Length; i++)
            {
                if (_token.IsCancellationRequested)
                {
                    break;
                }

                var word = words[i];
                Console.WriteLine("{0},{1}", word.AsString(), word.Count);
            }
        }
    }
}
