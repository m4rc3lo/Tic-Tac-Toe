using TicTacToe.Presentation;
using TicTacToe.Presentation.Navigation;
using Xunit;

namespace TicTacToe.Tests.Presentation.Navigation;

public class ConsoleAutomaticModeControlTests
{
    [Fact]
    public void wait_for_turn_should_pause_and_resume()
    {
        Queue<ConsoleKeyInfo> keys = new(
        [
            create_key(ConsoleKey.Spacebar, ' '),
            create_key(ConsoleKey.Spacebar, ' ')
        ]);
        ConsoleAutomaticModeControl control = new(
            new StringWriter(),
            new ImmediateDelayService(),
            () => keys.Count > 0,
            () => keys.Dequeue());

        AutomaticControlDecision decision =
            control.wait_for_turn();

        Assert.Equal(
            AutomaticControlDecision.Continue,
            decision);
        Assert.Empty(keys);
    }

    [Fact]
    public void wait_for_turn_should_cancel_from_pause()
    {
        Queue<ConsoleKeyInfo> keys = new(
        [
            create_key(ConsoleKey.Spacebar, ' '),
            create_key(ConsoleKey.Escape, '\u001b')
        ]);
        ConsoleAutomaticModeControl control = new(
            new StringWriter(),
            new ImmediateDelayService(),
            () => keys.Count > 0,
            () => keys.Dequeue());

        AutomaticControlDecision decision =
            control.wait_for_turn();

        Assert.Equal(
            AutomaticControlDecision.Cancel,
            decision);
    }


    [Fact]
    public void wait_for_turn_should_not_read_keys_when_input_is_not_interactive()
    {
        bool key_available_called = false;
        ConsoleAutomaticModeControl control = new(
            new StringWriter(),
            new ImmediateDelayService(),
            () =>
            {
                key_available_called = true;
                return true;
            },
            () => throw new InvalidOperationException(),
            supports_interactive_input: false);

        AutomaticControlDecision decision =
            control.wait_for_turn();

        Assert.Equal(
            AutomaticControlDecision.Continue,
            decision);
        Assert.False(key_available_called);
    }

    [Fact]
    public void wait_for_turn_should_fallback_when_key_query_is_unavailable()
    {
        ConsoleAutomaticModeControl control = new(
            new StringWriter(),
            new ImmediateDelayService(),
            () => throw new InvalidOperationException(),
            () => throw new InvalidOperationException());

        AutomaticControlDecision decision =
            control.wait_for_turn();

        Assert.Equal(
            AutomaticControlDecision.Continue,
            decision);
    }

    private static ConsoleKeyInfo create_key(
        ConsoleKey key,
        char character)
    {
        return new ConsoleKeyInfo(
            character,
            key,
            shift: false,
            alt: false,
            control: false);
    }
}
