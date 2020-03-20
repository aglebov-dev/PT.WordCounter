using System;
using System.IO;
using System.Linq;
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

        public void Write(TreeNode tree)
        {
            using (var stream = new FileStream(_options.ReportFilePath, FileMode.Create, FileAccess.Write, FileShare.None, _options.BufferSize, true))
            {
                Flush(stream, tree);
            }
        }

        public void Flush(Stream stream, TreeNode tree)
        {
            stream = stream ?? throw new ArgumentNullException(nameof(stream));

            var words = tree.GetWords().OrderByDescending(x => x).ToArray();
            for (int i = 0; i < words.Length - 1; i++)
            {
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
            var bytes = word.AsBytes();
            var count = Constants.EncodingWin1251.GetBytes(word.Count.ToString());

            stream.Write(bytes, 0, word.Length);
            stream.WriteByte(COMMA);
            stream.Write(count, 0, count.Length);
        }
    }
}
