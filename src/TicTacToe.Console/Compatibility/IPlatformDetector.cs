
namespace TicTacToe.Compatibility;

/// <summary>
/// Detecta somente a plataforma, sem inferir capacidades do terminal.
/// </summary>
public interface IPlatformDetector
{
    RuntimePlatform detect();
}
