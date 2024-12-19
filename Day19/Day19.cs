using System.Diagnostics;

namespace Day19;

public class Day19 : DayBase {
    private const string DemoInput = "r, wr, b, g, bwu, rb, gb, br\n\nbrwrr\nbggr\ngbbr\nrrbgbr\nubwu\nbwurrg\nbrgr\nbbrgwb";
    private readonly Dictionary<string, bool> _memoPossible = new();
    private readonly Dictionary<string, long> _memoCount = new();
     
    protected override int DayIndex() {
        return 19;
    }

    private (List<string> patterns, List<string> designs) ParseInput(string input) {
        var split = input.Split("\n\n");
        var patterns = split[0].Split(", ").ToList();
        var designs = split[1].Split("\n").ToList();
        designs.Remove("");

        return (patterns, designs);
    }

    private bool IsDesignPossible(List<string> patterns, string design) {
        if (design == "") return true;
        if (_memoPossible.TryGetValue(design, out var value)) return value;

        var possible = false;
        foreach (var pattern in patterns) {
            if (possible) break;
            
            if (pattern.Length > design.Length) continue;
            if (pattern == design.Substring(0, pattern.Length)) {
                possible |= IsDesignPossible(patterns, design.Substring(pattern.Length));
            }
        }

        _memoPossible.Add(design, possible);
        return possible;
    }
    
    private long CountDesignPossibilities(List<string> patterns, string design) {
        if (design == "") return 1;
        if (_memoCount.TryGetValue(design, out var value)) return value;

        long count = 0;
        foreach (var pattern in patterns) {
            if (pattern.Length > design.Length) continue;
            if (pattern == design.Substring(0, pattern.Length)) {
                count += CountDesignPossibilities(patterns, design.Substring(pattern.Length));
            }
        }

        _memoCount.Add(design, count);
        return count;
    }

    public override string RunPart1(Stopwatch stopwatch) {
        var (patterns, designs) = ParseInput(GetInput(stopwatch));
        var count = 0;

        foreach (var design in designs) {
            count += IsDesignPossible(patterns, design) ? 1 : 0;
        }
        
        return count.ToString();
    }

    public override string RunPart2(Stopwatch stopwatch) {
        var (patterns, designs) = ParseInput(GetInput(stopwatch));
        long count = 0;

        foreach (var design in designs) {
            count += CountDesignPossibilities(patterns, design);
        }
        
        return count.ToString();
    }
}