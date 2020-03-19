using System.Threading;
using PT.WordCounter.Contracts;

namespace PT.WordCounter.ConsoleProvider
{
    public class StdOutProvider : IDataProvider
    {
        private readonly string _text;

        public StdOutProvider(string text)
        {
            _text = text;
        }

        public IReader CreateReader(CancellationToken token)
        {
            return new ConsoleReader(_text, token);
        }

        public IWriter CreateWriter(CancellationToken token)
        {
            return new ConsoleWriter(token);
        }
    }
}
