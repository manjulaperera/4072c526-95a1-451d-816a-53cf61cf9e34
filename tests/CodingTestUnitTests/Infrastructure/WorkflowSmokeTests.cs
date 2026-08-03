using CodingTest.Handlers;
using FluentAssertions;

namespace CodingTestUnitTests.Infrastructure;

[Trait("Category", "Infrastructure")]
public class WorkflowSmokeTests
{
    [Fact]
    public void SubSequenceHandler_returns_longest_contiguous_run_for_sample_input()
    {
        string output = SubSequenceHandler.GetLongestIncreasingSubSequence("6 1 5 9 2");

        output.Should().Be("1 5 9");
    }
}
