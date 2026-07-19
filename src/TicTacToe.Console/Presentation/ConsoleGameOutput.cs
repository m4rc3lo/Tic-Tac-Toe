using TicTacToe.Application;
using TicTacToe.Audio;
using TicTacToe.Domain;

namespace TicTacToe.Presentation;

/// <summary>
/// Apresenta estados e resultados de uma partida em um fluxo textual.
/// </summary>
public sealed class ConsoleGameOutput : IGameOutput
{
    private readonly TextWriter writer;
    private readonly ConsoleTheme theme;
    private readonly AsciiArtCatalog art_catalog;
    private readonly IVisualFeedbackService visual_feedback;
    private readonly IAudioService audio_service;

    public ConsoleGameOutput(
        TextWriter writer,
        ConsoleBoardRenderer board_renderer)
        : this(
            writer,
            board_renderer,
            new ConsoleTheme(new PresentationPreferences()),
            new AsciiArtCatalog(),
            new VisualFeedbackService(board_renderer),
            new SilentAudioService())
    {
    }

    public ConsoleGameOutput(
        TextWriter writer,
        ConsoleBoardRenderer board_renderer,
        ConsoleTheme theme,
        AsciiArtCatalog art_catalog,
        IVisualFeedbackService visual_feedback,
        IAudioService audio_service)
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(board_renderer);
        ArgumentNullException.ThrowIfNull(theme);
        ArgumentNullException.ThrowIfNull(art_catalog);
        ArgumentNullException.ThrowIfNull(visual_feedback);
        ArgumentNullException.ThrowIfNull(audio_service);

        this.writer = writer;
        this.theme = theme;
        this.art_catalog = art_catalog;
        this.visual_feedback = visual_feedback;
        this.audio_service = audio_service;
    }

    public void show_match(Match match)
    {
        ArgumentNullException.ThrowIfNull(match);

        writer.WriteLine();

        BoardPosition? last_move =
            match.Moves.Count == 0
                ? null
                : match.Moves[^1].Position;

        visual_feedback.show_last_move(
            match.Board,
            last_move);

        if (last_move.HasValue)
        {
            audio_service.play(AudioCue.Move);
        }

        if (match.State == GameState.InProgress)
        {
            writer.WriteLine();
            writer.WriteLine(
                theme.colorize_accent(
                    $"Jogador atual: {match.CurrentPlayer.Name} " +
                    $"({match.CurrentPlayer.Symbol})"));
        }
    }

    public void show_invalid_move(
        Player player,
        BoardPosition position,
        string message)
    {
        ArgumentNullException.ThrowIfNull(player);

        audio_service.play(AudioCue.InvalidMove);

        writer.WriteLine(
            theme.colorize_warning(
                $"Jogada inválida de {player.Name} " +
                $"na posição ({position.Row + 1}, {position.Column + 1}): " +
                message));
    }

    public void show_result(Match match)
    {
        ArgumentNullException.ThrowIfNull(match);

        writer.WriteLine();

        if (match.WinningPositions.Count > 0)
        {
            visual_feedback.show_winning_sequence(
                match.Board,
                match.WinningPositions);
            writer.WriteLine();
        }

        switch (match.Result)
        {
            case GameResult.XWins:
                show_winner(match, Symbol.X);
                break;

            case GameResult.OWins:
                show_winner(match, Symbol.O);
                break;

            case GameResult.Draw:
                audio_service.play(AudioCue.Draw);
                show_art(art_catalog.get_draw());
                writer.WriteLine(
                    theme.colorize_warning("Resultado: empate."));
                break;

            default:
                writer.WriteLine("Resultado ainda não definido.");
                break;
        }
    }

    private void show_winner(
        Match match,
        Symbol symbol)
    {
        Player winner = match.get_player(symbol);
        bool automatic_match =
            match.FirstPlayer is ComputerPlayer &&
            match.SecondPlayer is ComputerPlayer;
        bool positive_result =
            automatic_match || winner is HumanPlayer;

        audio_service.play(
            positive_result
                ? AudioCue.Victory
                : AudioCue.Defeat);

        show_art(
            positive_result
                ? art_catalog.get_victory()
                : art_catalog.get_defeat());

        string message =
            $"Resultado: {winner.Name} venceu com {symbol}.";

        writer.WriteLine(
            positive_result
                ? theme.colorize_success(message)
                : theme.colorize_error(message));
    }

    private void show_art(IReadOnlyList<string> lines)
    {
        if (!theme.Preferences.VisualEffects)
        {
            return;
        }

        foreach (string line in lines)
        {
            writer.WriteLine(line);
        }
    }
}
