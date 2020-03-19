using System.Collections.Generic;

namespace PT.WordCounter.Contracts
{
    public interface IReader
    {
        IEnumerable<ReadPackage> Read();
    }
}
