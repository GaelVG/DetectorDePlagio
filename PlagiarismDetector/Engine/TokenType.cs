namespace PlagiarismDetector.Engine
{
    /// <summary>
    /// Categories of tokens produced by the Lexer (Análisis Léxico).
    /// </summary>
    public enum TokenType
    {
        Word,           // sequence of letters (a-z, A-Z) — words
        Number,         // sequence of digits (0-9)
        SpecialChar,    // + - * / = ( ) { } ; , _
        Whitespace,     // space, tab, newline
        StringLiteral,  // "text inside quotes"
        Boolean,        // #t or #f
        Unknown         // character NOT in Σ (e.g. @, $, #, etc.)
    }
}
