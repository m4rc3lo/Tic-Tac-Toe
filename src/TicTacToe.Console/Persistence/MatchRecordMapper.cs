using TicTacToe.Domain;

namespace TicTacToe.Persistence;

/// <summary>
/// Converte entidades do domínio em registros imutáveis de persistência.
/// </summary>
public sealed class MatchRecordMapper
{
    /// <summary>
    /// Converte uma partida concluída.
    /// </summary>
    public MatchRecord map(MatchPersistenceContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(context.Match);

        if (context.Match.State != GameState.Finished)
        {
            throw new InvalidOperationException(
                "Somente partidas concluídas podem ser persistidas.");
        }

        if (context.FinishedAt < context.StartedAt)
        {
            throw new ArgumentException(
                "O término não pode anteceder o início.",
                nameof(context));
        }

        TimeSpan duration =
            context.FinishedAt - context.StartedAt;

        return new MatchRecord(
            Guid.NewGuid(),
            context.StartedAt,
            context.FinishedAt,
            (long)duration.TotalMilliseconds,
            map_participant(
                context.Match.FirstPlayer,
                context.FirstStrategy),
            map_participant(
                context.Match.SecondPlayer,
                context.SecondStrategy),
            context.Match.Moves
                .Select(map_move)
                .ToArray(),
            context.Match.Result.ToString(),
            context.Match.WinningPositions
                .Select(position =>
                    new BoardPositionRecord(
                        position.Row,
                        position.Column))
                .ToArray(),
            context.RandomSeed,
            context.ApplicationVersion);
    }

    private static MatchParticipantRecord map_participant(
        Player player,
        string strategy)
    {
        string kind = player is HumanPlayer
            ? "Human"
            : "Computer";

        return new MatchParticipantRecord(
            player.Name,
            kind,
            player.Symbol.ToString(),
            player is ComputerPlayer
                ? strategy
                : null);
    }

    private static MatchMoveRecord map_move(Move move)
    {
        return new MatchMoveRecord(
            move.TurnNumber,
            move.Position.Row,
            move.Position.Column,
            move.Symbol.ToString());
    }
}
