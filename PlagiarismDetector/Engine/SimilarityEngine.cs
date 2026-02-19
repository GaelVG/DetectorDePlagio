using System;
using System.Collections.Generic;
using System.Linq;

namespace PlagiarismDetector.Engine
{
    /// <summary>
    /// Resultado de la comparación entre dos documentos.
    /// Incluye métricas individuales y puntuación combinada de plagio.
    /// </summary>
    public class ReporteSimilitud
    {
        // ─── Identificación ──────────────────────────────────────────────────
        public string NombreDocumentoA { get; set; } = "";
        public string NombreDocumentoB { get; set; } = "";

        // ─── Métricas individuales (0.0 – 1.0) ──────────────────────────────
        public double SimilitudCoseno    { get; set; }
        public double SimilitudJaccard   { get; set; }
        public double SimilitudBigramas  { get; set; }
        public double SimilitudTrigramas { get; set; }

        /// <summary>Puntuación combinada de plagio (0 – 100 %).</summary>
        public double PuntuacionCombinada =>
            Math.Round((SimilitudCoseno    * 0.40 +
                        SimilitudJaccard   * 0.30 +
                        SimilitudBigramas  * 0.20 +
                        SimilitudTrigramas * 0.10) * 100, 2);

        /// <summary>Palabras compartidas entre los dos documentos.</summary>
        public List<string> PalabrasComunes { get; set; } = new();

        /// <summary>Bigramas (pares de palabras) compartidos.</summary>
        public List<string> BigramasComunes { get; set; } = new();

        /// <summary>Veredicto legible según la puntuación combinada.</summary>
        public string Veredicto => PuntuacionCombinada switch
        {
            >= 80 => "⚠️ Alto riesgo de plagio",
            >= 50 => "⚡ Posible plagio detectado",
            >= 25 => "🔍 Similitud moderada",
            _     => "✅ Sin indicios significativos de plagio"
        };
    }

    /// <summary>
    /// Motor de Similitud.
    /// Compara dos documentos usando tres algoritmos complementarios aplicados
    /// sobre las listas de palabras producidas por el Analizador Léxico.
    /// </summary>
    public class MotorSimilitud
    {
        // ─── API pública ─────────────────────────────────────────────────────

        /// <summary>Compara dos documentos y devuelve un ReporteSimilitud.</summary>
        public static ReporteSimilitud Comparar(DocumentoProcesado a, DocumentoProcesado b)
        {
            var palabrasA = a.Palabras;
            var palabrasB = b.Palabras;

            return new ReporteSimilitud
            {
                NombreDocumentoA   = a.Nombre,
                NombreDocumentoB   = b.Nombre,
                SimilitudCoseno    = Coseno(palabrasA, palabrasB),
                SimilitudJaccard   = Jaccard(palabrasA, palabrasB),
                SimilitudBigramas  = SimilitudNgrama(palabrasA, palabrasB, 2),
                SimilitudTrigramas = SimilitudNgrama(palabrasA, palabrasB, 3),
                PalabrasComunes    = TiposPalabrasComunes(palabrasA, palabrasB),
                BigramasComunes    = NgramasComunes(palabrasA, palabrasB, 2)
            };
        }

        // ─── Algoritmo 1: Similitud Coseno (vectores de frecuencia TF) ──────

        private static double Coseno(List<string> a, List<string> b)
        {
            if (a.Count == 0 || b.Count == 0) return 0;

            var frecA = FrecuenciaTerminos(a);
            var frecB = FrecuenciaTerminos(b);
            var todosTerminos = frecA.Keys.Union(frecB.Keys).ToList();

            double productoPunto = todosTerminos.Sum(t => Obtener(frecA, t) * Obtener(frecB, t));
            double normaA = Math.Sqrt(frecA.Values.Sum(v => v * v));
            double normaB = Math.Sqrt(frecB.Values.Sum(v => v * v));

            if (normaA == 0 || normaB == 0) return 0;
            return Math.Min(1.0, productoPunto / (normaA * normaB));
        }

        // ─── Algoritmo 2: Similitud de Jaccard (conjuntos de tipos de palabras)

        private static double Jaccard(List<string> a, List<string> b)
        {
            if (a.Count == 0 || b.Count == 0) return 0;

            var conjuntoA = new HashSet<string>(a);
            var conjuntoB = new HashSet<string>(b);
            int interseccion = conjuntoA.Intersect(conjuntoB).Count();
            int union        = conjuntoA.Union(conjuntoB).Count();

            return union == 0 ? 0 : (double)interseccion / union;
        }

        // ─── Algoritmo 3: Similitud de N-gramas (detección a nivel de frases)

        private static double SimilitudNgrama(List<string> a, List<string> b, int n)
        {
            var ngramasA = ConstruirNgramas(a, n);
            var ngramasB = ConstruirNgramas(b, n);

            if (ngramasA.Count == 0 || ngramasB.Count == 0) return 0;

            var conjA = new HashSet<string>(ngramasA);
            var conjB = new HashSet<string>(ngramasB);
            int interseccion = conjA.Intersect(conjB).Count();
            int union        = conjA.Union(conjB).Count();

            return union == 0 ? 0 : (double)interseccion / union;
        }

        // ─── Métodos auxiliares ───────────────────────────────────────────────

        private static List<string> TiposPalabrasComunes(List<string> a, List<string> b)
        {
            var conjA = new HashSet<string>(a);
            var conjB = new HashSet<string>(b);
            return conjA.Intersect(conjB).OrderBy(p => p).ToList();
        }

        private static List<string> NgramasComunes(List<string> a, List<string> b, int n)
        {
            var conjA = new HashSet<string>(ConstruirNgramas(a, n));
            var conjB = new HashSet<string>(ConstruirNgramas(b, n));
            return conjA.Intersect(conjB).OrderBy(g => g).Take(50).ToList();
        }

        private static List<string> ConstruirNgramas(List<string> palabras, int n)
        {
            var ngramas = new List<string>();
            for (int i = 0; i <= palabras.Count - n; i++)
                ngramas.Add(string.Join(" ", palabras.Skip(i).Take(n)));
            return ngramas;
        }

        private static Dictionary<string, double> FrecuenciaTerminos(List<string> palabras)
        {
            var frecuencia = new Dictionary<string, double>();
            foreach (var p in palabras)
                frecuencia[p] = frecuencia.TryGetValue(p, out double v) ? v + 1 : 1;
            return frecuencia;
        }

        private static double Obtener(Dictionary<string, double> dic, string clave)
            => dic.TryGetValue(clave, out double v) ? v : 0;
    }
}
