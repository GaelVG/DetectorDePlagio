using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using PlagiarismDetector.Engine;
using PlagiarismDetector.Resources;

namespace PlagiarismDetector.Forms
{
    /// <summary>
    /// Panel educativo del Alfabeto Σ.
    /// Muestra los grupos de símbolos, propiedades del alfabeto y un
    /// verificador interactivo de pertenencia (s ∈ Σ ?).
    /// </summary>
    public class PanelAlfabeto : Panel
    {
        // ─── Controles interactivos ────────────────────────────────────────────
        private TextBox _cajaEntrada    = null!;
        private Label   _etiquetaResult = null!;
        private Label   _etiquetaGrupo  = null!;

        public PanelAlfabeto()
        {
            BackColor  = EstilosApp.FondoOscuro;
            Dock       = DockStyle.Fill;
            ConstruirUI();
        }

        private void ConstruirUI()
        {
            var scroll    = new Panel { Dock = DockStyle.Fill, AutoScroll = true, BackColor = EstilosApp.FondoOscuro };
            var contenedor = new Panel { Width = 800, Location = new Point(40, 30), BackColor = EstilosApp.FondoOscuro };

            int y = 0;

            // ── Título ──────────────────────────────────────────────────────
            AgregarEtiqueta(contenedor, "Alfabeto Σ (Sigma) – Teoría Formal", EstilosApp.FuenteTitulo, EstilosApp.Acento, ref y, 0);
            AgregarEtiqueta(contenedor, "Conjunto finito de símbolos que el sistema reconoce y procesa", EstilosApp.FuenteCuerpo, EstilosApp.TextoSecundario, ref y, 4);

            AgregarSeparador(contenedor, ref y, 20);

            // ── Cardinalidad ─────────────────────────────────────────────────
            AgregarEtiqueta(contenedor, $"Cardinalidad de Σ:  |Σ| = {Alfabeto.CardinalidadSigma} símbolos", EstilosApp.FuenteEncabezado, EstilosApp.AcentoClaro, ref y, 10);

            AgregarSeparador(contenedor, ref y, 20);

            // ── Grupos de símbolos ───────────────────────────────────────────
            AgregarGrupoSimbolo(contenedor, "Letras minúsculas  (a-z)", Alfabeto.LetrasMinusculas, ref y);
            AgregarGrupoSimbolo(contenedor, "Letras mayúsculas  (A-Z)", Alfabeto.LetrasMayusculas, ref y);
            AgregarGrupoSimbolo(contenedor, "Dígitos  (0-9)",           Alfabeto.Digitos,           ref y);
            AgregarGrupoSimbolo(contenedor, "Caracteres especiales",    Alfabeto.CaracteresEspeciales, ref y);
            AgregarEtiqueta(contenedor, "Espacio → ' '", EstilosApp.FuenteMono, EstilosApp.TextoSecundario, ref y, 8);

            AgregarSeparador(contenedor, ref y, 20);

            // ── Propiedades del alfabeto ─────────────────────────────────────
            AgregarEtiqueta(contenedor, "Propiedades del Alfabeto", EstilosApp.FuenteEncabezado, EstilosApp.TextoPrimario, ref y, 10);
            string[] propiedades = {
                "1. Puede ser vacío.",
                "2. Debe estar definido explícitamente.",
                "3. Debe ser finito.",
                "4. Todos sus elementos deben ser símbolos."
            };
            foreach (var p in propiedades)
                AgregarEtiqueta(contenedor, p, EstilosApp.FuenteCuerpo, EstilosApp.TextoSecundario, ref y, 0);

            AgregarSeparador(contenedor, ref y, 20);

            // ── Verificador de pertenencia interactivo ───────────────────────
            AgregarEtiqueta(contenedor, "Prueba de pertenencia  s ∈ Σ ?", EstilosApp.FuenteEncabezado, EstilosApp.TextoPrimario, ref y, 10);
            AgregarEtiqueta(contenedor, "Escribe un carácter para comprobar si pertenece al alfabeto:", EstilosApp.FuenteCuerpo, EstilosApp.TextoSecundario, ref y, 4);

            _cajaEntrada = new TextBox
            {
                MaxLength   = 1,
                Font        = new Font("Consolas", 14f),
                BackColor   = EstilosApp.FondoTarjeta,
                ForeColor   = EstilosApp.TextoPrimario,
                Width       = 60,
                Location    = new Point(0, y),
                BorderStyle = BorderStyle.FixedSingle,
                TextAlign   = HorizontalAlignment.Center
            };
            _cajaEntrada.TextChanged += AlCambiarEntrada;
            contenedor.Controls.Add(_cajaEntrada);
            y += 44;

            _etiquetaResult = new Label
            {
                Text = "", Font = EstilosApp.FuenteEncabezado,
                ForeColor = EstilosApp.TextoPrimario, BackColor = Color.Transparent,
                Location = new Point(0, y), AutoSize = true
            };
            contenedor.Controls.Add(_etiquetaResult);
            y += 30;

            _etiquetaGrupo = new Label
            {
                Text = "", Font = EstilosApp.FuenteCuerpo,
                ForeColor = EstilosApp.TextoSecundario, BackColor = Color.Transparent,
                Location = new Point(0, y), AutoSize = true
            };
            contenedor.Controls.Add(_etiquetaGrupo);
            y += 30;

            AgregarSeparador(contenedor, ref y, 20);

            // ── Sub-Alfabeto ─────────────────────────────────────────────────
            AgregarEtiqueta(contenedor, "Sub-Alfabeto  S1 ⊆ S2", EstilosApp.FuenteEncabezado, EstilosApp.TextoPrimario, ref y, 10);
            AgregarEtiqueta(contenedor, "S1 = {a, b, c}  ⊆  S2 = Σ  →  verdadero", EstilosApp.FuenteMono, EstilosApp.AcentoClaro, ref y, 4);
            AgregarEtiqueta(contenedor, "S1 = {a, b, c}  ⊆  S2 = {a, b}  →  falso", EstilosApp.FuenteMono, EstilosApp.AcentoClaro, ref y, 0);

            AgregarSeparador(contenedor, ref y, 20);

            // ── Símbolo Nulo ─────────────────────────────────────────────────
            AgregarEtiqueta(contenedor, "Símbolo Nulo  ε (Épsilon)", EstilosApp.FuenteEncabezado, EstilosApp.TextoPrimario, ref y, 10);
            AgregarEtiqueta(contenedor,
                "Representa la ausencia de símbolo. Es el caso base en procesos\n" +
                "recursivos del compilador y la construcción de palabras vacías.",
                EstilosApp.FuenteCuerpo, EstilosApp.TextoSecundario, ref y, 4);

            contenedor.Height = y + 40;
            scroll.Controls.Add(contenedor);
            Controls.Add(scroll);
        }

