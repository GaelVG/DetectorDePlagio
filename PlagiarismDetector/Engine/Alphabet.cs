using System;
using System.Collections.Generic;
using System.Linq;

namespace PlagiarismDetector.Engine
{
    /// <summary>
    /// Represents the formal Alphabet Σ (Sigma) as defined in compiler theory.
    /// An alphabet is a finite, explicitly defined set of symbols.
    /// </summary>
    public class Alphabet
    {
        // ────────────────────────────────────────────────────────────────────────
        // Symbol groups that make up our Σ
        // ────────────────────────────────────────────────────────────────────────
        public static readonly HashSet<char> LowercaseLetters =
            new HashSet<char>("abcdefghijklmnopqrstuvwxyz");

        public static readonly HashSet<char> UppercaseLetters =
            new HashSet<char>("ABCDEFGHIJKLMNOPQRSTUVWXYZ");

        public static readonly HashSet<char> Digits =
            new HashSet<char>("0123456789");

        public static readonly HashSet<char> SpecialChars =
            new HashSet<char>("+-*/=(){};,_");

        public static readonly HashSet<char> Whitespace =
            new HashSet<char>(new[] { ' ', '\t', '\n', '\r' });

        // The complete alphabet Σ (union of all groups)
        public static readonly HashSet<char> Sigma;

        // The empty alphabet ()
        public static readonly HashSet<char> EmptyAlphabet = new HashSet<char>();

        // Epsilon ε – the null/empty symbol
        public const char Epsilon = '\0';

        static Alphabet()
        {
            Sigma = new HashSet<char>();
            foreach (var c in LowercaseLetters) Sigma.Add(c);
            foreach (var c in UppercaseLetters) Sigma.Add(c);
            foreach (var c in Digits) Sigma.Add(c);
            foreach (var c in SpecialChars) Sigma.Add(c);
            Sigma.Add(' '); // space is part of Σ
        }

        // ────────────────────────────────────────────────────────────────────────
        // Membership: s ∈ Σ ?
        // ────────────────────────────────────────────────────────────────────────
        public static bool BelongsTo(char symbol, HashSet<char> alphabet)
            => alphabet.Contains(symbol);

        public static bool BelongsToSigma(char symbol)
            => Sigma.Contains(symbol);

        // ────────────────────────────────────────────────────────────────────────
        // Cardinality: |Σ|
        // ────────────────────────────────────────────────────────────────────────
        public static int Cardinality(HashSet<char> alphabet) => alphabet.Count;
        public static int SigmaCardinality => Sigma.Count;

        // ────────────────────────────────────────────────────────────────────────
        // Sub-Alphabet: S1 ⊆ S2 ?
        // ────────────────────────────────────────────────────────────────────────
        public static bool IsSubAlphabetOf(HashSet<char> s1, HashSet<char> s2)
            => s1.IsSubsetOf(s2);

        // ────────────────────────────────────────────────────────────────────────
        // Equality: S1 = S2 (order doesn't matter)
        // ────────────────────────────────────────────────────────────────────────
        public static bool AreEqual(HashSet<char> a1, HashSet<char> a2)
            => a1.SetEquals(a2);

        // ────────────────────────────────────────────────────────────────────────
        // Helper: get group name for a char
        // ────────────────────────────────────────────────────────────────────────
        public static string GetSymbolGroup(char c)
        {
            if (LowercaseLetters.Contains(c)) return "Letra minúscula";
            if (UppercaseLetters.Contains(c)) return "Letra mayúscula";
            if (Digits.Contains(c)) return "Dígito";
            if (SpecialChars.Contains(c)) return "Carácter especial";
            if (c == ' ') return "Espacio";
            return "No pertenece al alfabeto (Σ)";
        }

        // ────────────────────────────────────────────────────────────────────────
        // Full Σ as a sorted display string
        // ────────────────────────────────────────────────────────────────────────
        public static string SigmaAsString()
        {
            var letters = new string(LowercaseLetters.OrderBy(c => c).ToArray());
            var upper   = new string(UppercaseLetters.OrderBy(c => c).ToArray());
            var nums    = new string(Digits.OrderBy(c => c).ToArray());
            var sp      = new string(SpecialChars.OrderBy(c => c).ToArray());
            return $"Letras minúsculas: {letters}\n" +
                   $"Letras mayúsculas: {upper}\n" +
                   $"Dígitos: {nums}\n" +
                   $"Especiales: {sp}\n" +
                   $"Espacio: ' '";
        }
    }
}
