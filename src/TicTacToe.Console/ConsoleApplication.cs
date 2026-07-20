using TicTacToe.Application;
using TicTacToe.Audio;
using TicTacToe.Persistence;
using TicTacToe.Presentation;
using TicTacToe.Presentation.Navigation;
using TicTacToe.Presentation.Screens;

namespace TicTacToe;

/// <summary>
/// Compõe e executa a aplicação Console.
/// </summary>
public sealed class ConsoleApplication
{
    private readonly TextReader reader;
    private readonly TextWriter writer;

    public ConsoleApplication(
        TextReader reader,
        TextWriter writer)
    {
        ArgumentNullException.ThrowIfNull(reader);
        ArgumentNullException.ThrowIfNull(writer);
        this.reader = reader;
        this.writer = writer;
    }

    public void run()
    {
        string settings_path = Path.Combine(
            AppContext.BaseDirectory,
            "data",
            "settings.json");
        ISettingsRepository settings_repository =
            new JsonSettingsRepository(
                settings_path,
                new SettingsValidator());
        ApplicationSettings settings = settings_repository.load();
        PresentationPreferences preferences =
            PresentationPreferences.from_settings(settings);
        ConsoleTheme theme = new(preferences);
        AsciiArtCatalog art_catalog = new();
        IDelayService delay_service = new ThreadDelayService();
        IAnimationService animation_service =
            new AnimationService(
                writer,
                delay_service,
                preferences);
        ConsoleBoardRenderer board_renderer = new(writer, theme);
        ConsoleGameInput game_input = new(reader, writer);
        IVisualFeedbackService visual_feedback =
            new VisualFeedbackService(board_renderer);
        IAudioService enabled_audio =
            new AudioServiceSelector(writer).select(
                audio_enabled: true);
        IAudioService audio_service =
            new PreferenceAwareAudioService(
                preferences,
                enabled_audio);
        ConsoleGameOutput game_output = new(
            writer,
            board_renderer,
            theme,
            art_catalog,
            visual_feedback,
            audio_service);
        MatchPersistenceComponents persistence_components =
            create_match_persistence(settings);
        IMatchPersistenceService persistence_service =
            persistence_components.PersistenceService;
        IExternalFailureReporter failure_reporter =
            new TextExternalFailureReporter(writer);

        try
        {
            persistence_components.RecoveryService.recover();
        }
        catch (InfrastructureOperationException exception)
        {
            failure_reporter.report(
                "As estatísticas não puderam ser recuperadas.",
                exception);
        }
        IMoveStrategyFactory strategy_factory =
            new MoveStrategyFactory();
        IMatchSessionRunner match_runner =
            new ConsoleMatchSessionRunner(
                game_input,
                game_output,
                animation_service,
                preferences,
                persistence_service,
                strategy_factory,
                failure_reporter);
        IAutomaticModeControl mode_control =
            new ConsoleAutomaticModeControl(
                writer,
                delay_service);
        IAutomaticMatchRunner automatic_runner =
            new AutomaticMatchRunner(
                writer,
                game_output,
                delay_service,
                preferences,
                strategy_factory,
                mode_control,
                persistence_service,
                failure_reporter);
        CitationMetadata citation_metadata =
            new CitationMetadataLoader().load(
                Path.Combine(
                    AppContext.BaseDirectory,
                    "CITATION.cff"));

        IScreen[] screens =
        [
            new SplashScreen(
                reader,
                writer,
                theme,
                art_catalog,
                animation_service),
            new MainMenuScreen(reader, writer),
            new MatchSetupScreen(reader, writer),
            new PlayingScreen(match_runner),
            new MatchResultScreen(reader, writer),
            new AutomaticSetupScreen(reader, writer),
            new AutomaticPlayingScreen(automatic_runner),
            new StatisticsScreen(reader, writer),
            new ExperimentSetupScreen(reader, writer),
            new SettingsScreen(reader, writer),
            new HelpScreen(reader, writer),
            new CreditsScreen(reader, writer, citation_metadata),
            new ExitScreen(writer)
        ];

        ScreenContext context = new(
            preferences,
            settings,
            settings_repository);

        new ScreenManager(screens).run(
            ScreenState.Splash,
            context);
    }

    private static MatchPersistenceComponents create_match_persistence(
        ApplicationSettings settings)
    {
        string data_directory = Path.Combine(
            AppContext.BaseDirectory,
            settings.Directories.Data);
        IMatchHistoryRepository history_repository =
            new JsonMatchHistoryRepository(
                Path.Combine(data_directory, "matches.json"));
        IMatchStatisticsRepository statistics_repository =
            new JsonMatchStatisticsRepository(
                Path.Combine(data_directory, "statistics.json"));
        MatchStatisticsCalculator calculator = new();

        return new MatchPersistenceComponents(
            new MatchPersistenceService(
                history_repository,
                statistics_repository,
                new MatchRecordMapper(),
                calculator),
            new MatchPersistenceRecoveryService(
                history_repository,
                statistics_repository,
                calculator));
    }

    private sealed record MatchPersistenceComponents(
        IMatchPersistenceService PersistenceService,
        IMatchPersistenceRecoveryService RecoveryService);
}
