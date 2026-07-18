namespace TicTacToe.Persistence;

/// <summary>
/// Calcula estatísticas a partir do histórico persistido.
/// </summary>
public sealed class MatchStatisticsCalculator
{
    public MatchStatisticsRecord calculate(
        IReadOnlyList<MatchRecord> matches)
    {
        ArgumentNullException.ThrowIfNull(matches);

        if (matches.Count == 0)
        {
            return MatchStatisticsRecord.empty();
        }

        int x_wins =
            matches.Count(match => match.Result == "XWins");
        int o_wins =
            matches.Count(match => match.Result == "OWins");
        int draws =
            matches.Count(match => match.Result == "Draw");
        int total_moves =
            matches.Sum(match => match.Moves.Count);

        StrategyStatisticsRecord[] strategies =
            matches
                .SelectMany(get_computer_participants)
                .GroupBy(
                    item => item.Strategy!,
                    StringComparer.OrdinalIgnoreCase)
                .OrderBy(group => group.Key)
                .Select(group =>
                    new StrategyStatisticsRecord(
                        group.Key,
                        group.Count(),
                        group.Count(item => item.Won),
                        group.Count(item => item.Lost),
                        group.Count(item => item.Draw)))
                .ToArray();

        return new MatchStatisticsRecord(
            matches.Count,
            x_wins,
            o_wins,
            draws,
            total_moves,
            (double)total_moves / matches.Count,
            matches.Average(
                match => match.DurationMilliseconds),
            strategies);
    }

    private static IEnumerable<StrategyOutcome>
        get_computer_participants(MatchRecord match)
    {
        foreach (MatchParticipantRecord participant in
                 new[]
                 {
                     match.FirstParticipant,
                     match.SecondParticipant
                 })
        {
            if (participant.Kind != "Computer" ||
                string.IsNullOrWhiteSpace(participant.Strategy))
            {
                continue;
            }

            bool draw = match.Result == "Draw";
            bool won =
                match.Result == $"{participant.Symbol}Wins";

            yield return new StrategyOutcome(
                participant.Strategy,
                won,
                !won && !draw,
                draw);
        }
    }

    private sealed record StrategyOutcome(
        string Strategy,
        bool Won,
        bool Lost,
        bool Draw);
}
