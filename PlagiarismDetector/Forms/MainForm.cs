using System;
using System.Drawing;
using System.Windows.Forms;
using PlagiarismDetector.Resources;

namespace PlagiarismDetector.Forms
{
    /// <summary>
    /// Formulario principal de la aplicación.
    /// Contiene la barra lateral de navegación y el área de contenido
    /// con los tres paneles: Analizador, Alfabeto y Acerca de.
    /// </summary>
    public class FormularioPrincipal : Form
    {
        // ─── Paneles de navegación y contenido ────────────────────────────────
        private Panel _barraLateral  = null!;
        private Panel _areaContenido = null!;

        // ─── Paneles de contenido ─────────────────────────────────────────────
        private PanelAnalizador _panelAnalizador = null!;
        private PanelAlfabeto   _panelAlfabeto   = null!;
        private PanelAcercaDe   _panelAcercaDe   = null!;

        // ─── Botones de navegación ─────────────────────────────────────────────
        private Button _btnAnalizador = null!;
        private Button _btnAlfabeto   = null!;
        private Button _btnAcercaDe   = null!;
        private Button _btnActivo     = null!;

        public FormularioPrincipal()
        {
            Text            = "Sistema Detector de Plagio";
            Size            = new Size(1100, 740);
            MinimumSize     = new Size(960, 680);
            StartPosition   = FormStartPosition.CenterScreen;
            BackColor       = EstilosApp.FondoOscuro;
            FormBorderStyle = FormBorderStyle.Sizable;
            Icon            = SystemIcons.Application;

            ConstruirDisposicion();
            MostrarPanel(_panelAnalizador, _btnAnalizador);
        }

