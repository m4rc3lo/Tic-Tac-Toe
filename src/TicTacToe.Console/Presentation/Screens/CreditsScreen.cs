using TicTacToe.Presentation.Navigation;

namespace TicTacToe.Presentation.Screens;

/// <summary>
/// Apresenta créditos derivados dos metadados de citação do projeto.
/// </summary>
public sealed class CreditsScreen : IScreen
{
    private readonly TextReader reader;
    private readonly TextWriter writer;
    private readonly CitationMetadata metadata;

    public CreditsScreen(
        TextReader reader,
        TextWriter writer,
        CitationMetadata metadata)
    {
        ArgumentNullException.ThrowIfNull(reader);
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(metadata);

        this.reader = reader;
        this.writer = writer;
        this.metadata = metadata;
    }

    public ScreenState State => ScreenState.Credits;

    public ScreenTransition show(ScreenContext context)
    {
        writer.WriteLine();
        writer.WriteLine("Créditos");
        writer.WriteLine($"Projeto: {metadata.Title}");
        writer.WriteLine($"Versão: {metadata.Version}");
        writer.WriteLine($"Autor: {metadata.Author}");
        writer.WriteLine($"Licença: {metadata.License}");
        writer.WriteLine($"Repositório: {metadata.Repository}");
        writer.WriteLine();
        writer.WriteLine(
            "Metadados baseados no arquivo CITATION.cff.");
        writer.WriteLine("Pressione Enter para voltar ao menu.");
        reader.ReadLine();

        return new ScreenTransition(ScreenState.MainMenu);
    }
}
