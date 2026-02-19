using System;
using System.IO;
using System.Linq;

namespace PlagiarismDetector.Engine
{
    /// <summary>
    /// Procesador de Documentos.
    /// Lee archivos de texto, los preprocesa y ejecuta el Analizador Léxico
    /// para producir un DocumentoProcesado listo para la comparación.
    /// </summary>
    public class ProcesadorDocumentos
    {
        /// <summary>
        /// Lee un archivo .txt del disco y realiza el análisis léxico completo.
        /// </summary>
        /// <param name="rutaArchivo">Ruta absoluta al archivo de texto.</param>
        public static DocumentoProcesado Procesar(string rutaArchivo)
        {
            if (!File.Exists(rutaArchivo))
                throw new FileNotFoundException($"Archivo no encontrado: {rutaArchivo}");

            string textoOriginal = File.ReadAllText(rutaArchivo);
            return ProcesarTexto(textoOriginal, Path.GetFileName(rutaArchivo), rutaArchivo);
        }

        /// <summary>
        /// Procesa una cadena de texto directamente (para texto pegado en la interfaz).
        /// </summary>
        /// <param name="texto">Contenido textual a analizar.</param>
        /// <param name="nombre">Nombre descriptivo del documento.</param>
        /// <param name="rutaArchivo">Ruta del archivo (vacía si es texto directo).</param>
        public static DocumentoProcesado ProcesarTexto(
            string texto,
            string nombre      = "Documento",
            string rutaArchivo = "")
        {
            // ── Análisis léxico ─────────────────────────────────────────────
            var analizador = new AnalizadorLexico(texto);
            var tokens     = analizador.Tokenizar();

            // ── Extraer palabras normalizadas para el Motor de Similitud ────
            var palabras = tokens
                .Where(t => t.Tipo == TipoToken.Palabra)
                .Select(t => t.Valor.ToLowerInvariant())
                .ToList();

            return new DocumentoProcesado
            {
                Nombre        = nombre,
                RutaArchivo   = rutaArchivo,
                TextoOriginal = texto,
                Tokens        = tokens,
                Palabras      = palabras
            };
        }
    }
}
