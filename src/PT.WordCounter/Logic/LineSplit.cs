using System;
using System.Collections.Generic;

namespace PT.WordCounter.Logic
{
    public static class LineSplit
    {
        public static IEnumerable<string> Split(string source)
        {
            source = source ?? string.Empty;
            return source.Split(new char[] { }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
