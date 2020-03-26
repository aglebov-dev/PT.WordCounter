using System;
using System.Linq;
using System.Threading;
using PT.WordCounter.Contracts;

namespace PT.WordCounter.ConsoleProvider
{
    public class ConsoleWriter : IWriter
    {
        public void Write(TreeNode tree, CancellationToken token)
        {
            var words = tree.GetWords().OrderByDescending(x => x).ToArray();
            for (int i = 0; i < words.Length; i++)
            {
                if (token.IsCancellationRequested)
                {
                    break;
                }

                var word = words[i];
                Console.WriteLine("{0},{1}", word.AsString(), word.Count);
            }
        }
    }
}
