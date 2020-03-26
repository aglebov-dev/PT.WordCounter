using System;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PT.WordCounter.Contracts;
using PT.WordCounter.DatabaseProvider.DataAccess;

namespace PT.WordCounter.DatabaseProvider
{
    public class DatabaseReader : IReader
    {
        private readonly DatabaseContext _context;
        private readonly DatabaseProviderOptions _options;

        public DatabaseReader(DatabaseContext context, DatabaseProviderOptions options)
        {
            _context= context;
            _options = options;
        }

        public IEnumerable<ReadPackage> Read(CancellationToken token)
        {
            var commandText = $"select \"{_options.Column}\" as Line from \"{_options.Table}\"";

            var lines = _context.Texts
                .FromSqlRaw(commandText)
                .AsEnumerable()
                .SelectMany(x => x.Line.Split(Constants.LineSeparators, StringSplitOptions.RemoveEmptyEntries));

            foreach (var line in lines)
            {
                if (token.IsCancellationRequested)
                {
                    break;
                }

                yield return new ReadPackage(line);
            }
        }
    }
}
