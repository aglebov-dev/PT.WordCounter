using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
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

        public async Task StartAsync(CancellationToken token)
        {
            var report = await Read(token);
            WriteAsync(report, token);
        }

        private async Task<Report> Read(CancellationToken token)
        {
            var report = new Report();
            await CreateTask(_reader, report, token);

            return report;
        }

        private async Task CreateTask(IReader reader, Report report, CancellationToken token)
        {
            using (await LockAsync(token))
            {
                if (!token.IsCancellationRequested)
                {
                    ReadInternal(reader, report);
                }
            }
        }

        private void WriteAsync(Report report, CancellationToken token)
        {
            if (!token.IsCancellationRequested)
            {
                var tree = report.GetTree();
                _writer.Write(tree);
            }
        }

        private async Task<IDisposable> LockAsync(CancellationToken token)
        {
            await _semaphore.WaitAsync(token);
            return new SemaphoreWrapper(_semaphore);
        }

        private void ReadInternal(IReader reader, Report report)
        {
            foreach (var item in reader.Read())
            {
                var words = LineSplit.Split(item.TextLine);
                report.AddRange(words);
            }
        }

        private struct SemaphoreWrapper : IDisposable
        {
            private readonly SemaphoreSlim _semaphore;
            public void Dispose() => _semaphore.Release();
            public SemaphoreWrapper(SemaphoreSlim semaphore)
            {
                _semaphore = semaphore;
            }
        }
    }
}
