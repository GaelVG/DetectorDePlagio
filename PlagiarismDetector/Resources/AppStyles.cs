using System.Drawing;

namespace PlagiarismDetector.Resources
{
    /// <summary>
    /// Estilos centralizados de la aplicación: colores, fuentes y tamaños.
    /// Paleta de tema oscuro azul/índigo moderno.
    /// </summary>
    public static class EstilosApp
    {
        // ─── Paleta de colores ─────────────────────────────────────────────────
        public static readonly Color FondoOscuro    = Color.FromArgb(18,  20,  40);
        public static readonly Color FondoPanel     = Color.FromArgb(28,  32,  58);
        public static readonly Color FondoTarjeta   = Color.FromArgb(38,  44,  76);
        public static readonly Color BarraLateral   = Color.FromArgb(22,  26,  48);
        public static readonly Color Acento         = Color.FromArgb(99, 102, 241);
        public static readonly Color AcentoHover    = Color.FromArgb(129, 140, 248);
        public static readonly Color AcentoClaro    = Color.FromArgb(199, 210, 254);
        public static readonly Color Exito          = Color.FromArgb( 34, 197,  94);
        public static readonly Color Advertencia    = Color.FromArgb(234, 179,   8);
        public static readonly Color Peligro        = Color.FromArgb(239,  68,  68);
        public static readonly Color TextoPrimario  = Color.FromArgb(241, 245, 249);
        public static readonly Color TextoSecundario = Color.FromArgb(148, 163, 184);
        public static readonly Color Borde          = Color.FromArgb( 51,  65, 115);

        // ─── Fuentes ───────────────────────────────────────────────────────────
        public static readonly Font FuenteTitulo      = new Font("Segoe UI", 16f, FontStyle.Bold);
        public static readonly Font FuenteEncabezado  = new Font("Segoe UI", 12f, FontStyle.Bold);
        public static readonly Font FuenteCuerpo      = new Font("Segoe UI",  9f, FontStyle.Regular);
        public static readonly Font FuentePequena     = new Font("Segoe UI",  8f, FontStyle.Regular);
        public static readonly Font FuenteMono        = new Font("Consolas",  9f, FontStyle.Regular);

        // ─── Dimensiones ───────────────────────────────────────────────────────
        public const int AnchoBarraLateral = 200;
        public const int Radio             = 8;

        // ─── Color según puntuación de plagio ──────────────────────────────────
        public static Color ColorPuntuacion(double porcentaje)
        {
            if (porcentaje >= 80) return Peligro;
            if (porcentaje >= 50) return Advertencia;
            if (porcentaje >= 25) return Color.FromArgb(251, 146, 60); // naranja
            return Exito;
        }
    }
}
