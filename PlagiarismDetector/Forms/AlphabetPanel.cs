using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using PlagiarismDetector.Engine;
using PlagiarismDetector.Resources;

namespace PlagiarismDetector.Forms
{
    public class AlphabetPanel : Panel
    {
        private TextBox _inputBox = null!;
        private Label   _resultLabel = null!;
        private Label   _groupLabel  = null!;

        public AlphabetPanel()
        {
            BackColor  = AppStyles.BgDark;
            Dock       = DockStyle.Fill;
            BuildUI();
        }

        private void BuildUI()
        {
            var scroll = new Panel { Dock = DockStyle.Fill, AutoScroll = true, BackColor = AppStyles.BgDark };
            var inner  = new Panel { Width = 800, Location = new Point(40, 30), BackColor = AppStyles.BgDark };

            int y = 0;

            // ── Title ──────────────────────────────────────────────────────
            AddLabel(inner, "Alfabeto Σ (Sigma) – Teoría Formal", AppStyles.FontTitle, AppStyles.Accent, ref y, 0);
            AddLabel(inner, "Conjunto finito de símbolos que el sistema reconoce y procesa", AppStyles.FontBody, AppStyles.TextSecond, ref y, 4);

            AddSep(inner, ref y, 20);

            // ── Cardinality card ───────────────────────────────────────────
            AddLabel(inner, $"Cardinalidad de Σ:  |Σ| = {Alphabet.SigmaCardinality} símbolos", AppStyles.FontHeading, AppStyles.AccentLight, ref y, 10);

            AddSep(inner, ref y, 20);

            // ── Symbol groups ──────────────────────────────────────────────
            AddSymbolGroup(inner, "Letras minúsculas  (a-z)", Alphabet.LowercaseLetters, ref y);
            AddSymbolGroup(inner, "Letras mayúsculas  (A-Z)", Alphabet.UppercaseLetters, ref y);
            AddSymbolGroup(inner, "Dígitos  (0-9)",           Alphabet.Digits,           ref y);
            AddSymbolGroup(inner, "Caracteres especiales",    Alphabet.SpecialChars,     ref y);
            AddLabel(inner, "Espacio → ' '", AppStyles.FontMono, AppStyles.TextSecond, ref y, 8);

            AddSep(inner, ref y, 20);

            // ── Properties ────────────────────────────────────────────────
            AddLabel(inner, "Propiedades del Alfabeto", AppStyles.FontHeading, AppStyles.TextPrimary, ref y, 10);
            string[] props = {
                "1. Puede ser vacío.",
                "2. Debe estar definido explícitamente.",
                "3. Debe ser finito.",
                "4. Todos sus elementos deben ser símbolos."
            };
            foreach (var p in props)
                AddLabel(inner, p, AppStyles.FontBody, AppStyles.TextSecond, ref y, 0);

            AddSep(inner, ref y, 20);

            // ── Interactive membership tester ──────────────────────────────
            AddLabel(inner, "Prueba de pertenencia  s ∈ Σ ?", AppStyles.FontHeading, AppStyles.TextPrimary, ref y, 10);
            AddLabel(inner, "Escribe un carácter para comprobar si pertenece al alfabeto:", AppStyles.FontBody, AppStyles.TextSecond, ref y, 4);

            _inputBox = new TextBox
            {
                MaxLength = 1,
                Font      = new Font("Consolas", 14f),
                BackColor = AppStyles.BgCard,
                ForeColor = AppStyles.TextPrimary,
                Width     = 60,
                Height    = 36,
                Location  = new Point(0, y),
                BorderStyle = BorderStyle.FixedSingle,
                TextAlign   = HorizontalAlignment.Center
            };
            _inputBox.TextChanged += OnInputChanged;
            inner.Controls.Add(_inputBox);
            y += 44;

            _resultLabel = new Label
            {
                Text      = "",
                Font      = AppStyles.FontHeading,
                ForeColor = AppStyles.TextPrimary,
                BackColor = Color.Transparent,
                Location  = new Point(0, y),
                AutoSize  = true
            };
            inner.Controls.Add(_resultLabel);
            y += 30;

            _groupLabel = new Label
            {
                Text      = "",
                Font      = AppStyles.FontBody,
                ForeColor = AppStyles.TextSecond,
                BackColor = Color.Transparent,
                Location  = new Point(0, y),
                AutoSize  = true
            };
            inner.Controls.Add(_groupLabel);
            y += 30;

            AddSep(inner, ref y, 20);

            // ── Sub-alphabet example ────────────────────────────────────────
            AddLabel(inner, "Sub-Alfabeto  S1 ⊆ S2", AppStyles.FontHeading, AppStyles.TextPrimary, ref y, 10);
            AddLabel(inner, "S1 = {a, b, c}  ⊆  S2 = Σ  →  verdadero", AppStyles.FontMono, AppStyles.AccentLight, ref y, 4);
            AddLabel(inner, "S1 = {a, b, c}  ⊆  S2 = {a, b}  →  falso", AppStyles.FontMono, AppStyles.AccentLight, ref y, 0);

            AddSep(inner, ref y, 20);

            // ── Epsilon ────────────────────────────────────────────────────
            AddLabel(inner, "Símbolo Nulo  ε (Epsilon)", AppStyles.FontHeading, AppStyles.TextPrimary, ref y, 10);
            AddLabel(inner, "Representa la ausencia de símbolo. Es el caso base en procesos recursivos\ndel compilador y la construcción de palabras vacías.",
                AppStyles.FontBody, AppStyles.TextSecond, ref y, 4);

            inner.Height = y + 40;
            scroll.Controls.Add(inner);
            Controls.Add(scroll);
        }

        private void OnInputChanged(object? sender, EventArgs e)
        {
            string text = _inputBox.Text;
            if (text.Length == 0)
            {
                _resultLabel.Text = "";
                _groupLabel.Text  = "";
                return;
            }

            char c = text[0];
            bool belongs = Alphabet.BelongsToSigma(c);
            string group = Alphabet.GetSymbolGroup(c);

            _resultLabel.ForeColor = belongs ? AppStyles.Success : AppStyles.Danger;
            _resultLabel.Text = belongs
                ? $"'{c}'  ∈  Σ  →  verdadero ✓"
                : $"'{c}'  ∉  Σ  →  falso ✗";

            _groupLabel.Text = group;
        }

        // ── Helpers ──────────────────────────────────────────────────────

        private void AddLabel(Panel parent, string text, Font font, Color color, ref int y, int topPad)
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
            parent.Controls.Add(lbl);
            y += lbl.PreferredHeight + 6;
        }

        private void AddSymbolGroup(Panel parent, string title, HashSet<char> chars, ref int y)
        {
            AddLabel(parent, title, AppStyles.FontBody, AppStyles.TextPrimary, ref y, 10);
            string symbols = string.Join("  ", chars.OrderBy(c => c));
            AddLabel(parent, symbols, AppStyles.FontMono, AppStyles.TextSecond, ref y, 2);
        }

        private void AddSep(Panel parent, ref int y, int pad)
        {
            y += pad;
            parent.Controls.Add(new Panel
            {
                Height    = 1,
                Width     = 750,
                Location  = new Point(0, y),
                BackColor = AppStyles.Border
            });
            y += 1 + pad;
        }
    }
}
