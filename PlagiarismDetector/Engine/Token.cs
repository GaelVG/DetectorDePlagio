namespace PlagiarismDetector.Engine
{
    /// <summary>
    /// Represents a single token produced by the Lexer.
    /// A token is a meaningful unit extracted from the character stream.
    /// </summary>
    public class Token
    {
        public TokenType Type     { get; }
        public string    Value    { get; }
        public int       Position { get; }   // character offset in source text

        public Token(TokenType type, string value, int position)
        {
            Type     = type;
            Value    = value;
            Position = position;
        }

        public override string ToString()
            => $"[{Type}] \"{Value}\" @{Position}";
    }
}
