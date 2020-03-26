using System.Threading;

namespace PT.WordCounter.Contracts
{
    public interface IWriter
    {
        void Write(TreeNode tree, CancellationToken token);
    }
}
