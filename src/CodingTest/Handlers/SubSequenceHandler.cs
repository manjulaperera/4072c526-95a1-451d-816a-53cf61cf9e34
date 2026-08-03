using CodingTest.Evaluation;
using CodingTest.Formatting;
using CodingTest.Parsing;

namespace CodingTest.Handlers;

public static class SubSequenceHandler
{
    public static string GetLongestIncreasingSubSequence(string input)
    {
        int[] values = IntegerSequenceParser.Parse(input);
        int[] run = ContiguousRunEvaluator.FindLongestRun(values);
        return RunFormatter.Format(run);
    }
}
