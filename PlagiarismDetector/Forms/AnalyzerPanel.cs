using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using PlagiarismDetector.Engine;
using PlagiarismDetector.Resources;

namespace PlagiarismDetector.Forms
{
    /// <summary>
    /// Panel principal de análisis de plagio.
    /// Permite cargar o pegar dos documentos, analizarlos y ver los resultados
    /// de similitud con métricas detalladas y lista de coincidencias.
    /// </summary>
    public class PanelAnalizador : Panel
    {
        // ─── Documentos procesados ─────────────────────────────────────────────
        private DocumentoProcesado? _documentoA, _documentoB;

        // ─── Controles de carga ────────────────────────────────────────────────
        private Label   _etiquetaA = null!,    _etiquetaB = null!;
        private Button  _btnCargarA = null!,   _btnCargarB = null!;
        private TextBox _textoA = null!,       _textoB = null!;
        private Button  _btnAnalizar = null!;

        // ─── Controles de resultados ───────────────────────────────────────────
        private Panel   _tarjetaResultado  = null!;
        private Label   _etiqPuntuacion    = null!;
        private Label   _etiqVeredicto     = null!;
        private Label   _etiqCoseno        = null!;
        private Label   _etiqJaccard       = null!;
        private Label   _etiqBigramas      = null!;
        private Label   _etiqTrigramas     = null!;
        private ListBox _listaPalabrasComunes  = null!;
        private ListBox _listaBigramasComunes  = null!;
        private ListBox _listaTokensA          = null!;
        private ListBox _listaTokensB          = null!;

        public PanelAnalizador()
        {
            BackColor  = EstilosApp.FondoOscuro;
            Dock       = DockStyle.Fill;
            AutoScroll = true;
            ConstruirUI();
        }

        // ─────────────────────────────────────────────────────────────────────
        private void ConstruirUI()
        {
            int margen = 20;

            // ── Título de la página ─────────────────────────────────────────
            Controls.Add(new Label
            {
                Text = "Analizador de Plagio", Font = EstilosApp.FuenteTitulo,
                ForeColor = EstilosApp.Acento, BackColor = Color.Transparent,
                AutoSize = true, Location = new Point(margen, margen)
            });

            Controls.Add(new Label
            {
                Text = "Carga dos documentos y presiona «Analizar» para detectar similitudes.",
                Font = EstilosApp.FuenteCuerpo, ForeColor = EstilosApp.TextoSecundario,
                BackColor = Color.Transparent, AutoSize = true,
                Location = new Point(margen, margen + 36)
            });

            // ── Tarjeta Documento A ─────────────────────────────────────────
            var tarjetaA = CrearTarjetaDocumento("Documento A",
                out _etiquetaA, out _btnCargarA, out _textoA);
            tarjetaA.Location = new Point(margen, 90);
            Controls.Add(tarjetaA);

            // ── Tarjeta Documento B ─────────────────────────────────────────
            var tarjetaB = CrearTarjetaDocumento("Documento B",
                out _etiquetaB, out _btnCargarB, out _textoB);
            tarjetaB.Location = new Point(margen + tarjetaA.Width + margen, 90);
            Controls.Add(tarjetaB);

            _btnCargarA.Tag    = "A";
            _btnCargarB.Tag    = "B";
            _btnCargarA.Click += AlCargarArchivo;
            _btnCargarB.Click += AlCargarArchivo;

            // ── Botón Analizar ──────────────────────────────────────────────
            _btnAnalizar = BotonEstilizado("🔍  Analizar Documentos", EstilosApp.Acento);
            _btnAnalizar.Width    = 260;
            _btnAnalizar.Height   = 46;
            _btnAnalizar.Location = new Point(margen, 90 + tarjetaA.Height + 16);
            _btnAnalizar.Click   += AlAnalizar;
            Controls.Add(_btnAnalizar);

            // ── Tarjeta de resultados (oculta hasta analizar) ───────────────
            _tarjetaResultado = new Panel
            {
                Location    = new Point(margen, 90 + tarjetaA.Height + 74),
                Width       = tarjetaA.Width * 2 + margen,
                Height      = 520,
                BackColor   = EstilosApp.FondoPanel,
                Visible     = false
            };
            ConstruirTarjetaResultado(_tarjetaResultado);
            Controls.Add(_tarjetaResultado);
        }

