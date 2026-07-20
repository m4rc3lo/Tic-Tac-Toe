
using System.Reflection;
using TicTacToe.AI;
using TicTacToe.Domain;
using Xunit;

namespace TicTacToe.Tests.Architecture;

public class ArchitectureBoundaryTests
{
    private static readonly string[] forbidden_domain_namespaces =
    [
        "TicTacToe.AI",
        "TicTacToe.Audio",
        "TicTacToe.Compatibility",
        "TicTacToe.Persistence",
        "TicTacToe.Presentation"
    ];

    [Fact]
    public void domain_public_api_should_not_expose_external_modules()
    {
        Type[] domain_types = typeof(Match).Assembly
            .GetTypes()
            .Where(type =>
                type.Namespace == "TicTacToe.Domain")
            .ToArray();

        foreach (Type type in domain_types)
        {
            Assert.DoesNotContain(
                get_exposed_types(type),
                exposed => forbidden_domain_namespaces.Any(
                    prefix =>
                        exposed.Namespace?.StartsWith(
                            prefix,
                            StringComparison.Ordinal) == true));
        }
    }

    [Fact]
    public void match_should_expose_board_only_as_read_only_contract()
    {
        PropertyInfo property =
            typeof(Match).GetProperty(
                nameof(Match.Board))!;

        Assert.Equal(
            typeof(IReadOnlyBoard),
            property.PropertyType);
        Assert.False(property.CanWrite);
    }

    [Fact]
    public void strategies_should_receive_read_only_board()
    {
        MethodInfo method =
            typeof(IMoveStrategy).GetMethod(
                nameof(IMoveStrategy.choose_move))!;

        Assert.Equal(
            typeof(IReadOnlyBoard),
            method.GetParameters()[0].ParameterType);
    }

    private static IEnumerable<Type> get_exposed_types(
        Type type)
    {
        foreach (ConstructorInfo constructor in
                 type.GetConstructors())
        {
            foreach (ParameterInfo parameter in
                     constructor.GetParameters())
            {
                yield return unwrap(parameter.ParameterType);
            }
        }

        foreach (PropertyInfo property in
                 type.GetProperties())
        {
            yield return unwrap(property.PropertyType);
        }

        foreach (MethodInfo method in
                 type.GetMethods(
                     BindingFlags.Instance |
                     BindingFlags.Static |
                     BindingFlags.Public |
                     BindingFlags.DeclaredOnly))
        {
            yield return unwrap(method.ReturnType);

            foreach (ParameterInfo parameter in
                     method.GetParameters())
            {
                yield return unwrap(parameter.ParameterType);
            }
        }
    }

    private static Type unwrap(Type type)
    {
        if (type.IsArray)
        {
            return unwrap(type.GetElementType()!);
        }

        if (type.IsGenericType)
        {
            Type[] arguments = type.GetGenericArguments();

            if (arguments.Length == 1)
            {
                return unwrap(arguments[0]);
            }
        }

        return type;
    }
}
