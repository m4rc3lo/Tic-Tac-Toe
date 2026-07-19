using System.Reflection;
using TicTacToe.Application;
using Xunit;

namespace TicTacToe.Tests.Application;

public class ExperimentArchitectureTests
{
    [Fact]
    public void controller_contracts_should_not_expose_presentation_types()
    {
        Type controller_type = typeof(ExperimentController);

        Type[] exposed_types = controller_type
            .GetConstructors(BindingFlags.Public | BindingFlags.Instance)
            .SelectMany(constructor => constructor.GetParameters())
            .Select(parameter => parameter.ParameterType)
            .Append(
                controller_type.GetMethod(nameof(ExperimentController.run))!
                    .ReturnType)
            .ToArray();

        Assert.DoesNotContain(
            exposed_types,
            type => contains_presentation_namespace(type));
    }

    private static bool contains_presentation_namespace(Type type)
    {
        if (type.Namespace?.StartsWith(
                "TicTacToe.Presentation",
                StringComparison.Ordinal) == true)
        {
            return true;
        }

        return type.IsGenericType &&
               type.GetGenericArguments()
                   .Any(contains_presentation_namespace);
    }
}
