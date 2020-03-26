using System.IO;
using System.Threading;
using System.Collections.Generic;
using PT.WordCounter.Contracts;

namespace PT.WordCounter.FileProvider
{
    public class TextFileReader : IReader
    {
        private readonly TextFileProviderOptions _options;

        public TextFileReader(TextFileProviderOptions options)
        {
            _options = options;
        }

        public IEnumerable<ReadPackage> Read(CancellationToken token)
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
                    while (stream.Position != stream.Length && token.IsCancellationRequested == false)
                    {
                        var line = reader.ReadLine();
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
