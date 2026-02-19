namespace PlagiarismDetector.Engine
{
    /// <summary>
    /// Categorías de tokens producidos por el Analizador Léxico.
    /// Cada categoría corresponde a un grupo del Alfabeto Σ.
    /// </summary>
    public enum TipoToken
    {
        Palabra,           // Secuencia de letras (a-z, A-Z)
        Numero,            // Secuencia de dígitos (0-9)
        CaracterEspecial,  // + - * / = ( ) { } ; , _
        EspacioBlanco,     // Espacio, tabulación, salto de línea
        CadenaTexto,       // "texto entre comillas"
        Booleano,          // #t o #f
        Desconocido        // Carácter que no pertenece al Σ
    }
}
