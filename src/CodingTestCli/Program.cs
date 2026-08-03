using CodingTest.Handlers;

try
{
    string input = args.Length > 0
        ? string.Join(' ', args)
        : await Console.In.ReadLineAsync() ?? string.Empty;

    string result = SubSequenceHandler.GetLongestIncreasingSubSequence(input);
    await Console.Out.WriteAsync(result);
    return 0;
}
catch (Exception)
{
    return 1;
}
