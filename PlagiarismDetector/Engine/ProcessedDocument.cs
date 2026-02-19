using System.Collections.Generic;
using System.Linq;

namespace PlagiarismDetector.Engine
{
    /// <summary>
    /// Representa un documento completamente procesado:
    /// texto original + flujo de tokens + estadísticas calculadas.
    /// </summary>
    public class DocumentoProcesado
    {
        /// <summary>Nombre descriptivo del documento.</summary>
        public string Nombre { get; set; } = string.Empty;

        /// <summary>Ruta del archivo en disco (vacía si se ingresó texto directo).</summary>
        public string RutaArchivo { get; set; } = string.Empty;

        /// <summary>Texto original sin modificar.</summary>
        public string TextoOriginal { get; set; } = string.Empty;

        /// <summary>Flujo de tokens producido por el Analizador Léxico.</summary>
        public List<Token> Tokens { get; set; } = new();

        /// <summary>Palabras normalizadas en minúsculas — usadas por el Motor de Similitud.</summary>
        public List<string> Palabras { get; set; } = new();

        // ─── Estadísticas ──────────────────────────────────────────────────────
        public int TotalTokens      => Tokens.Count;
        public int CantidadPalabras => Palabras.Count;
        public int PalabrasDistintas => Palabras.Distinct().Count();
    }
}
