using System.Text;

namespace PT.WordCounter.FileProvider
{
    public class TextFileProviderOptions
    {
        public string SourceFilePath { get; }
        public string ReportFilePath { get; }
        public int BufferSize { get; }
        public Encoding Encoding { get; }

        public TextFileProviderOptions(string filePath, string reportFilePath, int bufferSize, Encoding encoding)
        {
            SourceFilePath = filePath;
            ReportFilePath = reportFilePath;
            BufferSize = bufferSize;
            Encoding = encoding;
        }
    }
}
