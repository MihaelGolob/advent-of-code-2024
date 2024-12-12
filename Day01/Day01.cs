namespace Day01;

public class Day01 : DayBase {
    
    protected override int DayIndex() => 1;

    private string demoInput = "3   4\n4   3\n2   5\n1   3\n3   9\n3   3";
    
    public override string RunPart1(System.Diagnostics.Stopwatch stopwatch)
    {
        var input = GetInput(stopwatch);

        var left = new List<int>();
        var right = new List<int>();

        var lines = input.Split('\n');
        foreach (var line in lines)
        {
            var nums = line.Split(' ');
            if (nums.Length < 4) continue;
            
            left.Add(int.Parse(nums[0]));
            right.Add(int.Parse(nums[3]));
        }
        
        left.Sort();
        right.Sort();

        int sum = 0;
        for (int i = 0; i < left.Count; ++i)
        {
            sum += int.Abs(left[i] - right[i]);
        }
        
        return sum.ToString();
    }
    
    public override string RunPart2(System.Diagnostics.Stopwatch stopwatch)
    {
        var input = GetInput(stopwatch);

        var left = new List<int>();
        var right = new List<int>();

        var lines = input.Split('\n');
        foreach (var line in lines)
        {
            var nums = line.Split(' ');
            if (nums.Length < 4) continue;
            
            left.Add(int.Parse(nums[0]));
            right.Add(int.Parse(nums[3]));
        }

        var similarityScore = 0;
        foreach (var x in left)
        {
            int count = 0;
            foreach (var y in right)
            {
                if (x == y) count++;
            }

            similarityScore += x * count;
        }

        return similarityScore.ToString();
    }
}