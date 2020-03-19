﻿using System;
using System.Threading;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace PT.WordCounter.Contracts
{
    [DebuggerDisplay("Key: {_key}; Count: {Count}")]
    public class TreeNode : IComparable<TreeNode>
    {
        private readonly byte _key;
        private readonly TreeNode _parent;
        private readonly ConcurrentDictionary<byte, TreeNode> _children;
        private int _wordsCount;
        public int Count => _wordsCount;
        public int Length { get; }

        public static TreeNode CreateRoot() => new TreeNode(Constants.SPACE, default);

        private TreeNode(byte key, TreeNode parent)
        {
            _key = key;
            _wordsCount = 0;
            _parent = parent;
            _children = new ConcurrentDictionary<byte, TreeNode>(Constants.THREADS_COUNT, 8);

            Length = parent is null ? 0 : parent.Length + 1;
        }

        public void Add(int length, byte[] word)
        {
            if (word != null)
            {
                length = Math.Min(length, word.Length);
                var node = this;
                var index = 0;

                while (index < length)
                {
                    var key = ToUpper(word[index]);
                    node = node._children.GetOrAdd(key, x => new TreeNode(x, node));
                    index++;
                }

                if (length > 0)
                {
                    Interlocked.Increment(ref node._wordsCount);
                }
            }
        }

        public IEnumerable<TreeNode> GetWords()
        {
            var queue = new Queue<TreeNode>(new[] { this });

            while (queue.Count > 0)
            {
                var node = queue.Dequeue();
                if (node._wordsCount > 0)
                {
                    yield return node;
                }
                foreach (var item in node._children.Values)
                {
                    queue.Enqueue(item);
                }
            }
        }

        public string AsString()
        {
            if (Length > 0)
            {
                var result = new byte[Length];
                var node = this;
                while (node.Length > 0)
                {
                    result[node.Length - 1] = node._key;
                    node = node._parent;
                }

                return Constants.EncodingWin1251.GetString(result);
            }
            else
            {
                return string.Empty;
            }
        }

        public byte[] AsBytes()
        {
            if (Length > 0)
            {
                var node = this;
                var result = new byte[node.Length];
                while (node.Length > 0)
                {
                    result[node.Length - 1] = node._key;
                    node = node._parent;
                }

                return result;
            }

            return Array.Empty<byte>();
        }

        public int CompareTo(TreeNode other)
        {
            return _wordsCount.CompareTo(other._wordsCount);
        }

        private byte ToUpper(byte value)
        {
            if ((value > 96 && value < 123) || (value > 223))
            {
                return (byte)(value - 32);
            }
            else if (value == 184)
            {
                return (byte)(value - 16);
            }

            return value;
        }
    }
}
