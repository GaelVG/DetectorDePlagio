using System;
using System.IO;
using System.Linq;

namespace PlagiarismDetector.Engine
{
    /// <summary>
    /// Reads a text file, preprocesses it, and runs the Lexer to produce a ProcessedDocument.
    /// </summary>
    public class DocumentProcessor
    {
        /// <summary>
        /// Reads a .txt file and performs lexical analysis.
        /// </summary>
        public static ProcessedDocument Process(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Archivo no encontrado: {filePath}");

            string rawText = File.ReadAllText(filePath);
            return ProcessText(rawText, Path.GetFileName(filePath), filePath);
        }

        /// <summary>
        /// Processes a raw text string directly (used by the UI when pasting text).
        /// </summary>
        public static ProcessedDocument ProcessText(string rawText, string name = "Documento", string filePath = "")
        {
            // ── Lexical analysis ────────────────────────────────────────────
            var lexer  = new Lexer(rawText);
            var tokens = lexer.Tokenize();

            // ── Extract normalized words for similarity comparison ────────
            var words = tokens
                .Where(t => t.Type == TokenType.Word)
                .Select(t => t.Value.ToLowerInvariant())
                .ToList();

            return new ProcessedDocument
            {
                Name     = name,
                FilePath = filePath,
                RawText  = rawText,
                Tokens   = tokens,
                Words    = words
            };
        }
    }
}