        // ─── Crea una tarjeta de carga de documento ────────────────────────────
        private Panel CrearTarjetaDocumento(string encabezado,
            out Label etiquetaRuta, out Button botonCargar, out TextBox cajaVista)
        {
            var tarjeta = new Panel
            {
                Width     = 420,
                Height    = 310,
                BackColor = EstilosApp.FondoTarjeta,
                Padding   = new Padding(14)
            };

            tarjeta.Controls.Add(new Label
            {
                Text = encabezado, Font = EstilosApp.FuenteEncabezado,
                ForeColor = EstilosApp.AcentoClaro, BackColor = Color.Transparent,
                AutoSize = true, Location = new Point(14, 14)
            });

            etiquetaRuta = new Label
            {
                Text = "Sin archivo cargado", Font = EstilosApp.FuentePequena,
                ForeColor = EstilosApp.TextoSecundario, BackColor = Color.Transparent,
                AutoSize = false, Width = 390, Location = new Point(14, 44)
            };
            tarjeta.Controls.Add(etiquetaRuta);

            botonCargar = BotonEstilizado("📂  Cargar archivo…", EstilosApp.FondoPanel);
            botonCargar.Width    = 180;
            botonCargar.Height   = 34;
            botonCargar.Location = new Point(14, 68);
            tarjeta.Controls.Add(botonCargar);

            tarjeta.Controls.Add(new Label
            {
                Text = "— o pega texto —", Font = EstilosApp.FuentePequena,
                ForeColor = EstilosApp.TextoSecundario, BackColor = Color.Transparent,
                AutoSize = true, Location = new Point(202, 76)
            });

            cajaVista = new TextBox
            {
                Multiline   = true,
                ScrollBars  = ScrollBars.Vertical,
                Font        = EstilosApp.FuenteMono,
                BackColor   = EstilosApp.FondoOscuro,
                ForeColor   = EstilosApp.TextoPrimario,
                BorderStyle = BorderStyle.FixedSingle,
                Location    = new Point(14, 108),
                Width       = 390,
                Height      = 188
            };
            tarjeta.Controls.Add(cajaVista);

            return tarjeta;
        }

        // ─── Construye la tarjeta de resultados ────────────────────────────────
        private void ConstruirTarjetaResultado(Panel tarjeta)
        {
            int m = 16;

            // ── Puntuación y veredicto ──────────────────────────────────────
            _etiqPuntuacion = new Label
            {
                Text = "–– %", Font = new Font("Segoe UI", 36f, FontStyle.Bold),
                ForeColor = EstilosApp.Acento, BackColor = Color.Transparent,
                AutoSize = true, Location = new Point(m, m)
            };
            tarjeta.Controls.Add(_etiqPuntuacion);

            _etiqVeredicto = new Label
            {
                Text = "", Font = EstilosApp.FuenteEncabezado,
                ForeColor = EstilosApp.TextoPrimario, BackColor = Color.Transparent,
                AutoSize = true, Location = new Point(m + 140, m + 10)
            };
            tarjeta.Controls.Add(_etiqVeredicto);

            // ── Métricas individuales ───────────────────────────────────────
            _etiqCoseno    = EtiquetaMetrica("Coseno:",     new Point(m,       90), tarjeta);
            _etiqJaccard   = EtiquetaMetrica("Jaccard:",    new Point(m,      118), tarjeta);
            _etiqBigramas  = EtiquetaMetrica("Bigramas:",   new Point(m + 220, 90), tarjeta);
            _etiqTrigramas = EtiquetaMetrica("Trigramas:",  new Point(m + 220,118), tarjeta);

            tarjeta.Controls.Add(new Panel
            {
                Height = 1, Width = tarjeta.Width - m * 2,
                Location = new Point(m, 150), BackColor = EstilosApp.Borde
            });

            // ── Palabras comunes ────────────────────────────────────────────
            AgregarSubtitulo(tarjeta, "Palabras en común", new Point(m, 162));
            _listaPalabrasComunes = ListaEstilizada(new Point(m, 186), 200, 140, tarjeta);

            // ── Bigramas comunes ────────────────────────────────────────────
            AgregarSubtitulo(tarjeta, "Frases comunes (bigramas)", new Point(m + 220, 162));
            _listaBigramasComunes = ListaEstilizada(new Point(m + 220, 186), 340, 140, tarjeta);

            tarjeta.Controls.Add(new Panel
            {
                Height = 1, Width = tarjeta.Width - m * 2,
                Location = new Point(m, 336), BackColor = EstilosApp.Borde
            });

            // ── Tokens por documento ────────────────────────────────────────
            AgregarSubtitulo(tarjeta, "Tokens – Documento A", new Point(m, 348));
            _listaTokensA = ListaEstilizada(new Point(m, 372), 200, 120, tarjeta);

            AgregarSubtitulo(tarjeta, "Tokens – Documento B", new Point(m + 220, 348));
            _listaTokensB = ListaEstilizada(new Point(m + 220, 372), 200, 120, tarjeta);
        }

