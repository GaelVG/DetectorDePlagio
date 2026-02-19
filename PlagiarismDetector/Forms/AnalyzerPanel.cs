using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using PlagiarismDetector.Engine;
using PlagiarismDetector.Resources;

namespace PlagiarismDetector.Forms
{
    public class AnalyzerPanel : Panel
    {
        // ── Document slots ───────────────────────────────────────────────────
        private ProcessedDocument? _docA, _docB;

        private Label   _labelA = null!,    _labelB = null!;
        private Button  _btnLoadA = null!,  _btnLoadB = null!;
        private TextBox _textA = null!,     _textB = null!;
        private Button  _btnAnalyze = null!;

        // ── Result widgets ────────────────────────────────────────────────
        private Panel   _resultCard   = null!;
        private Label   _scoreLbl     = null!;
        private Label   _verdictLbl   = null!;
        private Label   _cosLbl       = null!;
        private Label   _jacLbl       = null!;
        private Label   _biLbl        = null!;
        private Label   _triLbl       = null!;
        private ListBox _commonWordsBox  = null!;
        private ListBox _commonBigramBox = null!;
        private ListBox _tokenListA      = null!;
        private ListBox _tokenListB      = null!;

        public AnalyzerPanel()
        {
            BackColor  = AppStyles.BgDark;
            Dock       = DockStyle.Fill;
            AutoScroll = true;
            BuildUI();
        }

        // ─────────────────────────────────────────────────────────────────────
        // Build the UI layout
        // ─────────────────────────────────────────────────────────────────────

        private void BuildUI()
        {
            int pad = 20;

            // ── Page title ──────────────────────────────────────────────────
            var title = new Label
            {
                Text      = "Analizador de Plagio",
                Font      = AppStyles.FontTitle,
                ForeColor = AppStyles.Accent,
                BackColor = Color.Transparent,
                AutoSize  = true,
                Location  = new Point(pad, pad)
            };
            Controls.Add(title);

            var sub = new Label
            {
                Text      = "Carga dos documentos y presiona «Analizar» para detectar similitudes.",
                Font      = AppStyles.FontBody,
                ForeColor = AppStyles.TextSecond,
                BackColor = Color.Transparent,
                AutoSize  = true,
                Location  = new Point(pad, pad + 36)
            };
            Controls.Add(sub);

            // ── Document A card ─────────────────────────────────────────────
            var cardA = MakeDocCard("Documento A", out _labelA, out _btnLoadA, out _textA);
            cardA.Location = new Point(pad, 90);
            Controls.Add(cardA);

            // ── Document B card ─────────────────────────────────────────────
            var cardB = MakeDocCard("Documento B", out _labelB, out _btnLoadB, out _textB);
            cardB.Location = new Point(pad + cardA.Width + pad, 90);
            Controls.Add(cardB);

            _btnLoadA.Tag = "A";
            _btnLoadB.Tag = "B";
            _btnLoadA.Click += OnLoadFile;
            _btnLoadB.Click += OnLoadFile;

            // ── Analyze button ──────────────────────────────────────────────
            _btnAnalyze = StyledButton("🔍  Analizar Documentos", AppStyles.Accent);
            _btnAnalyze.Width    = 260;
            _btnAnalyze.Height   = 46;
            _btnAnalyze.Location = new Point(pad, 90 + cardA.Height + 16);
            _btnAnalyze.Click   += OnAnalyze;
            Controls.Add(_btnAnalyze);

            // ── Results card (hidden until analysis) ────────────────────────
            _resultCard = new Panel
            {
                Location  = new Point(pad, 90 + cardA.Height + 74),
                Width     = cardA.Width * 2 + pad,
                Height    = 520,
                BackColor = AppStyles.BgPanel,
                BorderStyle = BorderStyle.None,
                Visible   = false
            };
            BuildResultCard(_resultCard);
            Controls.Add(_resultCard);
        }

