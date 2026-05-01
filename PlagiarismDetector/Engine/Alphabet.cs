using System.Collections.Generic;
using System.Linq;

namespace PlagiarismDetector.Engine
{
    /// <summary>
    /// Representa el Alfabeto formal Σ (Sigma) de la teoría de compiladores.
    /// Un alfabeto es un conjunto finito y explícitamente definido de símbolos.
    /// </summary>
    public class Alfabeto
    {
        // ─── Grupos de símbolos del Alfabeto Σ ────────────────────────────────
        public static readonly HashSet<char> LetrasMinusculas =
            new HashSet<char>("abcdefghijklmnopqrstuvwxyzáéíóúüñ");

        public static readonly HashSet<char> LetrasMayusculas =
            new HashSet<char>("ABCDEFGHIJKLMNOPQRSTUVWXYZÁÉÍÓÚÜÑ");

        public static readonly HashSet<char> Digitos =
            new HashSet<char>("0123456789");

        public static readonly HashSet<char> CaracteresEspeciales =
            new HashSet<char>("+-*/=(){};,_.!?:\"'¿¡");

        public static readonly HashSet<char> EspaciosBlanco =
            new HashSet<char>(new[] { ' ', '\t', '\n', '\r' });

        /// <summary>El alfabeto completo Σ (unión de todos los grupos).</summary>
        public static readonly HashSet<char> Sigma;

        /// <summary>Alfabeto vacío ().</summary>
        public static readonly HashSet<char> AlfabetoVacio = new HashSet<char>();

        /// <summary>Símbolo nulo ε (Épsilon): representa la ausencia de símbolo.</summary>
        public const char Epsilon = '\0';

        static Alfabeto()
        {
            Sigma = new HashSet<char>();
            foreach (var c in LetrasMinusculas)       Sigma.Add(c);
            foreach (var c in LetrasMayusculas)       Sigma.Add(c);
            foreach (var c in Digitos)                Sigma.Add(c);
            foreach (var c in CaracteresEspeciales)   Sigma.Add(c);
            Sigma.Add(' '); // el espacio pertenece a Σ
        }

        // ─── Pertenencia: s ∈ Σ ? ─────────────────────────────────────────────
        public static bool Pertenece(char simbolo, HashSet<char> alfabeto)
            => alfabeto.Contains(simbolo);

        public static bool PerteneceSigma(char simbolo)
            => Sigma.Contains(simbolo);

        // ─── Cardinalidad: |Σ| ────────────────────────────────────────────────
        public static int Cardinalidad(HashSet<char> alfabeto) => alfabeto.Count;
        public static int CardinalidadSigma => Sigma.Count;

        // ─── Sub-Alfabeto: S1 ⊆ S2 ? ──────────────────────────────────────────
        public static bool EsSubAlfabetoDe(HashSet<char> s1, HashSet<char> s2)
            => s1.IsSubsetOf(s2);

        // ─── Igualdad de alfabetos ─────────────────────────────────────────────
        public static bool SonIguales(HashSet<char> a1, HashSet<char> a2)
            => a1.SetEquals(a2);

        // ─── Grupo del símbolo ─────────────────────────────────────────────────
        public static string ObtenerGrupoSimbolo(char c)
        {
            if (LetrasMinusculas.Contains(c))     return "Letra minúscula";
            if (LetrasMayusculas.Contains(c))     return "Letra mayúscula";
            if (Digitos.Contains(c))              return "Dígito";
            if (CaracteresEspeciales.Contains(c)) return "Carácter especial";
            if (c == ' ')                         return "Espacio";
            return "No pertenece al alfabeto Σ";
        }

        // ─── Representación textual de Σ ──────────────────────────────────────
        public static string SigmaComoTexto()
        {
            var minusculas = new string(LetrasMinusculas.OrderBy(c => c).ToArray());
            var mayusculas = new string(LetrasMayusculas.OrderBy(c => c).ToArray());
            var numeros    = new string(Digitos.OrderBy(c => c).ToArray());
            var especiales = new string(CaracteresEspeciales.OrderBy(c => c).ToArray());
            return $"Letras minúsculas: {minusculas}\n" +
                   $"Letras mayúsculas: {mayusculas}\n" +
                   $"Dígitos: {numeros}\n" +
                   $"Especiales: {especiales}\n" +
                   $"Espacio: ' '";
        }
    }
}
