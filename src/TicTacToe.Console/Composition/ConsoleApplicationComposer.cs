
using TicTacToe.Persistence;
using TicTacToe.Presentation.Navigation;

namespace TicTacToe.Composition;

/// <summary>
/// Atua como composition root único da aplicação Console.
/// </summary>
public sealed class ConsoleApplicationComposer
{
    private readonly TextReader reader;
    private readonly TextWriter writer;

    public ConsoleApplicationComposer(
        TextReader reader,
        TextWriter writer)
    {
        ArgumentNullException.ThrowIfNull(reader);
        ArgumentNullException.ThrowIfNull(writer);

        this.reader = reader;
        this.writer = writer;
    }

    public ConsoleApplicationRuntime compose()
    {
        SettingsComponents settings =
            SettingsComposition.create();
        CompatibilityComponents compatibility =
            CompatibilityComposition.create(
                settings.Settings);
        MatchPersistenceComponents persistence =
            PersistenceComposition.create(
                settings.Settings);
        PresentationComponents presentation =
            PresentationComposition.create(
                reader,
                writer,
                compatibility.Platform,
                compatibility.Capabilities,
                compatibility.Preferences,
                persistence.PersistenceService);

        recover_statistics(
            persistence,
            presentation.FailureReporter);

        IScreen[] screens =
            ScreenComposition.create(
                reader,
                writer,
                presentation);
        ScreenContext context = new(
            compatibility.Preferences,
            settings.Settings,
            settings.Repository);

        return new ConsoleApplicationRuntime(
            new ScreenManager(screens),
            context);
    }

    private static void recover_statistics(
        MatchPersistenceComponents persistence,
        IExternalFailureReporter failure_reporter)
    {
        try
        {
            persistence.RecoveryService.recover();
        }
        catch (InfrastructureOperationException exception)
        {
            failure_reporter.report(
                "As estatísticas não puderam ser recuperadas.",
                exception);
        }
    }
}
