using CodingTest.Handlers;

try
{
    string input = args.Length > 0
        ? string.Join(' ', args)
        : Console.In.ReadLine() ?? string.Empty;

    string result = SubSequenceHandler.GetLongestIncreasingSubSequence(input);
    Console.Write(result);
    return 0;
}
catch
{
    return 1;
}
