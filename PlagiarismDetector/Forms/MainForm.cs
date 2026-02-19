using System;
using System.Drawing;
using System.Windows.Forms;
using PlagiarismDetector.Resources;

namespace PlagiarismDetector.Forms
{
    public class MainForm : Form
    {
        private Panel   _sidebar     = null!;
        private Panel   _contentArea = null!;

        // Panels
        private AnalyzerPanel  _analyzerPanel  = null!;
        private AlphabetPanel  _alphabetPanel  = null!;
        private AboutPanel     _aboutPanel     = null!;

        // Nav buttons
        private Button _btnAnalyzer = null!;
        private Button _btnAlphabet = null!;
        private Button _btnAbout    = null!;
        private Button _activeBtn   = null!;

        public MainForm()
        {
            Text            = "Sistema Detector de Plagio";
            Size            = new Size(1100, 740);
            MinimumSize     = new Size(960, 680);
            StartPosition   = FormStartPosition.CenterScreen;
            BackColor       = AppStyles.BgDark;
            FormBorderStyle = FormBorderStyle.Sizable;
            Icon            = SystemIcons.Application;

            BuildLayout();
            ShowPanel(_analyzerPanel, _btnAnalyzer);
        }

        private void BuildLayout()
        {
            // ── Sidebar ─────────────────────────────────────────────────────
            _sidebar = new Panel
            {
                Width     = AppStyles.SidebarWidth,
                Dock      = DockStyle.Left,
                BackColor = AppStyles.Sidebar
            };

            // Logo / app name
            var logoPanel = new Panel
            {
                Height    = 80,
                Dock      = DockStyle.Top,
                BackColor = AppStyles.BgPanel,
                Padding   = new Padding(16, 0, 0, 0)
            };
            logoPanel.Controls.Add(new Label
            {
                Text      = "🔎",
                Font      = new Font("Segoe UI", 22f),
                ForeColor = AppStyles.Accent,
                BackColor = Color.Transparent,
                AutoSize  = true,
                Location  = new Point(14, 8)
            });
            logoPanel.Controls.Add(new Label
            {
                Text      = "Detector\nde Plagio",
                Font      = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = AppStyles.TextPrimary,
                BackColor = Color.Transparent,
                AutoSize  = true,
                Location  = new Point(50, 16)
            });
            _sidebar.Controls.Add(logoPanel);

            // Nav section label
            var navSection = new Label
            {
                Text      = "NAVEGACIÓN",
                Font      = new Font("Segoe UI", 7.5f),
                ForeColor = AppStyles.TextSecond,
                BackColor = Color.Transparent,
                AutoSize  = true,
                Location  = new Point(14, 96)
            };
            _sidebar.Controls.Add(navSection);

            // Nav buttons
            _btnAnalyzer = NavButton("🔍  Analizar",   120);
            _btnAlphabet = NavButton("Σ  Alfabeto",    160);
            _btnAbout    = NavButton("ℹ️  Acerca de",  200);

            _btnAnalyzer.Click += (_, _) => ShowPanel(_analyzerPanel, _btnAnalyzer);
            _btnAlphabet.Click += (_, _) => ShowPanel(_alphabetPanel, _btnAlphabet);
            _btnAbout.Click    += (_, _) => ShowPanel(_aboutPanel,    _btnAbout);

            _sidebar.Controls.Add(_btnAnalyzer);
            _sidebar.Controls.Add(_btnAlphabet);
            _sidebar.Controls.Add(_btnAbout);

            // Version footer
            _sidebar.Controls.Add(new Label
            {
                Text      = "v1.0  –  .NET 8",
                Font      = AppStyles.FontSmall,
                ForeColor = AppStyles.TextSecond,
                BackColor = Color.Transparent,
                AutoSize  = true,
                Location  = new Point(14, 680)
            });

            // ── Content area ────────────────────────────────────────────────
            _contentArea = new Panel
            {
                Dock      = DockStyle.Fill,
                BackColor = AppStyles.BgDark,
                Padding   = new Padding(0)
            };

            // ── Header bar ──────────────────────────────────────────────────
            var headerBar = new Panel
            {
                Height    = 42,
                Dock      = DockStyle.Top,
                BackColor = AppStyles.BgPanel,
                Padding   = new Padding(20, 0, 0, 0)
            };
            headerBar.Controls.Add(new Label
            {
                Text      = "Sistema Detector de Plagio  |  Área: Tecnología – Compilador",
                Font      = AppStyles.FontSmall,
                ForeColor = AppStyles.TextSecond,
                BackColor = Color.Transparent,
                AutoSize  = true,
                Location  = new Point(20, 12)
            });
            _contentArea.Controls.Add(headerBar);

            // ── Create panels ────────────────────────────────────────────────
            _analyzerPanel = new AnalyzerPanel { Visible = false, Dock = DockStyle.Fill };
            _alphabetPanel = new AlphabetPanel { Visible = false, Dock = DockStyle.Fill };
            _aboutPanel    = new AboutPanel    { Visible = false, Dock = DockStyle.Fill };

            _contentArea.Controls.Add(_analyzerPanel);
            _contentArea.Controls.Add(_alphabetPanel);
            _contentArea.Controls.Add(_aboutPanel);

            Controls.Add(_contentArea);
            Controls.Add(_sidebar);
        }

        private void ShowPanel(Panel panel, Button navBtn)
        {
            // Hide all
            _analyzerPanel.Visible = false;
            _alphabetPanel.Visible = false;
            _aboutPanel.Visible    = false;

            // Reset previous active style
            if (_activeBtn != null)
            {
                _activeBtn.BackColor = Color.Transparent;
                _activeBtn.ForeColor = AppStyles.TextSecond;
            }

            // Show selected
            panel.Visible   = true;
            panel.BringToFront();
            navBtn.BackColor = AppStyles.Accent;
            navBtn.ForeColor = AppStyles.TextPrimary;
            _activeBtn       = navBtn;
        }

        private Button NavButton(string text, int top)
        {
            var btn = new Button
            {
                Text      = text,
                Font      = AppStyles.FontBody,
                ForeColor = AppStyles.TextSecond,
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Width     = AppStyles.SidebarWidth,
                Height    = 40,
                Location  = new Point(0, top),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding   = new Padding(14, 0, 0, 0),
                Cursor    = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize           = 0;
            btn.FlatAppearance.MouseOverBackColor   = AppStyles.BgCard;
            return btn;
        }
    }
}
