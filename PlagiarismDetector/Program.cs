using PlagiarismDetector.Forms;

namespace PlagiarismDetector;

/// <summary>
/// Punto de entrada de la aplicación.
/// Inicializa la configuración de Windows Forms y lanza el formulario principal.
/// </summary>
static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new FormularioPrincipal());
    }
}