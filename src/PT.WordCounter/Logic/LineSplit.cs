using System;
using System.Collections.Generic;
using PT.WordCounter.Contracts;

namespace PT.WordCounter.Logic
{
    public static class LineSplit
    {
        public static IEnumerable<byte[]> Split(byte[] source)
        {
            if (source != null)
            {
                var offset = 0;
                for (int i = 0; i < source.Length; i++)
                {
                    var value = source[i];
                    if ( Constants.NonVocabularyCharacters.Contains(value))
                    {
                        if (i > offset)
                        {
                            yield return CreateData(source, offset, i);
                        }
                        offset = i + 1;
                    }
                }

                var data = CreateData(source, offset, source.Length);
                if (data.Length > 0)
                {
                    yield return CreateData(source, offset, source.Length);
                }
            }
        }

        private static byte[] CreateData(byte[] source, int offset, int i)
        {
            var length = i - offset;
            var data = new byte[length];
            Array.Copy(source, offset, data, 0, length);

            return data;
        }
    }
}
