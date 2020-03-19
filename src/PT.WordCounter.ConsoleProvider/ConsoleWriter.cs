using System;
using System.Linq;
using System.Threading;
using PT.WordCounter.Contracts;

namespace PT.WordCounter.ConsoleProvider
{
    internal class ConsoleWriter : IWriter
    {
        private readonly CancellationToken _token;

        public ConsoleWriter(CancellationToken token)
        {
            _token = token;
        }

        public void Write(TreeNode tree)
        {
            var words = tree.GetWords().OrderByDescending(x => x).ToArray();
            for (int i = 0; i < words.Length; i++)
            {
                if (_token.IsCancellationRequested)
                {
                    break;
                }

                var word = words[i];
                Console.WriteLine("{0},{1}", word.AsString(), word.Count);
            }
        }
    }
}
