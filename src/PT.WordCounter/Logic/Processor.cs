using System.Threading;
using PT.WordCounter.Contracts;

namespace PT.WordCounter.Logic
{
    internal class Processor
    {
        private readonly IReader _reader;
        private readonly IWriter _writer;
        private readonly SemaphoreSlim _semaphore;

        public Processor(IReader reader, IWriter writer)
        {
            _reader = reader;
            _writer = writer;
            _semaphore = new SemaphoreSlim(Constants.THREADS_COUNT);
        }

        public void StartAsync(CancellationToken token)
        {
            var report = ReadInternal(_reader, token);
            WriteAsync(report, token);
        }

        private void WriteAsync(Report report, CancellationToken token)
        {
            var tree = report.GetTree();
            _writer.Write(tree, token);
        }


        private Report ReadInternal(IReader reader, CancellationToken token)
        {
            var report = new Report();
            foreach (var item in reader.Read(token))
            {
                var words = LineSplit.Split(item.TextLine);
                report.AddRange(words);
            }

            return report;
        }
    }
}
