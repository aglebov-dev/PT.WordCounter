using System.IO;
using System.Threading;
using System.Collections.Generic;
using PT.WordCounter.Contracts;

namespace PT.WordCounter.FileProvider
{
    internal class TextFileReader : IReader
    {
        private readonly TextFileProviderOptions _options;
        private readonly CancellationToken _token;

        public TextFileReader(TextFileProviderOptions options, CancellationToken cancellationToken)
        {
            _options = options;
            _token = cancellationToken;
        }

        public IEnumerable<ReadPackage> Read()
        {
            var stream = default(FileStream);
            try
            {
                stream = new FileStream
                (
                    path: _options.SourceFilePath,
                    mode: FileMode.Open,
                    access: FileAccess.Read,
                    share: FileShare.Read,
                    bufferSize: _options.BufferSize,
                    useAsync: true
                );

                using (var reader = new StreamReader(stream, _options.Encoding))
                {
                    while (stream.Position != stream.Length && _token.IsCancellationRequested == false)
                    {
                        var text = reader.ReadLine();
                        var line = Constants.EncodingWin1251.GetBytes(text);
                        yield return new ReadPackage(line);
                    }
                }
            }
            finally
            {
                stream?.Dispose();
            }
        }
    }
}
