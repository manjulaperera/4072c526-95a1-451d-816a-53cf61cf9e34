using CodingTest.Handlers;
using FluentAssertions;

namespace CodingTestUnitTests.Supplementary;

/// <summary>
/// Supplementary tests for invalid and boundary input (FR-C01 through FR-C03).
/// Covers null, empty, malformed tokens, whitespace edge cases, overflow, and single-token input.
/// </summary>
[Trait("Category", "Supplementary")]
public class InvalidInputTests
{
    [Fact]
    public void Null_input_throws_ArgumentNullException()
    {
        Action act = () => SubSequenceHandler.GetLongestIncreasingSubSequence(null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Empty_input_returns_empty_string()
    {
        string output = SubSequenceHandler.GetLongestIncreasingSubSequence(string.Empty);

        output.Should().Be(string.Empty);
    }

    [Fact]
    public void Single_token_returns_that_token()
    {
        string output = SubSequenceHandler.GetLongestIncreasingSubSequence("42");

        output.Should().Be("42");
    }

    [Fact]
    public void Invalid_token_throws_FormatException()
    {
        Action act = () => SubSequenceHandler.GetLongestIncreasingSubSequence("1 abc 3");

        act.Should().Throw<FormatException>();
    }

    [Fact]
    public void Repeated_spaces_throw_FormatException_on_empty_token()
    {
        Action act = () => SubSequenceHandler.GetLongestIncreasingSubSequence("1  2");

        act.Should().Throw<FormatException>();
    }

    [Fact]
    public void Leading_space_throws_FormatException_on_empty_token()
    {
        Action act = () => SubSequenceHandler.GetLongestIncreasingSubSequence(" 1 2");

        act.Should().Throw<FormatException>();
    }

    [Fact]
    public void Trailing_space_throws_FormatException_on_empty_token()
    {
        Action act = () => SubSequenceHandler.GetLongestIncreasingSubSequence("1 2 ");

        act.Should().Throw<FormatException>();
    }

    [Fact]
    public void Overflow_token_throws_OverflowException()
    {
        Action act = () => SubSequenceHandler.GetLongestIncreasingSubSequence("2147483648");

        act.Should().Throw<OverflowException>();
    }
}
