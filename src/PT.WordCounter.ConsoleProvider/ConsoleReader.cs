using System;
using System.Threading;
using System.Collections.Generic;
using PT.WordCounter.Contracts;

namespace PT.WordCounter.ConsoleProvider
{
    public class ConsoleReader: IReader
    {
        private readonly string _text;

        public ConsoleReader(string text)
        {
            _text = text;
        }

        public IEnumerable<ReadPackage> Read(CancellationToken token)
        {
            var lines = _text
                .Split(Constants.LineSeparators, StringSplitOptions.RemoveEmptyEntries);
            
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
