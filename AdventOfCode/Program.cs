using AdventOfCode;
using System.Drawing;
using System.Text;

var unsortedNums =
    Encoding.UTF8.GetString(Resource.InputD1)
    .Split('\n')
    .SkipLast(1)
    .Select(s =>
    {
        var split = s.Split(' ', StringSplitOptions.TrimEntries);
        return new Point(int.Parse(split.First()), int.Parse(split.Last()));
    })
    .ToArray();

#if D1P1
var xSort = unsortedNums.OrderBy(o => o.X).ToList();
var ySort = unsortedNums.OrderBy(o => o.Y).ToList();
int solution = 0;
for (int i = 0; i < ySort.Count; i++)
{
    var diff = (xSort[i].X - ySort[i].Y);
    if (diff <= 0)
        diff *= -1;

    solution += diff;
}

Console.WriteLine(solution);
#elif D1P2
Console.WriteLine(unsortedNums
    .Select(s => s.X)
    .Distinct()
    .Sum(u =>
        unsortedNums
        .GroupBy(g => g.Y)
        .FirstOrDefault(f => f.Key == u)
        ?.Count() * u
        ?? 0
    ));
#endif
