using System.Threading;

namespace PT.WordCounter.Contracts
{
    public interface IDataProvider
    {
        IReader CreateReader(CancellationToken token);
        IWriter CreateWriter(CancellationToken token);
    }
}
