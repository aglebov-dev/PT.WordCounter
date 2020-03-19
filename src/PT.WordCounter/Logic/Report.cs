using System;
using System.Linq;
using System.Collections.Generic;
using PT.WordCounter.Contracts;

namespace PT.WordCounter.Logic
{
    internal class Report
    {
        private readonly TreeNode _root;

        public Report()
        {
            _root = TreeNode.CreateRoot();
        }

        public void Add(byte[] data)
        {
            if (data?.Length > 0)
            {
                _root.Add(data.Length, data);
            }
        }

        internal void AddRange(IEnumerable<byte[]> words)
        {
            words = words ?? throw new ArgumentNullException(nameof(words));
            words.AsParallel().ForAll(Add);
        }

        public TreeNode GetTree() => _root;
    }
}
