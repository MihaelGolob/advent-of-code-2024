namespace Day07;

public class Day07 : DayBase
{
    private const string DemoInput =
        "190: 10 19\n3267: 81 40 27\n83: 17 5\n156: 15 6\n7290: 6 8 6 15\n161011: 16 10 13\n192: 17 8 14\n21037: 9 7 18 13\n292: 11 6 16 20";
    
    protected override int DayIndex()
    {
        return 7;
    }

    private List<(long result, List<long> numbers)> ParseInput(string input)
    {
        var output = new List<(long result, List<long> numbers)>();
        var lines = input.Split('\n');
        foreach (var line in lines)
        {
            if (line == "") continue;
            var x = line.Split(":");
            var result = long.Parse(x[0]);
            var y = x[1].Split(" ");

            var numbers = new List<long>();
            foreach (var a in y)
            {
                if (a == "" || a == " ") continue;
                numbers.Add(long.Parse(a));
            }
            output.Add((result, numbers));
        }

        return output;
    }

    private long ConcatNumbers(long a, long b)
    {
        return long.Parse(a.ToString() + b.ToString());
    }

    private long CalculateNumbers(List<long> numbers, List<char> operators)
    {
        long result = 0;
        for (var i = 1; i < numbers.Count; i++)
        {
            if (operators[i - 1] == '+')
            {
                if (i == 1) result = numbers[0] + numbers[1];
                else result += numbers[i];
            } else if (operators[i - 1] == '*')
            {
                if (i == 1) result = numbers[0] * numbers[1];
                else result *= numbers[i];
            } else if (operators[i - 1] == '|')
            {
                if (i == 1) result = ConcatNumbers(numbers[0], numbers[1]);
                else result = ConcatNumbers(result, numbers[i]);
            }
        }
        
        return result;
    }

    private bool IsSolvable(long result, List<long> numbers, List<char> possibleOperators, List<char> operators)
    {
        if (operators.Count == numbers.Count - 1)
        {
            return result == CalculateNumbers(numbers, operators);
        }

        var output = false;
        foreach (var op in possibleOperators)
        {
            operators.Add(op);
            output |= IsSolvable(result, numbers, possibleOperators, operators);
            operators.RemoveAt(operators.Count - 1);
        }

        return output;
    }

    public override string RunPart1(System.Diagnostics.Stopwatch stopwatch)
    {
        var input = ParseInput(GetInput(stopwatch));
        long sum = 0;

        foreach (var equation in input)
        {
            if (IsSolvable(equation.result, equation.numbers, ['+', '*'], []))
            {
                sum += equation.result;
            }
        }

        return sum.ToString();
    }

    public override string RunPart2(System.Diagnostics.Stopwatch stopwatch)
    {
        var input = ParseInput(GetInput(stopwatch));
        long sum = 0;

        foreach (var equation in input)
        {
            if (IsSolvable(equation.result, equation.numbers, ['+', '*', '|'], []))
            {
                sum += equation.result;
            }
        }

        return sum.ToString();
    }
}