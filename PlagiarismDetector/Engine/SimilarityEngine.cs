using System;
using System.Collections.Generic;
using System.Linq;

namespace PlagiarismDetector.Engine
{
    /// <summary>
    /// Result of comparing two documents.
    /// </summary>
    public class SimilarityReport
    {
        public string DocumentAName  { get; set; } = "";
        public string DocumentBName  { get; set; } = "";

        // Three independent metrics (0.0 – 1.0)
        public double CosineSimilarity   { get; set; }
        public double JaccardSimilarity  { get; set; }
        public double BigramSimilarity   { get; set; }
        public double TrigramSimilarity  { get; set; }

        /// <summary>Combined plagiarism score (0–100 %).</summary>
        public double CombinedScore =>
            Math.Round((CosineSimilarity * 0.40 +
                        JaccardSimilarity * 0.30 +
                        BigramSimilarity  * 0.20 +
                        TrigramSimilarity * 0.10) * 100, 2);

        /// <summary>Shared word types between the two documents.</summary>
        public List<string> CommonWords  { get; set; } = new();

        /// <summary>Shared 2-grams (adjacent word pairs).</summary>
        public List<string> CommonBigrams { get; set; } = new();

        /// <summary>Human-readable verdict.</summary>
        public string Verdict
        {
            get
            {
                return CombinedScore switch
                {
                    >= 80 => "⚠️ Alto riesgo de plagio",
                    >= 50 => "⚡ Posible plagio detectado",
                    >= 25 => "🔍 Similitud moderada",
                    _     => "✅ Sin indicios significativos de plagio"
                };
            }
        }
    }

    /// <summary>
    /// Similarity engine using three complementary algorithms.
    /// All algorithms operate on the normalized word lists produced by the Lexer.
    /// </summary>
    public class SimilarityEngine
    {
        // ────────────────────────────────────────────────────────────────────
        // Public API
        // ────────────────────────────────────────────────────────────────────

        public static SimilarityReport Compare(ProcessedDocument a, ProcessedDocument b)
        {
            var wordsA = a.Words;
            var wordsB = b.Words;

            var report = new SimilarityReport
            {
                DocumentAName   = a.Name,
                DocumentBName   = b.Name,
                CosineSimilarity  = Cosine(wordsA, wordsB),
                JaccardSimilarity = Jaccard(wordsA, wordsB),
                BigramSimilarity  = NgramSimilarity(wordsA, wordsB, 2),
                TrigramSimilarity = NgramSimilarity(wordsA, wordsB, 3),
                CommonWords       = CommonWordTypes(wordsA, wordsB),
                CommonBigrams     = CommonNgrams(wordsA, wordsB, 2)
            };

            return report;
        }

        // ────────────────────────────────────────────────────────────────────
        // Algorithm 1: Cosine Similarity on TF vectors
        // ────────────────────────────────────────────────────────────────────

        private static double Cosine(List<string> a, List<string> b)
        {
            if (a.Count == 0 || b.Count == 0) return 0;

            var freqA = TermFrequency(a);
            var freqB = TermFrequency(b);

            var allTerms = freqA.Keys.Union(freqB.Keys).ToList();

            double dot   = allTerms.Sum(t => Get(freqA, t) * Get(freqB, t));
            double normA = Math.Sqrt(freqA.Values.Sum(v => v * v));
            double normB = Math.Sqrt(freqB.Values.Sum(v => v * v));

            if (normA == 0 || normB == 0) return 0;
            return Math.Min(1.0, dot / (normA * normB));
        }

        // ────────────────────────────────────────────────────────────────────
        // Algorithm 2: Jaccard Similarity on word type sets
        // ────────────────────────────────────────────────────────────────────

        private static double Jaccard(List<string> a, List<string> b)
        {
            if (a.Count == 0 || b.Count == 0) return 0;

            var setA = new HashSet<string>(a);
            var setB = new HashSet<string>(b);

            int intersection = setA.Intersect(setB).Count();
            int union        = setA.Union(setB).Count();

            return union == 0 ? 0 : (double)intersection / union;
        }

        // ────────────────────────────────────────────────────────────────────
        // Algorithm 3: N-gram Similarity
        // ────────────────────────────────────────────────────────────────────

        private static double NgramSimilarity(List<string> a, List<string> b, int n)
        {
            var ngramsA = BuildNgrams(a, n);
            var ngramsB = BuildNgrams(b, n);

            if (ngramsA.Count == 0 || ngramsB.Count == 0) return 0;

            var setA = new HashSet<string>(ngramsA);
            var setB = new HashSet<string>(ngramsB);

            int intersection = setA.Intersect(setB).Count();
            int union        = setA.Union(setB).Count();

            return union == 0 ? 0 : (double)intersection / union;
        }

        // ────────────────────────────────────────────────────────────────────
        // Helpers
        // ────────────────────────────────────────────────────────────────────

        private static List<string> CommonWordTypes(List<string> a, List<string> b)
        {
            var setA = new HashSet<string>(a);
            var setB = new HashSet<string>(b);
            return setA.Intersect(setB).OrderBy(w => w).ToList();
        }

        private static List<string> CommonNgrams(List<string> a, List<string> b, int n)
        {
            var ngramsA = new HashSet<string>(BuildNgrams(a, n));
            var ngramsB = new HashSet<string>(BuildNgrams(b, n));
            return ngramsA.Intersect(ngramsB).OrderBy(g => g).Take(50).ToList();
        }

        private static List<string> BuildNgrams(List<string> words, int n)
        {
            var ngrams = new List<string>();
            for (int i = 0; i <= words.Count - n; i++)
                ngrams.Add(string.Join(" ", words.Skip(i).Take(n)));
            return ngrams;
        }

        private static Dictionary<string, double> TermFrequency(List<string> words)
        {
            var freq = new Dictionary<string, double>();
            foreach (var w in words)
                freq[w] = freq.TryGetValue(w, out double v) ? v + 1 : 1;
            return freq;
        }

        private static double Get(Dictionary<string, double> d, string key)
            => d.TryGetValue(key, out double v) ? v : 0;
    }
}
