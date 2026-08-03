using CodingTest.Handlers;
using FluentAssertions;

namespace CodingTestUnitTests.Infrastructure;

[Trait("Category", "Infrastructure")]
public class WorkflowSmokeTests
{
    [Fact]
    public void SubSequenceHandler_stub_throws_not_implemented()
    {
        // Given
        const string input = "6 1 5 9 2";

        // When
        Action act = () => SubSequenceHandler.GetLongestIncreasingSubSequence(input);

        // Then
        act.Should().Throw<NotImplementedException>();
    }
}
