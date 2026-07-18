using TicTacToe.Domain;

namespace TicTacToe.Persistence;

/// <summary>
/// Reúne os dados externos necessários para converter uma partida concluída.
/// </summary>
public sealed record MatchPersistenceContext(
    Match Match,
    DateTimeOffset StartedAt,
    DateTimeOffset FinishedAt,
    string FirstStrategy,
    string SecondStrategy,
    int? RandomSeed,
    string ApplicationVersion);
