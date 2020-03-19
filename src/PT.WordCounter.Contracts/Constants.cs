using System;
using System.Text;
using System.Collections.Generic;

namespace PT.WordCounter.Contracts
{
    public static class Constants
    {
        public const byte R = 0x0D;
        public const byte N = 0x0A;
        public const byte SPACE = 0x20;
        public static int THREADS_COUNT = Environment.ProcessorCount > 2 ? Environment.ProcessorCount : 2;

        public static Encoding EncodingWin1251 = Encoding.GetEncoding("Windows-1251");
        public static char[] LineSeparators = new[] { '\n', '\r', '`', '\\' };
        public static HashSet<byte> NonVocabularyCharacters = new HashSet<byte> {
            0x0D,
            0x0A,
            0x20,
            0x2C,
            0x2E,
            0
        };
    }
}
