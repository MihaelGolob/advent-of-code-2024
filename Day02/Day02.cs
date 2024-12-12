namespace Day02;

public class Day02 : DayBase
{
    private const string DemoInput = "7 6 4 2 1\n1 2 7 8 9\n9 7 6 2 1\n1 3 2 4 5\n8 6 4 4 1\n1 3 6 7 9";
    
    protected override int DayIndex()
    {
        return 2;
    }

    private List<List<int>> ParseInput(string input)
    {
        var lines = input.Split('\n');
        var result = new List<List<int>>();
        foreach (var line in lines)
        {
            if (line == "") continue;
            var nums = line.Split(' ');
            result.Add(nums.Select(int.Parse).ToList());
        }

        return result;
    }

    private static bool IsReportSafe(List<int> report)
    {
        bool isDecreasing = report[0] > report[1];

        for (int i = 0; i < report.Count; i++)
        {
            if (i == 0) continue;
            if (isDecreasing && report[i - 1] <= report[i]) return false;
            if (!isDecreasing && report[i - 1] >= report[i]) return false;

            var diff = int.Abs(report[i - 1] - report[i]);
            if (diff < 1 || diff > 3) return false;
        }

        return true;
    }
   
    public override string RunPart1(System.Diagnostics.Stopwatch stopwatch)
    {
        var input = ParseInput(GetInput(stopwatch));
        var numSafeReports = input.Count(IsReportSafe);

        return numSafeReports.ToString();
    }

    public override string RunPart2(System.Diagnostics.Stopwatch stopwatch)
    {
        var input = ParseInput(GetInput(stopwatch));
        var numSafeReports = 0;

        foreach (var report in input)
        {
            int levelToRemove = -1;
            while (levelToRemove < report.Count)
            {
                if (levelToRemove == -1)
                {
                    if (IsReportSafe(report))
                    {
                        numSafeReports++;
                        break;
                    }
                }
                else
                {
                    var reportCopy = new List<int>(report);
                    reportCopy.RemoveAt(levelToRemove);
                    if (IsReportSafe(reportCopy))
                    {
                        numSafeReports++;
                        break;
                    }
                }

                levelToRemove++;
            }
        }
        
        return numSafeReports.ToString();
    }
}