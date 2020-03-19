using System.Threading;
using Microsoft.EntityFrameworkCore;
using PT.WordCounter.Contracts;
using PT.WordCounter.DatabaseProvider.DataAccess;

namespace PT.WordCounter.DatabaseProvider
{
    public class DatabaseProvider : IDataProvider
    {
        private readonly DatabaseProviderOptions _options;
        private readonly DatabaseContext _context;

        public DatabaseProvider(DatabaseProviderOptions options)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseNpgsql(options.ConnectionStrings);
            _context = new DatabaseContext(optionsBuilder.Options);
            _options = options;
        }

        public IReader CreateReader(CancellationToken token)
        {
            return new DatabaseReader(_context, _options, token);
        }

        public IWriter CreateWriter(CancellationToken token)
        {
            return new DatabaseWriter(_context, _options, token);
        }
    }
}
