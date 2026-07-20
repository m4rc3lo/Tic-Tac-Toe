
namespace TicTacToe.Compatibility;

/// <summary>
/// Detecta capacidades reais do terminal e dos fluxos atuais.
/// </summary>
public interface IConsoleCapabilityDetector
{
    ConsoleCapabilities detect(
        RuntimePlatform platform);
}
