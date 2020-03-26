using System;
using System.Text;

namespace PT.WordCounter.Contracts
{
    public static class Constants
    {
        public const char SPACE = ' ';
        public static int THREADS_COUNT = Environment.ProcessorCount > 2 ? Environment.ProcessorCount : 2;

        public static Encoding EncodingWin1251 = Encoding.GetEncoding("Windows-1251");
        public static char[] LineSeparators = new[] { '\n', '\r', '`', '\\' };
    }
}