        private Panel MakeDocCard(string header,
                                   out Label pathLabel,
                                   out Button loadButton,
                                   out TextBox previewBox)
        {
            var card = new Panel
            {
                Width     = 420,
                Height    = 310,
                BackColor = AppStyles.BgCard,
                Padding   = new Padding(14)
            };

            var hdr = new Label
            {
                Text      = header,
                Font      = AppStyles.FontHeading,
                ForeColor = AppStyles.AccentLight,
                BackColor = Color.Transparent,
                AutoSize  = true,
                Location  = new Point(14, 14)
            };
            card.Controls.Add(hdr);

            pathLabel = new Label
            {
                Text      = "Sin archivo cargado",
                Font      = AppStyles.FontSmall,
                ForeColor = AppStyles.TextSecond,
                BackColor = Color.Transparent,
                AutoSize  = false,
                Width     = 390,
                Location  = new Point(14, 44)
            };
            card.Controls.Add(pathLabel);

            loadButton = StyledButton("📂  Cargar archivo…", AppStyles.BgPanel);
            loadButton.Width    = 180;
            loadButton.Height   = 34;
            loadButton.Location = new Point(14, 68);
            card.Controls.Add(loadButton);

            var orLbl = new Label
            {
                Text      = "— o pega texto —",
                Font      = AppStyles.FontSmall,
                ForeColor = AppStyles.TextSecond,
                BackColor = Color.Transparent,
                AutoSize  = true,
                Location  = new Point(202, 76)
            };
            card.Controls.Add(orLbl);

            previewBox = new TextBox
            {
                Multiline   = true,
                ScrollBars  = ScrollBars.Vertical,
                Font        = AppStyles.FontMono,
                BackColor   = AppStyles.BgDark,
                ForeColor   = AppStyles.TextPrimary,
                BorderStyle = BorderStyle.FixedSingle,
                Location    = new Point(14, 108),
                Width       = 390,
                Height      = 188
            };
            card.Controls.Add(previewBox);

            return card;
        }

        private void BuildResultCard(Panel card)
        {
            int pad = 16;

            // ── Score + verdict ─────────────────────────────────────────────
            _scoreLbl = new Label
            {
                Text      = "–– %",
                Font      = new Font("Segoe UI", 36f, FontStyle.Bold),
                ForeColor = AppStyles.Accent,
                BackColor = Color.Transparent,
                AutoSize  = true,
                Location  = new Point(pad, pad)
            };
            card.Controls.Add(_scoreLbl);

            _verdictLbl = new Label
            {
                Text      = "",
                Font      = AppStyles.FontHeading,
                ForeColor = AppStyles.TextPrimary,
                BackColor = Color.Transparent,
                AutoSize  = true,
                Location  = new Point(pad + 140, pad + 10)
            };
            card.Controls.Add(_verdictLbl);

            // ── Individual metrics ─────────────────────────────────────────
            _cosLbl = MetricLabel("Coseno:",   new Point(pad, 90), card);
            _jacLbl = MetricLabel("Jaccard:",  new Point(pad, 118), card);
            _biLbl  = MetricLabel("Bigramas:", new Point(pad + 220, 90), card);
            _triLbl = MetricLabel("Trigramas:", new Point(pad + 220, 118), card);

            // ── Separator ───────────────────────────────────────────────────
            card.Controls.Add(new Panel
            {
                Height    = 1,
                Width     = card.Width - pad * 2,
                Location  = new Point(pad, 150),
                BackColor = AppStyles.Border
            });

            // ── Common words list ────────────────────────────────────────────
            AddSubHeading(card, "Palabras en común", new Point(pad, 162));
            _commonWordsBox = StyledListBox(new Point(pad, 186), 200, 140, card);

            // ── Common bigrams list ──────────────────────────────────────────
            AddSubHeading(card, "Frases comunes (bigramas)", new Point(pad + 220, 162));
            _commonBigramBox = StyledListBox(new Point(pad + 220, 186), 340, 140, card);

            // ── Token tables ─────────────────────────────────────────────────
            card.Controls.Add(new Panel
            {
                Height    = 1,
                Width     = card.Width - pad * 2,
                Location  = new Point(pad, 336),
                BackColor = AppStyles.Border
            });

            AddSubHeading(card, "Tokens – Documento A", new Point(pad, 348));
            _tokenListA = StyledListBox(new Point(pad, 372), 200, 120, card);

            AddSubHeading(card, "Tokens – Documento B", new Point(pad + 220, 348));
            _tokenListB = StyledListBox(new Point(pad + 220, 372), 200, 120, card);
        }

        // ─────────────────────────────────────────────────────────────────────
        // Event handlers
        // ─────────────────────────────────────────────────────────────────────

