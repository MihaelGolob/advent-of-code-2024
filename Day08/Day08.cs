namespace Day08;

public class Day08 : DayBase
{
    private const string DemoInput =
        "............\n........0...\n.....0......\n.......0....\n....0.......\n......A.....\n............\n............\n........A...\n.........A..\n............\n............";

    private int gridWidth = -1;
    private int gridHeight = -1;
        
    protected override int DayIndex()
    {
        return 8;
    }

    private Dictionary<char, List<(int x, int y)>> ParseInput(string input)
    {
        var output = new Dictionary<char, List<(int x, int y)>>();
        var lines = input.Split('\n');
        gridHeight = 0;
        gridWidth = lines[0].Length;

        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i] == "") continue;
            gridHeight++;

            for (int j = 0; j < lines[i].Length; j++)
            {
                if (lines[i][j] == '.') continue;
                var antenna = lines[i][j];

                if (output.ContainsKey(antenna))
                {
                    output[antenna].Add((j, i));
                }
                else
                {
                    output.Add(antenna, [(j, i)]);
                }
            }
        }

        return output;
    }

    private ((int, int), (int, int)) CalculateAntiNodes((int x, int y) a, (int x, int y) b)
    {
        if (a.y > b.y)
        {
            (a, b) = (b, a);
        }
        
        var dx = Math.Abs(a.x - b.x);
        var dy = Math.Abs(a.y - b.y);

        if (a.x < b.x) return ((a.x - dx, a.y - dy), (b.x + dx, b.y + dy));
        return ((a.x + dx, a.y - dy), (b.x - dx, b.y + dy));
    }
    
    private List<(int, int)> CalculateAntiNodes2((int x, int y) a, (int x, int y) b)
    {
        if (a.y > b.y)
        {
            (a, b) = (b, a);
        }
        
        var dx = Math.Abs(a.x - b.x);
        var dy = Math.Abs(a.y - b.y);

        var result = new List<(int, int)>();
        var an1 = a;
        var an2 = b;

        while (IsInBounds(an1.x, an1.y) || IsInBounds(an2.x, an2.y))
        {
            if (IsInBounds(an1.x, an1.y)) result.Add(an1);
            if (IsInBounds(an2.x, an2.y)) result.Add(an2);

            if (a.x < b.x)
            {
                an1 = (an1.x - dx, an1.y - dy);
                an2 = (an2.x + dx, an2.y + dy);
            }
            else
            {
                an1 = (an1.x + dx, an1.y - dy);
                an2 = (an2.x - dx, an2.y + dy); 
            }
        }

        return result;
    }

    private bool IsInBounds(int x, int y)
    {
        return x >= 0 && x < gridWidth && y >= 0 && y < gridHeight;
    }

    public override string RunPart1(System.Diagnostics.Stopwatch stopwatch)
    {
        var map = ParseInput(GetInput(stopwatch));
        var uniqueAntiNodes = new HashSet<(int x, int y)>();

        foreach (var freq in map.Keys)
        {
            var antennas = map[freq];

            for (int a = 0; a < antennas.Count - 1; a++)
            {
                for (int b = a + 1; b < antennas.Count; b++)
                {
                    var antiNodes = CalculateAntiNodes(antennas[a], antennas[b]);
                    if (IsInBounds(antiNodes.Item1.Item1, antiNodes.Item1.Item2)) uniqueAntiNodes.Add(antiNodes.Item1);
                    if (IsInBounds(antiNodes.Item2.Item1, antiNodes.Item2.Item2)) uniqueAntiNodes.Add(antiNodes.Item2);
                }
            }
        }

        return uniqueAntiNodes.Count.ToString();
    }

    public override string RunPart2(System.Diagnostics.Stopwatch stopwatch)
    {
        var map = ParseInput(GetInput(stopwatch));
        var uniqueAntiNodes = new HashSet<(int x, int y)>();

        foreach (var freq in map.Keys)
        {
            var antennas = map[freq];

            for (int a = 0; a < antennas.Count - 1; a++)
            {
                for (int b = a + 1; b < antennas.Count; b++)
                {
                    var antiNodes = CalculateAntiNodes2(antennas[a], antennas[b]);
                    foreach (var an in antiNodes)
                    {
                        uniqueAntiNodes.Add(an);
                    }
                }
            }
        }

        return uniqueAntiNodes.Count.ToString();
    }
}