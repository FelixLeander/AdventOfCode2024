var unsortedNums =
    System.Text.Encoding.UTF8.GetString(AdventOfCode.Resource.InputD1)
    .Split('\n')
    .SkipLast(1)
    .Select(s =>
    {
        var split = s.Split(' ', StringSplitOptions.TrimEntries);
        return new System.Drawing.Point(int.Parse(split.First()), int.Parse(split.Last()));
    })
    .ToArray();

Console.WriteLine(
#if D1P1
    unsortedNums.OrderBy(o => o.X)
        .Zip(unsortedNums.OrderBy(o => o.Y))
        .Sum(s =>
            int.Abs(s.First.X - s.Second.Y))
#elif D1P2
unsortedNums
        .Select(s => s.X)
        .Distinct()
        .Sum(u =>
            unsortedNums
            .GroupBy(g => g.Y)
            .FirstOrDefault(f =>
                f.Key == u)
            ?.Count() * u
                ?? 0
        )
#endif
);
