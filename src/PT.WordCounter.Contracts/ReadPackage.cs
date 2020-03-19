using System;

namespace PT.WordCounter.Contracts
{
    public struct ReadPackage
    {
        /// <summary>
        /// String at Windows-1251 encoding
        /// </summary>
        public byte[] TextLine { get; }
        public ReadPackage(byte[] textLine)
        {
            TextLine = textLine ?? Array.Empty<byte>();
        }
    }
}
