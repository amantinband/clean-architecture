using FluentAssertions;

namespace NSubstitute;

public static class Must
{
    public static List<T> BeEmptyList<T>() =>
        Arg.Do<List<T>>(x => x.Should().BeEmpty());

    public static List<T> BeListWith<T>(List<T> value) =>
        Arg.Do<List<T>>(x => x.Should().BeEquivalentTo(value));
}