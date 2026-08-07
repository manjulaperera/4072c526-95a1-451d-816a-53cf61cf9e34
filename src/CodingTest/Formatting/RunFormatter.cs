using System.Globalization;

namespace CodingTest.Formatting;

internal static class RunFormatter
{
    /// <summary>
    /// Formats an array of integers into a single space-separated string using the invariant culture.
    /// Returns an empty string when the input array is empty.
    /// </summary>
    /// <param name="values">The integers to format.</param>
    /// <returns>A space-separated string representation of the input integers.</returns>
    public static string Format(int[] values)
    {
        if (values.Length == 0)
        {
            return string.Empty;
        }

        return string.Join(
            ' ',
            values.Select(static value => value.ToString(CultureInfo.InvariantCulture)));
    }
}
