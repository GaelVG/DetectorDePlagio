using System.Collections.Generic;

namespace PlagiarismDetector.Engine
{
    /// <summary>
    /// Analizador Léxico.
    /// Lee el texto de entrada carácter por carácter y agrupa los caracteres
    /// en objetos Token, usando el Alfabeto Σ para validar cada símbolo.
    /// </summary>
    public class AnalizadorLexico
    {
        private readonly string _entrada; // texto fuente a analizar
        private int _pos;                 // posición actual en la cadena

        /// <param name="entrada">Texto a analizar léxicamente.</param>
        public AnalizadorLexico(string entrada)
        {
            _entrada = entrada ?? string.Empty;
            _pos     = 0;
        }

        /// <summary>Tokeniza la entrada completa y devuelve el flujo de tokens.</summary>
        public List<Token> Tokenizar()
        {
            var tokens = new List<Token>();
            _pos = 0;

            while (_pos < _entrada.Length)
            {
                char actual = _entrada[_pos];

                // ── Cadenas de texto: "..." ───────────────────────────────────
                if (actual == '"')
                {
                    tokens.Add(LeerCadenaTexto());
                    continue;
                }

                // ── Símbolos booleanos: #t / #f ───────────────────────────────
                if (actual == '#' && _pos + 1 < _entrada.Length &&
                    (_entrada[_pos + 1] == 't' || _entrada[_pos + 1] == 'f'))
                {
                    tokens.Add(new Token(TipoToken.Booleano, _entrada.Substring(_pos, 2), _pos));
                    _pos += 2;
                    continue;
                }

                // ── Letras → PALABRA ──────────────────────────────────────────
                if (Alfabeto.LetrasMinusculas.Contains(actual) ||
                    Alfabeto.LetrasMayusculas.Contains(actual) ||
                    actual == '_')
                {
                    tokens.Add(LeerPalabra());
                    continue;
                }

                // ── Dígitos → NUMERO ──────────────────────────────────────────
                if (Alfabeto.Digitos.Contains(actual))
                {
                    tokens.Add(LeerNumero());
                    continue;
                }

                // ── Espacios en blanco ────────────────────────────────────────
                if (Alfabeto.EspaciosBlanco.Contains(actual))
                {
                    tokens.Add(LeerEspacioBlanco());
                    continue;
                }

                // ── Caracteres especiales del Σ ───────────────────────────────
                if (Alfabeto.CaracteresEspeciales.Contains(actual))
                {
                    tokens.Add(new Token(TipoToken.CaracterEspecial, actual.ToString(), _pos));
                    _pos++;
                    continue;
                }

                // ── Desconocido (no pertenece a Σ) ───────────────────────────
                tokens.Add(new Token(TipoToken.Desconocido, actual.ToString(), _pos));
                _pos++;
            }

            return tokens;
        }

        // ─── Métodos auxiliares de lectura ─────────────────────────────────────

        private Token LeerPalabra()
        {
            int inicio = _pos;
            while (_pos < _entrada.Length &&
                   (Alfabeto.LetrasMinusculas.Contains(_entrada[_pos]) ||
                    Alfabeto.LetrasMayusculas.Contains(_entrada[_pos]) ||
                    _entrada[_pos] == '_'))
            {
                _pos++;
            }
            return new Token(TipoToken.Palabra, _entrada.Substring(inicio, _pos - inicio), inicio);
        }

        private Token LeerNumero()
        {
            int inicio = _pos;
            while (_pos < _entrada.Length && Alfabeto.Digitos.Contains(_entrada[_pos]))
                _pos++;
            return new Token(TipoToken.Numero, _entrada.Substring(inicio, _pos - inicio), inicio);
        }

        private Token LeerEspacioBlanco()
        {
            int inicio = _pos;
            while (_pos < _entrada.Length && Alfabeto.EspaciosBlanco.Contains(_entrada[_pos]))
                _pos++;
            return new Token(TipoToken.EspacioBlanco, _entrada.Substring(inicio, _pos - inicio), inicio);
        }

        private Token LeerCadenaTexto()
        {
            int inicio = _pos;
            _pos++; // saltar comilla de apertura "
            while (_pos < _entrada.Length && _entrada[_pos] != '"')
                _pos++;
            if (_pos < _entrada.Length) _pos++; // saltar comilla de cierre "
            return new Token(TipoToken.CadenaTexto, _entrada.Substring(inicio, _pos - inicio), inicio);
        }
    }
}
