using System.Globalization;

namespace CodingTest.Parsing;

internal static class IntegerSequenceParser
{
    /// <summary>
    /// Parses a space-separated sequence of integers from the given input string.
    /// </summary>
    /// <param name="input">The input string containing integers separated by spaces.</param>
    /// <returns>An array of parsed integers.</returns>
    public static int[] Parse(string input)
    {
        ArgumentNullException.ThrowIfNull(input);

        if (input.Length == 0)
        {
            return [];
        }

        string[] parts = input.Split(' ');
        var values = new int[parts.Length];

        for (int i = 0; i < parts.Length; i++)
        {
            values[i] = int.Parse(parts[i], NumberStyles.Integer, CultureInfo.InvariantCulture);
        }

        return values;
    }
}
