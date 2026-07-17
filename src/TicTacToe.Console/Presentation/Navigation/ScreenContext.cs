using TicTacToe.Domain;
using TicTacToe.Presentation;

namespace TicTacToe.Presentation.Navigation;

/// <summary>
/// Mantém dados compartilhados entre telas sem armazenar regras de domínio.
/// </summary>
public sealed class ScreenContext
{
    public ScreenContext()
        : this(new PresentationPreferences())
    {
    }

    public ScreenContext(PresentationPreferences presentation_preferences)
    {
        ArgumentNullException.ThrowIfNull(presentation_preferences);
        PresentationPreferences = presentation_preferences;
    }

    /// <summary>
    /// Obtém as preferências visuais compartilhadas pelas telas.
    /// </summary>
    public PresentationPreferences PresentationPreferences { get; }

    /// <summary>
    /// Obtém a configuração selecionada para a próxima partida.
    /// </summary>
    public MatchConfiguration? MatchConfiguration { get; private set; }

    /// <summary>
    /// Obtém a última partida executada.
    /// </summary>
    public Match? LastMatch { get; private set; }

    /// <summary>
    /// Atualiza a configuração da próxima partida.
    /// </summary>
    /// <param name="configuration">Configuração validada pela apresentação.</param>
    public void set_match_configuration(
        MatchConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        MatchConfiguration = configuration;
    }

    /// <summary>
    /// Registra a última partida executada.
    /// </summary>
    /// <param name="match">Partida concluída ou em andamento.</param>
    public void set_last_match(Match match)
    {
        ArgumentNullException.ThrowIfNull(match);
        LastMatch = match;
    }
}
