using System.Text;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Collections.Frozen;

static void Warning(string e, bool newLine = true) => PrintColor(e, ConsoleColor.Yellow, newLine);
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
    var joker = true;
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
            {
                if (joker)
                {
                    Warning("Joker used, changeDir+.");
                    joker = false;
                    return true;
                }
                return false;
            }
        }
        else
        {
            if (last > a)
            {
                last = a;
                return true;
            }
            else
            {
                if (joker)
                {
                    Warning("Joker used, changeDir-.");
                    joker = false;
                    return true;
                }
                return false;
            }
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
    var joker = true;
    var withinLimit = report.All(a =>
    {
        if (last == null) // First
        {
            last = a;
            return true;
        }

        if (last == a) // same
        {
            if (joker)
            {
                Warning("Joker used, same.");
                joker = false;
                return true;
            }

            Error($"Same {last}=={a}");
            return false;
        }

        if (last * crease > a * crease) // Change in direction
        {
            if (joker)
            {
                Warning("Joker used, direction chagne.");
                joker = false;
                return true;
            }
            Error($"Direction change: {last} => {a}");
            return false;
        }

        var limitChange = (limit * crease) + last;
        if (crease == 1 ? limitChange < a : limitChange > a) // Overshoot
        {
            if (joker)
            {
                Warning("Joker used, overshoot.");
                return true;
            }
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

    PrintColor($"Valid", ConsoleColor.Cyan);
    valid++;
}

Console.WriteLine(valid);
Console.WriteLine("END");
#endif
#endregion


#region Day3
#if D3P1
var mulStarts = Encoding.UTF8.GetString(AdventOfCode.Resource.InputD3)
    .Split("mul(");

int sum = 0;
foreach (var mulLine in mulStarts)
{
    Console.WriteLine();
    Console.WriteLine(mulLine);
    var commaPos = mulLine.IndexOf(',');
    if (commaPos == -1)
    {
        Error("Skip, no comma.");
        continue;
    }

    var firstNumText = mulLine[..commaPos];
    if (!int.TryParse(firstNumText, out int numOne))
    {
        Error($"P1 not a number '{numOne}'.");
        continue;
    }

    var paranthesisPos = mulLine.IndexOf(')', commaPos);
    if (paranthesisPos == -1)
    {
        Error("Skip, no paranthesisPos.");
        continue;
    }

    var secondNumtext = mulLine[(commaPos+1)..paranthesisPos];
    if (!int.TryParse(secondNumtext, out int numTwo))
    {
        Error($"P2 not a number '{numTwo}'.");
        continue;
    }

    var mulResult = numOne * numTwo;
    sum += mulResult;
    Valid($"P1: {numOne}, P2: {numTwo}");
}

Console.WriteLine(sum);

#endif
#endregion