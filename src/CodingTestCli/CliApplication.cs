using CodingTest.Handlers;

namespace CodingTestCli;

/// <summary>
/// CLI entry logic: reads input, invokes the handler, and maps exceptions to exit codes and stderr messages.
/// </summary>
public static class CliApplication
{
    /// <summary>
    /// Runs the CLI using the supplied input/output streams.
    /// </summary>
    /// <param name="args">Command-line arguments; when present they are joined with a single space.</param>
    /// <param name="input">Standard input when <paramref name="args"/> is empty.</param>
    /// <param name="output">Destination for the handler result on success.</param>
    /// <param name="error">Destination for error messages on failure.</param>
    /// <returns>0 on success; 1 when an exception is handled.</returns>
    public static async Task<int> RunAsync(
        string[] args,
        TextReader input,
        TextWriter output,
        TextWriter error)
    {
        try
        {
            string line = args.Length > 0
                ? string.Join(' ', args)
                : await input.ReadLineAsync() ?? string.Empty;

            string result = SubSequenceHandler.GetLongestIncreasingSubSequence(line);
            await output.WriteAsync(result);
            return 0;
        }
        catch (FormatException ex)
        {
            await error.WriteLineAsync($"Error: invalid integer token. {ex.Message}");
            return 1;
        }
        catch (OverflowException ex)
        {
            await error.WriteLineAsync($"Error: integer value out of range. {ex.Message}");
            return 1;
        }
        catch (Exception ex)
        {
            await error.WriteLineAsync($"Error: {ex.Message}");
            return 1;
        }
    }
}