        // ─── Evento: verificar pertenencia ────────────────────────────────────
        private void AlCambiarEntrada(object? remitente, EventArgs e)
        {
            string texto = _cajaEntrada.Text;
            if (texto.Length == 0)
            {
                _etiquetaResult.Text = "";
                _etiquetaGrupo.Text  = "";
                return;
            }

            char c         = texto[0];
            bool pertenece = Alfabeto.PerteneceSigma(c);
            string grupo   = Alfabeto.ObtenerGrupoSimbolo(c);

            _etiquetaResult.ForeColor = pertenece ? EstilosApp.Exito : EstilosApp.Peligro;
            _etiquetaResult.Text = pertenece
                ? $"'{c}'  ∈  Σ  →  verdadero ✓"
                : $"'{c}'  ∉  Σ  →  falso ✗";

            _etiquetaGrupo.Text = grupo;
        }

        // ─── Métodos auxiliares de construcción ───────────────────────────────

        private void AgregarEtiqueta(Panel padre, string texto, Font fuente, Color color, ref int y, int relleno)
        {
            y += relleno;
            var etiqueta = new Label
            {
                Text = texto, Font = fuente, ForeColor = color,
                BackColor = Color.Transparent, Location = new Point(0, y), AutoSize = true
            };
            padre.Controls.Add(etiqueta);
            y += etiqueta.PreferredHeight + 6;
        }

        private void AgregarGrupoSimbolo(Panel padre, string titulo, HashSet<char> simbolos, ref int y)
        {
            AgregarEtiqueta(padre, titulo, EstilosApp.FuenteCuerpo, EstilosApp.TextoPrimario, ref y, 10);
            string lista = string.Join("  ", simbolos.OrderBy(c => c));
            AgregarEtiqueta(padre, lista, EstilosApp.FuenteMono, EstilosApp.TextoSecundario, ref y, 2);
        }

        private void AgregarSeparador(Panel padre, ref int y, int relleno)
        {
            y += relleno;
            padre.Controls.Add(new Panel
            {
                Height = 1, Width = 750, Location = new Point(0, y),
                BackColor = EstilosApp.Borde
            });
            y += 1 + relleno;
        }
    }
}
