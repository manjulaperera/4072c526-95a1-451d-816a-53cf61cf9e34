using System.Globalization;

namespace CodingTest.Parsing;

public static class IntegerSequenceParser
{
    public static int[] Parse(string input)
    {
        string[] parts = input.Split(' ');
        var values = new int[parts.Length];

        for (int i = 0; i < parts.Length; i++)
        {
            values[i] = int.Parse(parts[i], NumberStyles.Integer, CultureInfo.InvariantCulture);
        }

        return values;
    }
}
