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
        private readonly IEnumerable<IDataProvider> _textProviders;
        private readonly SemaphoreSlim _semaphore;

        public Processor(IEnumerable<IDataProvider> textProviders)
        {
            _textProviders = textProviders;
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
            var query =
                from provider in _textProviders
                let reader = provider.CreateReader(token)
                select CreateTask(reader, report, token);

            await Task.WhenAll(query);

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
                foreach (var writer in _textProviders.Select(x => x.CreateWriter(token)))
                {
                    writer.Write(tree);
                }
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
