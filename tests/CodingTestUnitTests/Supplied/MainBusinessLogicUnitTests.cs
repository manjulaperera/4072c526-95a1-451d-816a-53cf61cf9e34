using CodingTest.Handlers;
using CodingTestUnitTests.Fixtures;
using FluentAssertions;

namespace CodingTestUnitTests.Supplied;

[Trait("Category", "Unit_Tests")]
public class MainBusinessLogicUnitTests
{
    [Fact]
    public void Test_Case_One() => RunCase(1);

    [Fact]
    public void Test_Case_Two() => RunCase(2);

    [Fact]
    public void Test_Case_Three() => RunCase(3);

    [Fact]
    public void Test_Case_Four() => RunCase(4);

    [Fact]
    public void Test_Case_Five() => RunCase(5);

    [Fact]
    public void Test_Case_Six() => RunCase(6);

    [Fact]
    public void Test_Case_Seven() => RunCase(7);

    [Fact]
    public void Test_Case_Eight() => RunCase(8);

    [Fact]
    public void Test_Case_Nine() => RunCase(9);

    [Fact]
    public void Test_Case_Ten() => RunCase(10);

    [Fact]
    public void Test_Case_Eleven() => RunCase(11);

    private static void RunCase(int caseNumber)
    {
        // Given
        string input = AcceptanceFixtureReader.ReadInput(caseNumber);
        string expectedSubSequence = AcceptanceFixtureReader.ReadExpected(caseNumber);

        // When
        string output = SubSequenceHandler.GetLongestIncreasingSubSequence(input);

        // Then
        output.Should().Be(expectedSubSequence);
    }
}
