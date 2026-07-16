using TicTacToe.AI;
using TicTacToe.Domain;
using Xunit;

namespace TicTacToe.Tests.AI;

/// <summary>
/// Verifica a delegação entre <see cref="ComputerPlayer"/> e sua estratégia.
/// </summary>
public class ComputerPlayerStrategyTests
{
    /// <summary>
    /// Confirma que o participante delega tabuleiro e símbolo à estratégia.
    /// </summary>
    [Fact]
    public void choose_move_should_delegate_to_strategy()
    {
        Board board = new();
        BoardPosition expected_position = new(2, 1);
        RecordingStrategy strategy = new(expected_position);
        ComputerPlayer player = new("CPU", Symbol.O, strategy);

        BoardPosition selected_position = player.choose_move(board);

        Assert.Equal(expected_position, selected_position);
        Assert.Same(board, strategy.ReceivedBoard);
        Assert.Equal(Symbol.O, strategy.ReceivedSymbol);
    }

    /// <summary>
    /// Confirma que estratégia nula é rejeitada.
    /// </summary>
    [Fact]
    public void constructor_should_reject_null_strategy()
    {
        Assert.Throws<ArgumentNullException>(
            () => new ComputerPlayer("CPU", Symbol.O, null!));
    }

    private sealed class RecordingStrategy : IMoveStrategy
    {
        private readonly BoardPosition position;

        public RecordingStrategy(BoardPosition position)
        {
            this.position = position;
        }

        public Board? ReceivedBoard { get; private set; }

        public Symbol ReceivedSymbol { get; private set; }

        public BoardPosition choose_move(Board board, Symbol symbol)
        {
            ReceivedBoard = board;
            ReceivedSymbol = symbol;

            return position;
        }
    }
}
