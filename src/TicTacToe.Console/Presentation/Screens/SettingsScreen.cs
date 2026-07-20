using TicTacToe.Persistence;
using TicTacToe.Presentation.Navigation;

namespace TicTacToe.Presentation.Screens;

/// <summary>
/// Permite configurar e persistir preferências da apresentação.
/// </summary>
public sealed class SettingsScreen : IScreen
{
    private readonly TextReader reader;
    private readonly TextWriter writer;

    public SettingsScreen(
        TextReader reader,
        TextWriter writer)
    {
        ArgumentNullException.ThrowIfNull(reader);
        ArgumentNullException.ThrowIfNull(writer);
        this.reader = reader;
        this.writer = writer;
    }

    public ScreenState State => ScreenState.Settings;

    public ScreenTransition show(ScreenContext context)
    {
        PresentationPreferences preferences =
            context.PresentationPreferences;

        while (true)
        {
            writer.WriteLine();
            writer.WriteLine("Configurações");
            writer.WriteLine(
                $"1 - Unicode: {format(preferences.UseUnicode)}");
            writer.WriteLine(
                $"2 - Cores ANSI: {format(preferences.UseAnsiColors)}");
            writer.WriteLine(
                $"3 - Limpeza de tela: {format(preferences.ClearScreen)}");
            writer.WriteLine(
                $"4 - Efeitos visuais: {format(preferences.VisualEffects)}");
            writer.WriteLine(
                $"5 - Áudio: {format(preferences.AudioEnabled)}");
            writer.WriteLine(
                $"6 - Atraso: {preferences.AnimationDelayMilliseconds} ms");
            writer.WriteLine("0 - Salvar e voltar");
            writer.Write("Opção: ");

            switch (reader.ReadLine()?.Trim())
            {
                case "1":
                    preferences.UseUnicode = !preferences.UseUnicode;
                    break;
                case "2":
                    preferences.UseAnsiColors = !preferences.UseAnsiColors;
                    break;
                case "3":
                    preferences.ClearScreen = !preferences.ClearScreen;
                    break;
                case "4":
                    preferences.VisualEffects = !preferences.VisualEffects;
                    break;
                case "5":
                    preferences.AudioEnabled = !preferences.AudioEnabled;
                    break;
                case "6":
                    read_delay(preferences);
                    break;
                case "0":
                    persist(context);
                    return new ScreenTransition(ScreenState.MainMenu);
                default:
                    writer.WriteLine("Opção inválida.");
                    break;
            }
        }
    }

    private void read_delay(PresentationPreferences preferences)
    {
        writer.Write("Novo atraso em milissegundos [0-5000]: ");
        string? input = reader.ReadLine();

        if (int.TryParse(input, out int delay) &&
            delay >= 0 && delay <= 5000)
        {
            preferences.AnimationDelayMilliseconds = delay;
            return;
        }

        writer.WriteLine("Atraso inválido.");
    }

    private void persist(ScreenContext context)
    {
        try
        {
            context.persist_presentation_preferences();
            writer.WriteLine("Configurações salvas.");
        }
        catch (InfrastructureOperationException exception)
        {
            writer.WriteLine(
                $"Não foi possível salvar as configurações. " +
                $"Diagnóstico: {exception.DiagnosticId:N}.");
        }
        catch (IOException exception)
        {
            writer.WriteLine(
                $"Não foi possível salvar as configurações: {exception.Message}");
        }
        catch (UnauthorizedAccessException exception)
        {
            writer.WriteLine(
                $"Não foi possível salvar as configurações: {exception.Message}");
        }
        catch (ArgumentException exception)
        {
            writer.WriteLine(
                $"Configurações inválidas: {exception.Message}");
        }
    }

    private static string format(bool value)
    {
        return value ? "ativado" : "desativado";
    }
}
