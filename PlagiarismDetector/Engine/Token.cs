namespace PlagiarismDetector.Engine
{
    /// <summary>
    /// Representa un token individual producido por el Analizador Léxico.
    /// Un token es la unidad significativa mínima extraída del texto fuente.
    /// </summary>
    public class Token
    {
        /// <summary>Categoría del token según el Alfabeto Σ.</summary>
        public TipoToken Tipo { get; }

        /// <summary>Valor léxico (texto original del token).</summary>
        public string Valor { get; }

        /// <summary>Posición del token en el texto fuente (desplazamiento de carácter).</summary>
        public int Posicion { get; }

        public Token(TipoToken tipo, string valor, int posicion)
        {
            Tipo     = tipo;
            Valor    = valor;
            Posicion = posicion;
        }

        public override string ToString()
            => $"[{Tipo}] \"{Valor}\" @{Posicion}";
    }
}
