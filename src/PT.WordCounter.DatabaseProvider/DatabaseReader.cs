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
        private readonly CancellationToken _token;

        public DatabaseReader(DatabaseContext context, DatabaseProviderOptions options, CancellationToken token)
        {
            _context= context;
            _options = options;
            _token = token;
        }

        public IEnumerable<ReadPackage> Read()
        {
            var commandText = $"select \"{_options.Column}\" as Line from \"{_options.Table}\"";

            var lines = _context.Texts
                .FromSqlRaw(commandText)
                .AsEnumerable()
                .SelectMany(x => x.Line.Split(Constants.LineSeparators, StringSplitOptions.RemoveEmptyEntries))
                .Select(Constants.EncodingWin1251.GetBytes);

            foreach (var line in lines)
            {
                if (_token.IsCancellationRequested)
                {
                    break;
                }

                yield return new ReadPackage(line);
            }
        }
    }
}
