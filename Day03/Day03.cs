using System.Text.RegularExpressions;

namespace Day03;

public class Day03 : DayBase
{
    private const string DemoInput = "xmul(2,4)%&mul[3,7]!@^do_not_mul(5,5)+mul(32,64]then(mul(11,8)mul(8,5))mul( 2, 2)";
    private const string DemoInput2 = "xmul(2,4)&mul[3,7]!^don't()_mul(5,5)+mul(32,64](mul(11,8)undo()?mul(8,5))";
    
    protected override int DayIndex()
    {
        return 3;
    }
    
    public override string RunPart1()
    {
        var pattern = @"mul\((\d{1,3}),(\d{1,3})\)";
        var reg = new Regex(pattern);

        var matches = reg.Matches(GetInput());

        var sum = 0;
        foreach (Match match in matches)
        {
            var x = match.Value;
            x = x.Remove(0, 4);
            x = x.Remove(x.Length - 1);
            
            var nums = x.Split(',');
            sum += int.Parse(nums[0]) * int.Parse(nums[1]);
        }

        return sum.ToString();
    }

    public override string RunPart2()
    {
        var pattern = @"(mul\((\d{1,3}),(\d{1,3})\)|do\(\)|don't\(\))";
        var reg = new Regex(pattern);

        var matches = reg.Matches(GetInput());

        var sum = 0;
        var enabled = true;
        foreach (Match match in matches)
        {
            var x = match.Value;
            if (x == "do()")
            {
                enabled = true;
                continue;
            }
            else if (x == "don't()")
            {
                enabled = false;
                continue;
            }

            if (!enabled) continue;
            
            x = x.Remove(0, 4);
            x = x.Remove(x.Length - 1);
            
            var nums = x.Split(',');
            sum += int.Parse(nums[0]) * int.Parse(nums[1]);
        }

        return sum.ToString();
    }
}