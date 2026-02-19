using System.Windows.Forms;
using System.Drawing;
using PlagiarismDetector.Resources;

namespace PlagiarismDetector.Forms
{
    public class AboutPanel : Panel
    {
        public AboutPanel()
        {
            BackColor = AppStyles.BgDark;
            Dock      = DockStyle.Fill;

            BuildUI();
        }

        private void BuildUI()
        {
            var scroll = new Panel
            {
                Dock      = DockStyle.Fill,
                BackColor = AppStyles.BgDark,
                AutoScroll = true
            };

            var inner = new Panel
            {
                BackColor = AppStyles.BgDark,
                Width     = 700,
                Height    = 700,
                Location  = new Point(40, 30)
            };

            int y = 0;

            inner.Controls.Add(MakeLabel("Sistema Detector de Plagio", AppStyles.FontTitle, AppStyles.Accent, ref y, 0));
            inner.Controls.Add(MakeLabel("Área: Tecnología  |  Subárea: Compilador", AppStyles.FontBody, AppStyles.TextSecond, ref y, 4));

            inner.Controls.Add(Separator(ref y, 20));

            inner.Controls.Add(MakeLabel("Descripción", AppStyles.FontHeading, AppStyles.TextPrimary, ref y, 10));
            inner.Controls.Add(MakeLabel(
                "El Sistema Detector de Plagio analiza y compara documentos digitales\n" +
                "para identificar similitudes entre textos. Utiliza técnicas de análisis\n" +
                "léxico y sintáctico, propias de los compiladores, para detectar\n" +
                "coincidencias que puedan indicar plagio.",
                AppStyles.FontBody, AppStyles.TextSecond, ref y, 4));

            inner.Controls.Add(Separator(ref y, 20));

            inner.Controls.Add(MakeLabel("Objetivo", AppStyles.FontHeading, AppStyles.TextPrimary, ref y, 10));
            inner.Controls.Add(MakeLabel(
                "Desarrollar un sistema detector de plagio que permita analizar y\n" +
                "comparar documentos de manera automática, utilizando análisis léxico\n" +
                "y sintáctico para determinar posibles casos de plagio.",
                AppStyles.FontBody, AppStyles.TextSecond, ref y, 4));

            inner.Controls.Add(Separator(ref y, 20));

            inner.Controls.Add(MakeLabel("Características", AppStyles.FontHeading, AppStyles.TextPrimary, ref y, 10));
            string[] features =
            {
                "• Análisis de textos mediante técnicas propias de un compilador.",
                "• Comparación automática entre dos o más documentos.",
                "• Detección de similitudes en palabras, frases o estructuras del texto.",
                "• Interfaz sencilla para cargar y analizar documentos."
            };
            foreach (var f in features)
                inner.Controls.Add(MakeLabel(f, AppStyles.FontBody, AppStyles.TextSecond, ref y, 0));

            inner.Controls.Add(Separator(ref y, 20));

            inner.Controls.Add(MakeLabel("Herramientas", AppStyles.FontHeading, AppStyles.TextPrimary, ref y, 10));
            inner.Controls.Add(MakeLabel("Lenguaje: C#\nTipo: Escritorio – Windows Forms (.NET 8)",
                AppStyles.FontBody, AppStyles.TextSecond, ref y, 4));

            inner.Controls.Add(Separator(ref y, 20));

            inner.Controls.Add(MakeLabel("Algoritmos de Similitud", AppStyles.FontHeading, AppStyles.TextPrimary, ref y, 10));
            string[] algos =
            {
                "• Similitud Coseno (40%) – vectores de frecuencia de términos",
                "• Similitud de Jaccard (30%) – conjuntos de tipos de palabras",
                "• Similitud de Bigramas (20%) – pares de palabras adyacentes",
                "• Similitud de Trigramas (10%) – triples de palabras adyacentes"
            };
            foreach (var a in algos)
                inner.Controls.Add(MakeLabel(a, AppStyles.FontBody, AppStyles.AccentLight, ref y, 0));

            inner.Height = y + 40;
            scroll.Controls.Add(inner);
            Controls.Add(scroll);
        }

        private Label MakeLabel(string text, Font font, Color color, ref int y, int topPad)
        {
            y += topPad;
            var lbl = new Label
            {
                Text      = text,
                Font      = font,
                ForeColor = color,
                BackColor = Color.Transparent,
                Location  = new Point(0, y),
                AutoSize  = true
            };
            y += lbl.PreferredHeight + 6;
            return lbl;
        }

        private Panel Separator(ref int y, int pad)
        {
            y += pad;
            var sep = new Panel
            {
                Height    = 1,
                Width     = 650,
                Location  = new Point(0, y),
                BackColor = AppStyles.Border
            };
            y += 1 + pad;
            return sep;
        }
    }
}
