using System.Text;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Collections.Frozen;

static void Error(string e, bool newLine = true) => PrintColor(e, ConsoleColor.Red, newLine);
static void Valid(string e, bool newLine = true) => PrintColor(e, ConsoleColor.Green, newLine);
static void PrintColor(string text, ConsoleColor consoleColor, bool newLine = true)
{
    Console.ForegroundColor = consoleColor;
    if (newLine)
        Console.WriteLine(text);
    else
        Console.Write(text);
    Console.ForegroundColor = ConsoleColor.White;
}

#region Day1
#if D1P1 || D1P2
var unsortedNums = Encoding.UTF8.GetString(AdventOfCode.Resource.InputD1).Split('\n').SkipLast(1).Select(s =>
{
    var split = s.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
    return new Point(int.Parse(split.First()), int.Parse(split.Last()));
}).ToArray();
#endif

#if D1P1
Console.WriteLine(
unsortedNums.OrderBy(o => o.X)
    .Zip(unsortedNums.OrderBy(o => o.Y))
    .Sum(s =>
        int.Abs(s.First.X - s.Second.Y)
        )
);

#elif D1P2
Console.WriteLine(
unsortedNums.Select(s => s.X)
    .Distinct()
    .Sum(u =>
        unsortedNums.GroupBy(g => g.Y)
        .FirstOrDefault(f => f.Key == u)
        ?.Count() * u ?? 0
    )
);
#endif
#endregion

#region Day2
#if D2P1

var reports = Encoding.UTF8.GetString(AdventOfCode.Resource.InputD2)
    .Split('\n', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
    .Select(s =>
        s.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
        .Select(int.Parse)
        .ToArray())
    .ToArray();

static bool? DetermineCrease(int[] nums)
{
    int last = nums.First();
    var numNotFirst = nums.Skip(1);
    var increase = nums.First() < numNotFirst.First();

    var valid = numNotFirst.All(a =>
    {
        if (increase)
        {
            if (last < a)
            {
                last = a;
                return true;
            }
            else
                return false;
        }
        else
        {
            if (last > a)
            {
                last = a;
                return true;
            }
            else
                return false;
        }
    });

    if (!valid)
        return null;

    return increase;
}

var limit = 3;
int? last = null;
var valid = 0;
foreach (var report in reports)
{
    Console.WriteLine();
    Console.WriteLine(string.Join(',', report));

    var direction = DetermineCrease(report); //I know this word dosen'T exist :d
    if (direction == null)
    {
        Error("No direction");
        continue;
    }

    Console.WriteLine(direction.Value ? "Increaseing" : "Decreasing");

    var crease = direction.Value ? 1 : -1;
    var withinLimit = report.All(a =>
    {
        if (last == null) // First
        {
            last = a;
            return true;
        }

        if (last == a) // same
        {
            Error($"Same {last}=={a}");
            return false;
        }

        if (last * crease > a * crease) // Change in direction
        {
            Error($"Direction change: {last} => {a}");
            return false;
        }

        var limitChange = (limit * crease) + last;
        if (crease == 1 ? limitChange < a : limitChange > a) // Overshoot
        {
            Error($"{limit}: out {last} > {a}");
            return false;
        }

        Valid($"{limit}: in {last} < {a}");
        last = a;
        return true;
    });

    last = null;
    if (!withinLimit)
        continue;

    PrintColor($"Valid", ConsoleColor.DarkYellow);
    valid++;
}

Console.WriteLine(valid);
Console.WriteLine("END");
#endif
#endregion


