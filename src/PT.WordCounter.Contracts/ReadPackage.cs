namespace PT.WordCounter.Contracts
{
    public struct ReadPackage
    {
        public string TextLine { get; }
        public ReadPackage(string textLine)
        {
            TextLine = textLine ?? string.Empty;
        }
    }
}
