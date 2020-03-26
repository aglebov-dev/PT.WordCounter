using System.Collections.Generic;
using System.Threading;

namespace PT.WordCounter.Contracts
{
    public interface IReader
    {
        IEnumerable<ReadPackage> Read(CancellationToken token);
    }
}
