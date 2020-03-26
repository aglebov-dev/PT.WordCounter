using System;
using System.IO;
using System.Linq;
using System.Threading;
using PT.WordCounter.Contracts;

namespace PT.WordCounter.FileProvider
{
    public class TextFileWriter : IWriter
    {
        private const byte COMMA = 0x2C;
        private const byte NEW_LINE = 0x0A;
        private readonly TextFileProviderOptions _options;

        public TextFileWriter(TextFileProviderOptions options)
        {
            _options = options;
        }

        public void Write(TreeNode tree, CancellationToken token)
        {
            using (var stream = new FileStream(_options.ReportFilePath, FileMode.Create, FileAccess.Write, FileShare.None, _options.BufferSize, true))
            {
                Flush(stream, tree, token);
            }
        }

        public void Flush(Stream stream, TreeNode tree, CancellationToken token)
        {
            stream = stream ?? throw new ArgumentNullException(nameof(stream));

            var words = tree.GetWords().OrderByDescending(x => x).ToArray();
            for (int i = 0; i < words.Length - 1; i++)
            {
                if (token.IsCancellationRequested)
                {
                    break;
                }
                WriteToStream(stream, words[i]);
                stream.WriteByte(NEW_LINE);
            }

            if (words.Length > 0)
            {
                WriteToStream(stream, words[words.Length - 1]);
            }
        }

        private void WriteToStream(Stream stream, TreeNode word)
        {
            var bytes = Constants.EncodingWin1251.GetBytes(word.AsString());
            var count = Constants.EncodingWin1251.GetBytes(word.Count.ToString());

            stream.Write(bytes, 0, word.Length);
            stream.WriteByte(COMMA);
            stream.Write(count, 0, count.Length);
        }
    }
}