        // ─── Construcción de la interfaz ───────────────────────────────────────
        private void ConstruirDisposicion()
        {
            // ── Barra lateral (sin Dock en hijos — solo coordenadas absolutas) ──
            _barraLateral = new Panel
            {
                Width     = EstilosApp.AnchoBarraLateral,
                Dock      = DockStyle.Left,
                BackColor = EstilosApp.BarraLateral
            };

            // ── Sección logo — TableLayoutPanel garantiza sin solapamiento ────
            var panelLogo = new Panel
            {
                Location  = new Point(0, 0),
                Width     = EstilosApp.AnchoBarraLateral,
                Height    = 80,
                BackColor = EstilosApp.FondoPanel
            };

            // TableLayoutPanel: col0=50px (emoji), col1=resto (texto)
            var tbl = new TableLayoutPanel
            {
                Location    = new Point(0, 0),
                Width       = EstilosApp.AnchoBarraLateral,
                Height      = 80,
                ColumnCount = 2,
                RowCount    = 1,
                BackColor   = Color.Transparent
            };
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50));   // emoji
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,  100));  // texto
            tbl.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            tbl.Controls.Add(new Label
            {
                Text      = "🔎",
                Font      = new Font("Segoe UI Emoji", 18f),
                ForeColor = EstilosApp.Acento,
                BackColor = Color.Transparent,
                Dock      = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            }, 0, 0);

            tbl.Controls.Add(new Label
            {
                Text      = "Detector\nde Plagio",
                Font      = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = EstilosApp.TextoPrimario,
                BackColor = Color.Transparent,
                Dock      = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            }, 1, 0);

            panelLogo.Controls.Add(tbl);
            _barraLateral.Controls.Add(panelLogo);

            // ── Etiqueta "NAVEGACIÓN" (y=88) ─────────────────────────────────
            _barraLateral.Controls.Add(new Label
            {
                Text      = "NAVEGACIÓN",
                Font      = new Font("Segoe UI", 7.5f),
                ForeColor = EstilosApp.TextoSecundario,
                BackColor = Color.Transparent,
                AutoSize  = true,
                Location  = new Point(14, 90)
            });

            // ── Botones de navegación (y=110, 152, 194) ───────────────────────
            _btnAnalizador = BotonNavegacion("🔍  Analizar",   110);
            _btnAlfabeto   = BotonNavegacion("Σ  Alfabeto",    152);
            _btnAcercaDe   = BotonNavegacion("ℹ️  Acerca de", 194);

            _btnAnalizador.Click += (_, _) => MostrarPanel(_panelAnalizador, _btnAnalizador);
            _btnAlfabeto.Click   += (_, _) => MostrarPanel(_panelAlfabeto,   _btnAlfabeto);
            _btnAcercaDe.Click   += (_, _) => MostrarPanel(_panelAcercaDe,   _btnAcercaDe);

            _barraLateral.Controls.Add(_btnAnalizador);
            _barraLateral.Controls.Add(_btnAlfabeto);
            _barraLateral.Controls.Add(_btnAcercaDe);

            // ── Versión al pie (anclada cerca del fondo) ─────────────────────
            _barraLateral.Controls.Add(new Label
            {
                Text      = "v1.0  –  .NET 8",
                Font      = EstilosApp.FuentePequena,
                ForeColor = EstilosApp.TextoSecundario,
                BackColor = Color.Transparent,
                AutoSize  = true,
                Location  = new Point(14, 650)
            });

            // ── Área de contenido ────────────────────────────────────────────
            _areaContenido = new Panel
            {
                Dock      = DockStyle.Fill,
                BackColor = EstilosApp.FondoOscuro
            };

            // Barra de encabezado superior
            var barraEncabezado = new Panel
            {
                Height    = 42,
                Dock      = DockStyle.Top,
                BackColor = EstilosApp.FondoPanel
            };
            barraEncabezado.Controls.Add(new Label
            {
                Text      = "Sistema Detector de Plagio  |  Área: Tecnología – Compilador",
                Font      = EstilosApp.FuentePequena,
                ForeColor = EstilosApp.TextoSecundario,
                BackColor = Color.Transparent,
                AutoSize  = true,
                Location  = new Point(20, 12)
            });
            _areaContenido.Controls.Add(barraEncabezado);

            // ── Crear paneles de contenido ────────────────────────────────────
            _panelAnalizador = new PanelAnalizador { Visible = false, Dock = DockStyle.Fill };
            _panelAlfabeto   = new PanelAlfabeto   { Visible = false, Dock = DockStyle.Fill };
            _panelAcercaDe   = new PanelAcercaDe   { Visible = false, Dock = DockStyle.Fill };

            _areaContenido.Controls.Add(_panelAnalizador);
            _areaContenido.Controls.Add(_panelAlfabeto);
            _areaContenido.Controls.Add(_panelAcercaDe);

            Controls.Add(_areaContenido);
            Controls.Add(_barraLateral);
        }

        // ─── Navegación entre paneles ──────────────────────────────────────────
        private void MostrarPanel(Panel panel, Button btnNav)
        {
            // Ocultar todos los paneles
            _panelAnalizador.Visible = false;
            _panelAlfabeto.Visible   = false;
            _panelAcercaDe.Visible   = false;

            // Restablecer estilo del botón anterior
            if (_btnActivo != null)
            {
                _btnActivo.BackColor = Color.Transparent;
                _btnActivo.ForeColor = EstilosApp.TextoSecundario;
            }

            // Mostrar el panel seleccionado y resaltar botón
            panel.Visible     = true;
            panel.BringToFront();
            btnNav.BackColor  = EstilosApp.Acento;
            btnNav.ForeColor  = EstilosApp.TextoPrimario;
            _btnActivo        = btnNav;
        }

        // ─── Crear botón de navegación ─────────────────────────────────────────
        private Button BotonNavegacion(string texto, int posicionSuperior)
        {
            var boton = new Button
            {
                Text      = texto,
                Font      = EstilosApp.FuenteCuerpo,
                ForeColor = EstilosApp.TextoSecundario,
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Width     = EstilosApp.AnchoBarraLateral,
                Height    = 40,
                Location  = new Point(0, posicionSuperior),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding   = new Padding(14, 0, 0, 0),
                Cursor    = Cursors.Hand
            };
            boton.FlatAppearance.BorderSize          = 0;
            boton.FlatAppearance.MouseOverBackColor  = EstilosApp.FondoTarjeta;
            return boton;
        }
    }
}
