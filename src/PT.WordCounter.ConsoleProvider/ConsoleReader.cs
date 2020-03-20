using System;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using PT.WordCounter.Contracts;

namespace PT.WordCounter.ConsoleProvider
{
    public class ConsoleReader: IReader
    {
        private readonly string _text;
        private readonly CancellationToken _token;

        public ConsoleReader(string text, CancellationToken token)
        {
            _text = text;
            _token = token;
        }

        public IEnumerable<ReadPackage> Read()
        {
            var lines = _text
                .Split(Constants.LineSeparators, StringSplitOptions.RemoveEmptyEntries)
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
