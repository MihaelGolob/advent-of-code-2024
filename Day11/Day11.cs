using System.Diagnostics.CodeAnalysis;

namespace Day11;

public class Day11 : DayBase {

    private const string DemoInput = "125 17";
    private Dictionary<(long stone, int depth), long> cache = new();
    
    protected override int DayIndex() {
        return 11;
    }

    private List<long> ParseInput(string input) {
        var output = new List<long>();
        var nums = input.Split(' ');
        foreach (var x in nums) {
            output.Add(long.Parse(x));
        }

        return output;
    }

    private List<long> SimulateBlinkNaive(List<long> stones) {
        var tmp = new List<long>();
        foreach (var x in stones) {
            if (x == 0) tmp.Add(1);
            else if (x.ToString().Length % 2 == 0) {
                var num = x.ToString();
                tmp.Add(long.Parse(num.Substring(0, num.Length/2)));
                tmp.Add(long.Parse(num.Substring(num.Length/2, num.Length/2)));
            }
            else tmp.Add(x * 2024);
        }

        return tmp;
    }

    private long SimulateBlinkRecursive(int currentDepth, long stone) {
        if (cache.ContainsKey((stone, currentDepth))) return cache[(stone, currentDepth)];
        if (currentDepth == 0) {
            return 1;
        }

        if (stone == 0) {
            var tmp = SimulateBlinkRecursive(currentDepth - 1, 1);
            cache.Add((stone, currentDepth), tmp);
            return tmp;
        }
        
        if (stone.ToString().Length % 2 == 0) {
            var num = stone.ToString();
            var tmp = SimulateBlinkRecursive(currentDepth - 1, long.Parse(num.Substring(0, num.Length / 2)))
                   + SimulateBlinkRecursive(currentDepth - 1,
                       long.Parse(num.Substring(num.Length / 2, num.Length / 2)));
            cache.Add((stone, currentDepth), tmp);
            return tmp;
        }
        
        var temp = SimulateBlinkRecursive(currentDepth - 1, stone * 2024);
        cache.Add((stone, currentDepth), temp);
        return temp;
    }

    public override string RunPart1() {
        var stones = ParseInput(GetInput());
        var numBlinks = 25;

        for (var i = 0; i < numBlinks; i++) {
            stones = SimulateBlinkNaive(stones);
        }

        return stones.Count.ToString();
    }

    public override string RunPart2() {
        var stones = ParseInput(GetInput());
        var numBlinks = 75;

        long sum = 0;
        foreach (var x in stones) {
            sum += SimulateBlinkRecursive(numBlinks, x);
        }

        return sum.ToString();
    }
}
