using System;
using System.Collections.Generic;

namespace PlagiarismDetector.Engine
{
    /// <summary>
    /// Lexical Analyzer (Análisis Léxico).
    /// Reads a text character by character and groups characters into Token objects.
    /// Uses the Alphabet Σ to validate each character.
    /// </summary>
    public class Lexer
    {
        private readonly string _input;
        private int _pos;

        public Lexer(string input)
        {
            _input = input ?? string.Empty;
            _pos = 0;
        }

        /// <summary>
        /// Tokenizes the entire input string and returns the token stream.
        /// </summary>
        public List<Token> Tokenize()
        {
            var tokens = new List<Token>();
            _pos = 0;

            while (_pos < _input.Length)
            {
                char current = _input[_pos];

                // ── String literals: "..." ───────────────────────────────────
                if (current == '"')
                {
                    tokens.Add(ReadStringLiteral());
                    continue;
                }

                // ── Boolean symbols: #t / #f ─────────────────────────────────
                if (current == '#' && _pos + 1 < _input.Length &&
                    (_input[_pos + 1] == 't' || _input[_pos + 1] == 'f'))
                {
                    tokens.Add(new Token(TokenType.Boolean, _input.Substring(_pos, 2), _pos));
                    _pos += 2;
                    continue;
                }

                // ── Letters → WORD token ─────────────────────────────────────
                if (Alphabet.LowercaseLetters.Contains(current) ||
                    Alphabet.UppercaseLetters.Contains(current) ||
                    current == '_')
                {
                    tokens.Add(ReadWord());
                    continue;
                }

                // ── Digits → NUMBER token ────────────────────────────────────
                if (Alphabet.Digits.Contains(current))
                {
                    tokens.Add(ReadNumber());
                    continue;
                }

                // ── Whitespace ───────────────────────────────────────────────
                if (Alphabet.Whitespace.Contains(current))
                {
                    tokens.Add(ReadWhitespace());
                    continue;
                }

                // ── Special chars in Σ ───────────────────────────────────────
                if (Alphabet.SpecialChars.Contains(current))
                {
                    tokens.Add(new Token(TokenType.SpecialChar, current.ToString(), _pos));
                    _pos++;
                    continue;
                }

                // ── Unknown (not in Σ): record and skip ──────────────────────
                tokens.Add(new Token(TokenType.Unknown, current.ToString(), _pos));
                _pos++;
            }

            return tokens;
        }

        // ── Helpers ──────────────────────────────────────────────────────────

        private Token ReadWord()
        {
            int start = _pos;
            while (_pos < _input.Length &&
                   (Alphabet.LowercaseLetters.Contains(_input[_pos]) ||
                    Alphabet.UppercaseLetters.Contains(_input[_pos]) ||
                    _input[_pos] == '_'))
            {
                _pos++;
            }
            return new Token(TokenType.Word, _input.Substring(start, _pos - start), start);
        }

        private Token ReadNumber()
        {
            int start = _pos;
            while (_pos < _input.Length && Alphabet.Digits.Contains(_input[_pos]))
                _pos++;
            return new Token(TokenType.Number, _input.Substring(start, _pos - start), start);
        }

        private Token ReadWhitespace()
        {
            int start = _pos;
            while (_pos < _input.Length && Alphabet.Whitespace.Contains(_input[_pos]))
                _pos++;
            return new Token(TokenType.Whitespace, _input.Substring(start, _pos - start), start);
        }

        private Token ReadStringLiteral()
        {
            int start = _pos;
            _pos++; // skip opening "
            while (_pos < _input.Length && _input[_pos] != '"')
                _pos++;
            if (_pos < _input.Length) _pos++; // skip closing "
            return new Token(TokenType.StringLiteral, _input.Substring(start, _pos - start), start);
        }
    }
}