        private void OnLoadFile(object? sender, EventArgs e)
        {
            string tag = (sender as Button)?.Tag?.ToString() ?? "A";

            using var dlg = new OpenFileDialog
            {
                Title  = "Seleccionar archivo de texto",
                Filter = "Archivos de texto (*.txt)|*.txt|Todos los archivos (*.*)|*.*"
            };
            if (dlg.ShowDialog() != DialogResult.OK) return;

            try
            {
                var doc = DocumentProcessor.Process(dlg.FileName);
                if (tag == "A")
                {
                    _docA    = doc;
                    _labelA.Text  = Path.GetFileName(dlg.FileName);
                    _textA.Text   = doc.RawText.Length > 3000
                                     ? doc.RawText.Substring(0, 3000) + "…"
                                     : doc.RawText;
                }
                else
                {
                    _docB    = doc;
                    _labelB.Text  = Path.GetFileName(dlg.FileName);
                    _textB.Text   = doc.RawText.Length > 3000
                                     ? doc.RawText.Substring(0, 3000) + "…"
                                     : doc.RawText;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar archivo:\n{ex.Message}",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnAnalyze(object? sender, EventArgs e)
        {
            // Allow pasted text too
            if (_docA == null && !string.IsNullOrWhiteSpace(_textA.Text))
                _docA = DocumentProcessor.ProcessText(_textA.Text, "Documento A");

            if (_docB == null && !string.IsNullOrWhiteSpace(_textB.Text))
                _docB = DocumentProcessor.ProcessText(_textB.Text, "Documento B");

            if (_docA == null || _docB == null)
            {
                MessageBox.Show("Por favor carga o pega ambos documentos antes de analizar.",
                                "Faltan documentos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SimilarityReport report = SimilarityEngine.Compare(_docA, _docB);
            ShowResults(report);
        }

        private void ShowResults(SimilarityReport r)
        {
            Color scoreColor = AppStyles.ScoreColor(r.CombinedScore);
            _scoreLbl.Text      = $"{r.CombinedScore:F1} %";
            _scoreLbl.ForeColor = scoreColor;
            _verdictLbl.Text    = r.Verdict;
            _verdictLbl.ForeColor = scoreColor;

            _cosLbl.Text = $"Coseno:    {r.CosineSimilarity * 100:F1} %";
            _jacLbl.Text = $"Jaccard:   {r.JaccardSimilarity * 100:F1} %";
            _biLbl.Text  = $"Bigramas:  {r.BigramSimilarity * 100:F1} %";
            _triLbl.Text = $"Trigramas: {r.TrigramSimilarity * 100:F1} %";

            _commonWordsBox.Items.Clear();
            foreach (var w in r.CommonWords.Take(100))
                _commonWordsBox.Items.Add(w);

            _commonBigramBox.Items.Clear();
            foreach (var b in r.CommonBigrams.Take(50))
                _commonBigramBox.Items.Add(b);

            _tokenListA.Items.Clear();
            if (_docA != null)
            {
                var grouped = _docA.Tokens
                    .Where(t => t.Type != TokenType.Whitespace)
                    .GroupBy(t => t.Type)
                    .OrderByDescending(g => g.Count());
                foreach (var g in grouped)
                    _tokenListA.Items.Add($"{g.Key}: {g.Count()} tokens");
            }

            _tokenListB.Items.Clear();
            if (_docB != null)
            {
                var grouped = _docB.Tokens
                    .Where(t => t.Type != TokenType.Whitespace)
                    .GroupBy(t => t.Type)
                    .OrderByDescending(g => g.Count());
                foreach (var g in grouped)
                    _tokenListB.Items.Add($"{g.Key}: {g.Count()} tokens");
            }

            _resultCard.Visible = true;
        }

        // ─────────────────────────────────────────────────────────────────────
        // Small helpers
        // ─────────────────────────────────────────────────────────────────────

        private Button StyledButton(string text, Color bg)
        {
            var btn = new Button
            {
                Text      = text,
                Font      = AppStyles.FontBody,
                BackColor = bg,
                ForeColor = AppStyles.TextPrimary,
                FlatStyle = FlatStyle.Flat,
                Cursor    = Cursors.Hand
            };
            btn.FlatAppearance.BorderColor = AppStyles.Border;
            btn.FlatAppearance.MouseOverBackColor = AppStyles.AccentHover;
            return btn;
        }

        private Label MetricLabel(string prefix, Point loc, Panel parent)
        {
            var lbl = new Label
            {
                Text      = prefix,
                Font      = AppStyles.FontBody,
                ForeColor = AppStyles.TextSecond,
                BackColor = Color.Transparent,
                AutoSize  = true,
                Location  = loc
            };
            parent.Controls.Add(lbl);
            return lbl;
        }

        private void AddSubHeading(Panel parent, string text, Point loc)
        {
            parent.Controls.Add(new Label
            {
                Text      = text,
                Font      = AppStyles.FontBody,
                ForeColor = AppStyles.AccentLight,
                BackColor = Color.Transparent,
                AutoSize  = true,
                Location  = loc
            });
        }

        private ListBox StyledListBox(Point loc, int w, int h, Panel parent)
        {
            var lb = new ListBox
            {
                Location  = loc,
                Width     = w,
                Height    = h,
                BackColor = AppStyles.BgDark,
                ForeColor = AppStyles.TextPrimary,
                Font      = AppStyles.FontMono,
                BorderStyle = BorderStyle.FixedSingle,
                ScrollAlwaysVisible = false
            };
            parent.Controls.Add(lb);
            return lb;
        }
    }
}
