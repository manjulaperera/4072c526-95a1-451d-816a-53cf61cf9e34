using CodingTest.Evaluation;
using CodingTest.Formatting;
using CodingTest.Parsing;

namespace CodingTest.Handlers;

/// <summary>
/// Finds the longest contiguous strictly increasing run in a whitespace-separated integer string.
/// </summary>
public static class SubSequenceHandler
{
    /// <summary>
    /// Returns the longest contiguous strictly increasing run, formatted with single-space delimiters.
    /// When multiple runs share the maximum length, the earliest run is returned.
    /// </summary>
    /// <param name="input">Whitespace-separated base-10 integers.</param>
    /// <returns>Selected run as a space-delimited string, or empty string when input is empty.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="input"/> is null.</exception>
    /// <exception cref="FormatException">When a token is not a valid base-10 integer.</exception>
    /// <exception cref="OverflowException">When a token exceeds the signed 32-bit range.</exception>
    public static string GetLongestIncreasingSubSequence(string input)
    {
        int[] values = IntegerSequenceParser.Parse(input);
        int[] run = ContiguousRunEvaluator.FindLongestRun(values);
        return RunFormatter.Format(run);
    }
}
