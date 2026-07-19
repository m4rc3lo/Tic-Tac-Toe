using TicTacToe.Domain;
using TicTacToe.Persistence;

namespace TicTacToe.Presentation.Navigation;

/// <summary>
/// Mantém dados compartilhados entre telas sem armazenar regras de domínio.
/// </summary>
public sealed class ScreenContext
{
    public ScreenContext()
        : this(
            new PresentationPreferences(),
            ApplicationSettings.create_default(),
            settings_repository: null)
    {
    }

    public ScreenContext(
        PresentationPreferences presentation_preferences)
        : this(
            presentation_preferences,
            ApplicationSettings.create_default(),
            settings_repository: null)
    {
    }

    public ScreenContext(
        PresentationPreferences presentation_preferences,
        ApplicationSettings application_settings,
        ISettingsRepository? settings_repository)
    {
        ArgumentNullException.ThrowIfNull(presentation_preferences);
        ArgumentNullException.ThrowIfNull(application_settings);

        PresentationPreferences = presentation_preferences;
        ApplicationSettings = application_settings;
        SettingsRepository = settings_repository;
    }

    public PresentationPreferences PresentationPreferences { get; }

    public ApplicationSettings ApplicationSettings { get; }

    public ISettingsRepository? SettingsRepository { get; }

    public MatchConfiguration? MatchConfiguration { get; private set; }

    public Match? LastMatch { get; private set; }

    public AutomaticMatchConfiguration? AutomaticMatchConfiguration
    {
        get;
        private set;
    }

    public AutomaticMatchResult? LastAutomaticMatchResult
    {
        get;
        private set;
    }

    public void set_match_configuration(
        MatchConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        MatchConfiguration = configuration;
    }

    public void set_last_match(Match match)
    {
        ArgumentNullException.ThrowIfNull(match);
        LastMatch = match;
    }

    public void set_automatic_match_configuration(
        AutomaticMatchConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        AutomaticMatchConfiguration = configuration;
    }

    public void set_last_automatic_match_result(
        AutomaticMatchResult result)
    {
        ArgumentNullException.ThrowIfNull(result);
        LastAutomaticMatchResult = result;
    }

    public void persist_presentation_preferences()
    {
        ApplicationSettings.UseUnicode =
            PresentationPreferences.UseUnicode;
        ApplicationSettings.UseAnsiColors =
            PresentationPreferences.UseAnsiColors;
        ApplicationSettings.ClearScreen =
            PresentationPreferences.ClearScreen;
        ApplicationSettings.AnimationsEnabled =
            PresentationPreferences.VisualEffects;
        ApplicationSettings.AudioEnabled =
            PresentationPreferences.AudioEnabled;
        ApplicationSettings.AnimationDelayMilliseconds =
            PresentationPreferences.AnimationDelayMilliseconds;

        SettingsRepository?.save(ApplicationSettings);
    }
}
