namespace CodingTest.Evaluation;

public static class ContiguousRunEvaluator
{
    public static int[] FindLongestRun(int[] values)
    {
        if (values.Length == 0)
        {
            return [];
        }

        int currentStart = 0;
        int currentLength = 0;
        int bestStart = 0;
        int bestLength = 0;

        for (int i = 0; i < values.Length; i++)
        {
            if (i == 0 || values[i] > values[i - 1])
            {
                if (currentLength == 0)
                {
                    currentStart = i;
                    currentLength = 1;
                }
                else
                {
                    currentLength++;
                }
            }
            else
            {
                currentStart = i;
                currentLength = 1;
            }

            if (currentLength > bestLength)
            {
                bestStart = currentStart;
                bestLength = currentLength;
            }
        }

        return values[bestStart..(bestStart + bestLength)];
    }
}
