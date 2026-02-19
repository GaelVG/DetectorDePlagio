using System.Drawing;

namespace PlagiarismDetector.Resources
{
    /// <summary>
    /// Centralized design tokens: colors, fonts, sizes.
    /// Modern dark-blue theme with accent colors.
    /// </summary>
    public static class AppStyles
    {
        // ── Color palette ────────────────────────────────────────────────────
        public static readonly Color BgDark      = Color.FromArgb(18,  20,  40);   // near-black
        public static readonly Color BgPanel     = Color.FromArgb(28,  32,  58);   // dark blue
        public static readonly Color BgCard      = Color.FromArgb(38,  44,  76);   // card bg
        public static readonly Color Sidebar     = Color.FromArgb(22,  26,  48);   // sidebar
        public static readonly Color Accent      = Color.FromArgb(99, 102, 241);   // indigo
        public static readonly Color AccentHover = Color.FromArgb(129, 140, 248);
        public static readonly Color AccentLight = Color.FromArgb(199, 210, 254);
        public static readonly Color Success     = Color.FromArgb( 34, 197,  94);
        public static readonly Color Warning     = Color.FromArgb(234, 179,   8);
        public static readonly Color Danger      = Color.FromArgb(239,  68,  68);
        public static readonly Color TextPrimary = Color.FromArgb(241, 245, 249);
        public static readonly Color TextSecond  = Color.FromArgb(148, 163, 184);
        public static readonly Color Border      = Color.FromArgb( 51,  65, 115);

        // ── Fonts ────────────────────────────────────────────────────────────
        public static readonly Font FontTitle   = new Font("Segoe UI", 16f, FontStyle.Bold);
        public static readonly Font FontHeading = new Font("Segoe UI", 12f, FontStyle.Bold);
        public static readonly Font FontBody    = new Font("Segoe UI",  9f, FontStyle.Regular);
        public static readonly Font FontSmall   = new Font("Segoe UI",  8f, FontStyle.Regular);
        public static readonly Font FontMono    = new Font("Consolas",  9f, FontStyle.Regular);

        // ── Sizes ────────────────────────────────────────────────────────────
        public const int SidebarWidth = 200;
        public const int Radius       = 8;

        // ── Score → color ─────────────────────────────────────────────────
        public static Color ScoreColor(double pct)
        {
            if (pct >= 80) return Danger;
            if (pct >= 50) return Warning;
            if (pct >= 25) return Color.FromArgb(251, 146, 60); // orange
            return Success;
        }
    }
}
