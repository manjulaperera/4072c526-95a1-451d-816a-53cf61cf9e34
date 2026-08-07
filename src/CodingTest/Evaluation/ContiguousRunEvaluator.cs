namespace CodingTest.Evaluation;

internal static class ContiguousRunEvaluator
{
    /// <summary>
    /// Finds the longest strictly increasing contiguous subsequence (run) in the provided integer array.
    ///
    /// How it works:
    /// - If the input is empty, an empty array is returned.
    /// - The method does a single forward pass (O(n)) over the array while maintaining two running windows:
    ///   * "current" window: the start index and length of the current strictly increasing run that ends at the current element.
    ///   * "best" window: the start index and length of the longest run seen so far.
    /// - For each element, if it continues an increasing sequence (value > previous value) we extend the current window; otherwise
    ///   we start a new current window at the current element.
    /// - Whenever the current window becomes longer than the best window, the best window is updated.
    /// - At the end the method returns a slice of the original array corresponding to the best window.
    ///
    /// Note: If there are multiple runs with the same maximum length, the first one encountered is returned.
    /// </summary>
    /// <param name="values">Input array of integers to search for the longest increasing contiguous run.</param>
    /// <returns>An array containing the longest strictly increasing contiguous run from the input.</returns>
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
