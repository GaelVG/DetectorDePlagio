using System.Drawing;
using System.Windows.Forms;
using PlagiarismDetector.Resources;

namespace PlagiarismDetector.Forms
{
    /// <summary>
    /// Panel de información del proyecto: descripción, objetivo, herramientas y algoritmos.
    /// </summary> hola prueba git
    public class PanelAcercaDe : Panel
    {
        public PanelAcercaDe()
        {
            BackColor  = EstilosApp.FondoOscuro;
            Dock       = DockStyle.Fill;
            ConstruirUI();
        }

        private void ConstruirUI()
        {
            var scroll = new Panel
            {
                Dock       = DockStyle.Fill,
                BackColor  = EstilosApp.FondoOscuro,
                AutoScroll = true
            };

            var contenedor = new Panel
            {
                BackColor = EstilosApp.FondoOscuro,
                Width     = 700,
                Height    = 700,
                Location  = new System.Drawing.Point(40, 30)
            };

            int y = 0;

            contenedor.Controls.Add(CrearEtiqueta("Sistema Detector de Plagio", EstilosApp.FuenteTitulo, EstilosApp.Acento, ref y, 0));
            contenedor.Controls.Add(CrearEtiqueta("Área: Tecnología  |  Subárea: Compilador", EstilosApp.FuenteCuerpo, EstilosApp.TextoSecundario, ref y, 4));

            contenedor.Controls.Add(Separador(ref y, 20));

            contenedor.Controls.Add(CrearEtiqueta("Descripción", EstilosApp.FuenteEncabezado, EstilosApp.TextoPrimario, ref y, 10));
            contenedor.Controls.Add(CrearEtiqueta(
                "El Sistema Detector de Plagio analiza y compara documentos digitales\n" +
                "para identificar similitudes entre textos. Utiliza técnicas de análisis\n" +
                "léxico y sintáctico, propias de los compiladores.",
                EstilosApp.FuenteCuerpo, EstilosApp.TextoSecundario, ref y, 4));

            contenedor.Controls.Add(Separador(ref y, 20));

            contenedor.Controls.Add(CrearEtiqueta("Objetivo", EstilosApp.FuenteEncabezado, EstilosApp.TextoPrimario, ref y, 10));
            contenedor.Controls.Add(CrearEtiqueta(
                "Desarrollar un sistema detector de plagio que permita analizar y\n" +
                "comparar documentos automáticamente usando análisis léxico y sintáctico.",
                EstilosApp.FuenteCuerpo, EstilosApp.TextoSecundario, ref y, 4));

            contenedor.Controls.Add(Separador(ref y, 20));

            contenedor.Controls.Add(CrearEtiqueta("Características", EstilosApp.FuenteEncabezado, EstilosApp.TextoPrimario, ref y, 10));
            string[] caracteristicas =
            {
                "• Análisis de textos con técnicas de compilador.",
                "• Comparación automática entre dos o más documentos.",
                "• Detección de similitudes en palabras, frases o estructuras.",
                "• Interfaz sencilla para cargar y analizar documentos."
            };
            foreach (var c in caracteristicas)
                contenedor.Controls.Add(CrearEtiqueta(c, EstilosApp.FuenteCuerpo, EstilosApp.TextoSecundario, ref y, 0));

            contenedor.Controls.Add(Separador(ref y, 20));

            contenedor.Controls.Add(CrearEtiqueta("Herramientas", EstilosApp.FuenteEncabezado, EstilosApp.TextoPrimario, ref y, 10));
            contenedor.Controls.Add(CrearEtiqueta("Lenguaje: C#\nTipo: Escritorio – Windows Forms (.NET 8)",
                EstilosApp.FuenteCuerpo, EstilosApp.TextoSecundario, ref y, 4));

            contenedor.Controls.Add(Separador(ref y, 20));

            contenedor.Controls.Add(CrearEtiqueta("Algoritmos de Similitud", EstilosApp.FuenteEncabezado, EstilosApp.TextoPrimario, ref y, 10));
            string[] algoritmos =
            {
                "• Similitud Coseno   (40%) – vectores de frecuencia de términos",
                "• Similitud Jaccard  (30%) – conjuntos de tipos de palabras",
                "• Similitud Bigramas (20%) – pares de palabras adyacentes",
                "• Similitud Trigramas(10%) – triples de palabras adyacentes"
            };
            foreach (var a in algoritmos)
                contenedor.Controls.Add(CrearEtiqueta(a, EstilosApp.FuenteCuerpo, EstilosApp.AcentoClaro, ref y, 0));

            contenedor.Height = y + 40;
            scroll.Controls.Add(contenedor);
            Controls.Add(scroll);
        }

        // ─── Métodos auxiliares ────────────────────────────────────────────────

        private Label CrearEtiqueta(string texto, Font fuente, System.Drawing.Color color, ref int y, int relleno)
        {
            y += relleno;
            var etiqueta = new Label
            {
                Text      = texto,
                Font      = fuente,
                ForeColor = color,
                BackColor = System.Drawing.Color.Transparent,
                Location  = new System.Drawing.Point(0, y),
                AutoSize  = true
            };
            y += etiqueta.PreferredHeight + 6;
            return etiqueta;
        }

        private Panel Separador(ref int y, int relleno)
        {
            y += relleno;
            var sep = new Panel
            {
                Height    = 1,
                Width     = 650,
                Location  = new System.Drawing.Point(0, y),
                BackColor = EstilosApp.Borde
            };
            y += 1 + relleno;
            return sep;
        }
    }
}
