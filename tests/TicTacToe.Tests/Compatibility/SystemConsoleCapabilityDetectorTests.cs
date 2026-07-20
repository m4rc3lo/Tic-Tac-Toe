
using System.Text;
using TicTacToe.Compatibility;
using Xunit;

namespace TicTacToe.Tests.Compatibility;

public class SystemConsoleCapabilityDetectorTests
{
    [Fact]
    public void detect_should_separate_platform_from_terminal_capabilities()
    {
        SystemConsoleCapabilityDetector detector = new(
            () => false,
            () => false,
            () => "xterm-256color",
            () => null,
            () => new UTF8Encoding(false));

        ConsoleCapabilities capabilities =
            detector.detect(RuntimePlatform.Linux);

        Assert.True(capabilities.SupportsInteractiveInput);
        Assert.True(capabilities.SupportsUnicode);
        Assert.True(capabilities.SupportsAnsi);
        Assert.True(capabilities.SupportsTerminalBell);
        Assert.False(capabilities.SupportsConsoleBeep);
    }

    [Fact]
    public void detect_should_disable_terminal_features_for_dumb_terminal()
    {
        SystemConsoleCapabilityDetector detector = new(
            () => true,
            () => false,
            () => "dumb",
            () => null,
            () => Encoding.ASCII);

        ConsoleCapabilities capabilities =
            detector.detect(RuntimePlatform.Windows);

        Assert.False(capabilities.SupportsInteractiveInput);
        Assert.False(capabilities.SupportsUnicode);
        Assert.False(capabilities.SupportsAnsi);
        Assert.False(capabilities.SupportsClearScreen);
        Assert.False(capabilities.SupportsConsoleBeep);
    }

    [Fact]
    public void detect_should_honor_no_color()
    {
        SystemConsoleCapabilityDetector detector = new(
            () => false,
            () => false,
            () => "xterm",
            () => "1",
            () => new UTF8Encoding(false));

        ConsoleCapabilities capabilities =
            detector.detect(RuntimePlatform.Linux);

        Assert.False(capabilities.SupportsAnsi);
        Assert.True(capabilities.SupportsUnicode);
    }
}
