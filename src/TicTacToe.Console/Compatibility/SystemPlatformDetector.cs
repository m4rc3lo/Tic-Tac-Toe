
namespace TicTacToe.Compatibility;

/// <summary>
/// Detecta a plataforma por APIs do runtime .NET.
/// </summary>
public sealed class SystemPlatformDetector : IPlatformDetector
{
    public RuntimePlatform detect()
    {
        if (OperatingSystem.IsWindows())
        {
            return RuntimePlatform.Windows;
        }

        if (OperatingSystem.IsLinux())
        {
            return RuntimePlatform.Linux;
        }

        if (OperatingSystem.IsMacOS())
        {
            return RuntimePlatform.MacOS;
        }

        return RuntimePlatform.Other;
    }
}
