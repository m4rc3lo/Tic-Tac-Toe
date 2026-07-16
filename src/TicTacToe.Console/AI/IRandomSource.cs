namespace TicTacToe.AI;

/// <summary>
/// Abstrai a geração de números pseudoaleatórios para permitir testes e
/// experimentos reproduzíveis.
/// </summary>
public interface IRandomSource
{
    /// <summary>
    /// Produz um inteiro no intervalo de zero, inclusivo, até o limite,
    /// exclusivo.
    /// </summary>
    /// <param name="max_exclusive">Limite superior exclusivo.</param>
    /// <returns>Valor pseudoaleatório válido para o intervalo.</returns>
    int next(int max_exclusive);
}
