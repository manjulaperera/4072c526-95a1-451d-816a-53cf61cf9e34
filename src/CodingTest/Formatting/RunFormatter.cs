using System.Globalization;

namespace CodingTest.Formatting;

public static class RunFormatter
{
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
