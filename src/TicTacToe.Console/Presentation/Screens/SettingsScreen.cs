using TicTacToe.Presentation.Navigation;

namespace TicTacToe.Presentation.Screens;

/// <summary>
/// Permite ativar ou desativar preferências visuais da apresentação.
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
            writer.WriteLine("Configurações visuais");
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
            writer.WriteLine("0 - Voltar");
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

                case "0":
                    return new ScreenTransition(ScreenState.MainMenu);

                default:
                    writer.WriteLine("Opção inválida.");
                    break;
            }
        }
    }

    private static string format(bool value)
    {
        return value ? "ativado" : "desativado";
    }
}
