using CodingTestCli;
using FluentAssertions;

namespace CodingTestUnitTests.Cli;

/// <summary>
/// Unit tests for CLI error handling and exit codes via <see cref="CliApplication"/>.
/// </summary>
[Trait("Category", "Cli")]
public class CliApplicationTests
{
    [Fact]
    public async Task Valid_args_write_result_to_stdout_and_return_zero()
    {
        using var output = new StringWriter();
        using var error = new StringWriter();

        int exitCode = await CliApplication.RunAsync(["6", "1", "5", "9", "2"], new StringReader(string.Empty), output, error);

        exitCode.Should().Be(0);
        output.ToString().Should().Be("1 5 9");
        error.ToString().Should().BeEmpty();
    }

    [Fact]
    public async Task Stdin_input_write_result_to_stdout_and_return_zero()
    {
        using var output = new StringWriter();
        using var error = new StringWriter();

        int exitCode = await CliApplication.RunAsync([], new StringReader("6 1 5 9 2"), output, error);

        exitCode.Should().Be(0);
        output.ToString().Should().Be("1 5 9");
        error.ToString().Should().BeEmpty();
    }

    [Fact]
    public async Task Invalid_token_writes_format_error_to_stderr_and_returns_one()
    {
        using var output = new StringWriter();
        using var error = new StringWriter();

        int exitCode = await CliApplication.RunAsync(["1", "abc", "3"], new StringReader(string.Empty), output, error);

        exitCode.Should().Be(1);
        output.ToString().Should().BeEmpty();
        error.ToString().Should().Contain("Error: invalid integer token.");
    }

    [Fact]
    public async Task Overflow_token_writes_range_error_to_stderr_and_returns_one()
    {
        using var output = new StringWriter();
        using var error = new StringWriter();

        int exitCode = await CliApplication.RunAsync(["2147483648"], new StringReader(string.Empty), output, error);

        exitCode.Should().Be(1);
        output.ToString().Should().BeEmpty();
        error.ToString().Should().Contain("Error: integer value out of range.");
    }
}