        // ─── Eventos ───────────────────────────────────────────────────────────

        private void AlCargarArchivo(object? remitente, EventArgs e)
        {
            string etiquetaSlot = (remitente as Button)?.Tag?.ToString() ?? "A";

            using var dialogo = new OpenFileDialog
            {
                Title  = "Seleccionar archivo de texto",
                Filter = "Archivos de texto (*.txt)|*.txt|Todos los archivos (*.*)|*.*"
            };
            if (dialogo.ShowDialog() != DialogResult.OK) return;

            try
            {
                var doc = ProcesadorDocumentos.Procesar(dialogo.FileName);
                string vistaPrevia = doc.TextoOriginal.Length > 3000
                    ? doc.TextoOriginal.Substring(0, 3000) + "…"
                    : doc.TextoOriginal;

                if (etiquetaSlot == "A")
                {
                    _documentoA         = doc;
                    _etiquetaA.Text     = Path.GetFileName(dialogo.FileName);
                    _textoA.Text    = vistaPrevia;
                }
                else
                {
                    _documentoB         = doc;
                    _etiquetaB.Text     = Path.GetFileName(dialogo.FileName);
                    _textoB.Text    = vistaPrevia;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar archivo:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AlAnalizar(object? remitente, EventArgs e)
        {
            // Verificar que ambas cajas tengan contenido
            if (string.IsNullOrWhiteSpace(_textoA.Text) || string.IsNullOrWhiteSpace(_textoB.Text))
            {
                MessageBox.Show("Por favor carga o pega ambos documentos antes de analizar.",
                    "Faltan documentos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Siempre re-procesar desde el contenido actual de las cajas de texto.
            // Esto permite modificar o cambiar el texto y analizar de nuevo sin problemas.
            string nombreA = (_etiquetaA.Text == "Sin archivo cargado" || string.IsNullOrWhiteSpace(_etiquetaA.Text))
                ? "Documento A" : _etiquetaA.Text;
            string nombreB = (_etiquetaB.Text == "Sin archivo cargado" || string.IsNullOrWhiteSpace(_etiquetaB.Text))
                ? "Documento B" : _etiquetaB.Text;

            _documentoA = ProcesadorDocumentos.ProcesarTexto(_textoA.Text, nombreA);
            _documentoB = ProcesadorDocumentos.ProcesarTexto(_textoB.Text, nombreB);

            ReporteSimilitud reporte = MotorSimilitud.Comparar(_documentoA, _documentoB);
            MostrarResultados(reporte);
        }

        private void MostrarResultados(ReporteSimilitud reporte)
        {
            Color colorPuntuacion = EstilosApp.ColorPuntuacion(reporte.PuntuacionCombinada);

            _etiqPuntuacion.Text      = $"{reporte.PuntuacionCombinada:F1} %";
            _etiqPuntuacion.ForeColor = colorPuntuacion;
            _etiqVeredicto.Text       = reporte.Veredicto;
            _etiqVeredicto.ForeColor  = colorPuntuacion;

            _etiqCoseno.Text    = $"Coseno:     {reporte.SimilitudCoseno    * 100:F1} %";
            _etiqJaccard.Text   = $"Jaccard:    {reporte.SimilitudJaccard   * 100:F1} %";
            _etiqBigramas.Text  = $"Bigramas:   {reporte.SimilitudBigramas  * 100:F1} %";
            _etiqTrigramas.Text = $"Trigramas:  {reporte.SimilitudTrigramas * 100:F1} %";

            _listaPalabrasComunes.Items.Clear();
            foreach (var p in reporte.PalabrasComunes.Take(100))
                _listaPalabrasComunes.Items.Add(p);

            _listaBigramasComunes.Items.Clear();
            foreach (var b in reporte.BigramasComunes.Take(50))
                _listaBigramasComunes.Items.Add(b);

            // ── Estadísticas de tokens por documento ────────────────────────
            RellenarListaTokens(_listaTokensA, _documentoA);
            RellenarListaTokens(_listaTokensB, _documentoB);

            _tarjetaResultado.Visible = true;
        }

        private void RellenarListaTokens(ListBox lista, DocumentoProcesado? doc)
        {
            lista.Items.Clear();
            if (doc == null) return;

            var agrupados = doc.Tokens
                .Where(t => t.Tipo != TipoToken.EspacioBlanco)
                .GroupBy(t => t.Tipo)
                .OrderByDescending(g => g.Count());

            foreach (var grupo in agrupados)
                lista.Items.Add($"{grupo.Key}: {grupo.Count()} tokens");
        }

        // ─── Métodos auxiliares de construcción ───────────────────────────────

        private Button BotonEstilizado(string texto, Color fondo)
        {
            var btn = new Button
            {
                Text      = texto,
                Font      = EstilosApp.FuenteCuerpo,
                BackColor = fondo,
                ForeColor = EstilosApp.TextoPrimario,
                FlatStyle = FlatStyle.Flat,
                Cursor    = Cursors.Hand
            };
            btn.FlatAppearance.BorderColor         = EstilosApp.Borde;
            btn.FlatAppearance.MouseOverBackColor  = EstilosApp.AcentoHover;
            return btn;
        }

        private Label EtiquetaMetrica(string prefijo, Point ubicacion, Panel padre)
        {
            var etiqueta = new Label
            {
                Text = prefijo, Font = EstilosApp.FuenteCuerpo,
                ForeColor = EstilosApp.TextoSecundario, BackColor = Color.Transparent,
                AutoSize = true, Location = ubicacion
            };
            padre.Controls.Add(etiqueta);
            return etiqueta;
        }

        private void AgregarSubtitulo(Panel padre, string texto, Point ubicacion)
        {
            padre.Controls.Add(new Label
            {
                Text = texto, Font = EstilosApp.FuenteCuerpo,
                ForeColor = EstilosApp.AcentoClaro, BackColor = Color.Transparent,
                AutoSize = true, Location = ubicacion
            });
        }

        private ListBox ListaEstilizada(Point ubicacion, int ancho, int alto, Panel padre)
        {
            var lista = new ListBox
            {
                Location            = ubicacion,
                Width               = ancho,
                Height              = alto,
                BackColor           = EstilosApp.FondoOscuro,
                ForeColor           = EstilosApp.TextoPrimario,
                Font                = EstilosApp.FuenteMono,
                BorderStyle         = BorderStyle.FixedSingle,
                ScrollAlwaysVisible = false
            };
            padre.Controls.Add(lista);
            return lista;
        }
    }
}
