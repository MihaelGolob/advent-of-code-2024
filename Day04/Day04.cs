namespace Day04;

public class Day04 : DayBase
{

    private const string DemoInput = "MMMSXXMASM\nMSAMXMSMSA\nAMXSXMAAMM\nMSAMASMSMX\nXMASAMXAMM\nXXAMMXXAMA\nSMSMSASXSS\nSAXAMASAAA\nMAMMMXMMMM\nMXMXAXMASX";
    
    private int[] _rowDir = { -1, 1, -1, -1, 0, 1, 1, 0 };
    private int[] _colDir = { 1, 1, 0, -1, 1, 0, -1, -1 };
    
    protected override int DayIndex()
    {
        return 4;
    }

    List<List<char>> ParseInput(string input)
    {
        var lines = input.Split('\n');
        var res = new List<List<char>>();
        
        foreach (var line in lines)
        {
            if (line == "") continue;
            res.Add(new List<char>());
            res[^1].AddRange(line);
        }

        return res;
    }

    bool IsValidPosition(List<List<char>> grid, int x, int y)
    {
        return x < grid[0].Count && x >= 0 && y < grid.Count && y >= 0;
    }

    bool FindWord(List<List<char>> grid, string word, int x, int y, int direction, int index)
    {
        if (index == word.Length) return true;
        if (!IsValidPosition(grid, x, y)) return false;
        if (grid[y][x] != word[index]) return false;

        return FindWord(grid, word, x + _rowDir[direction], y + _colDir[direction], direction, index + 1);
    }
    
    public override string RunPart1()
    {
        var grid = ParseInput(GetInput());
        var sum = 0;
        
        for (int i = 0; i < grid.Count; i++)
        {
            for (int j = 0; j < grid[0].Count; j++)
            {
                for (int d = 0; d < 8; d++)
                {
                    if (FindWord(grid, "XMAS", j, i, d, 0)) sum++;
                }
            }
        }

        return sum.ToString();
    }

    public override string RunPart2()
    {
        var grid = ParseInput(GetInput());
        var sum = 0;
        
        for (int i = 0; i < grid.Count; i++)
        {
            for (int j = 0; j < grid[0].Count; j++)
            {
                if (FindWord(grid, "MAS", j, i, 1, 0) || FindWord(grid, "SAM", j, i, 1, 0))
                {
                    if (FindWord(grid, "MAS", j + 2, i, 0, 0) || FindWord(grid, "SAM", j + 2, i, 0, 0))
                    {
                        sum++;
                    }
                }
            }
        }
        
        return sum.ToString();
    }
}   