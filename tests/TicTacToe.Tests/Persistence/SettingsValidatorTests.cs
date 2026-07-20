using TicTacToe.Persistence;
using Xunit;

namespace TicTacToe.Tests.Persistence;

public class SettingsValidatorTests
{
    [Fact]
    public void validate_should_accept_default_settings()
    {
        SettingsValidator validator = new();

        SettingsValidationResult result =
            validator.validate(
                ApplicationSettings.create_default());

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }


    [Fact]
    public void validate_should_reject_missing_directories()
    {
        ApplicationSettings settings =
            ApplicationSettings.create_default();
        settings.Directories = null!;

        SettingsValidationResult result =
            new SettingsValidator().validate(settings);

        Assert.False(result.IsValid);
        Assert.Contains(
            "Directories deve ser informado.",
            result.Errors);
    }

    [Fact]
    public void validate_should_reject_empty_relative_directories()
    {
        ApplicationSettings settings =
            ApplicationSettings.create_default();
        settings.Directories.Data = " ";
        settings.Directories.Exports = string.Empty;

        SettingsValidationResult result =
            new SettingsValidator().validate(settings);

        Assert.False(result.IsValid);
        Assert.Contains(
            "Directories.Data não pode ser vazio.",
            result.Errors);
        Assert.Contains(
            "Directories.Exports não pode ser vazio.",
            result.Errors);
    }

    [Fact]
    public void validate_should_reject_invalid_delay_directory_and_strategy()
    {
        ApplicationSettings settings =
            ApplicationSettings.create_default();
        settings.AnimationDelayMilliseconds = 6000;
        settings.DefaultStrategy = "Other";
        settings.Directories.Data = Path.GetPathRoot(
            Environment.CurrentDirectory) ?? "/";
        SettingsValidator validator = new();

        SettingsValidationResult result =
            validator.validate(settings);

        Assert.False(result.IsValid);
        Assert.True(result.Errors.Count >= 3);
    }
}
