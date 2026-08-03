using System.Reflection;

namespace CodingTestUnitTests.Fixtures;

public static class AcceptanceFixtureReader
{
    private const string ResourcePrefix = "CodingTestUnitTests.Fixtures.";

    public static string ReadInput(int caseNumber) =>
        ReadFixture(caseNumber, "input");

    public static string ReadExpected(int caseNumber) =>
        ReadFixture(caseNumber, "expected");

    private static string ReadFixture(int caseNumber, string kind)
    {
        var resourceName = $"{ResourcePrefix}ac-{caseNumber:D2}-{kind}.txt";
        var assembly = Assembly.GetExecutingAssembly();

        using var stream = assembly.GetManifestResourceStream(resourceName)
            ?? throw new InvalidOperationException($"Embedded resource not found: {resourceName}");

        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}
