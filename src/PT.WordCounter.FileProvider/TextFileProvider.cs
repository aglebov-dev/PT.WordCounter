using System.Threading;
using PT.WordCounter.Contracts;

namespace PT.WordCounter.FileProvider
{
    public class TextFileProvider : IDataProvider
    {
        private readonly TextFileProviderOptions _options;

        public TextFileProvider(TextFileProviderOptions options)
        {
            _options = options;
        }

        public IReader CreateReader(CancellationToken token)
        {
            return new TextFileReader(_options, token);
        }

        public IWriter CreateWriter(CancellationToken token)
        {
            return new TextFileWriter(_options);
        }
    }
}
