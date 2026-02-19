using System.Collections.Generic;
using System.Linq;

namespace PlagiarismDetector.Engine
{
    /// <summary>
    /// Represents a fully processed document:
    /// raw text + token stream + computed statistics.
    /// </summary>
    public class ProcessedDocument
    {
        public string        Name       { get; set; } = string.Empty;
        public string        FilePath   { get; set; } = string.Empty;
        public string        RawText    { get; set; } = string.Empty;
        public List<Token>   Tokens     { get; set; } = new();

        // Normalized (lowercased) words only – used by similarity engine
        public List<string>  Words      { get; set; } = new();

        public int TotalTokens   => Tokens.Count;
        public int WordCount     => Words.Count;
        public int DistinctWords => Words.Distinct().Count();
    }
}
