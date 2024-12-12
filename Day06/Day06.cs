namespace Day06;

public class Day06 : DayBase
{
    private static string DemoInput = "....#.....\n.........#\n..........\n..#.......\n.......#..\n..........\n.#..^.....\n........#.\n#.........\n......#...";

    private (List<List<bool>> map, (int, int) position) ParseInput(string input)
    {
        var map = new List<List<bool>>();
        var position = (-1, -1);
        var lines = input.Split('\n');
        
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i] == "") continue;

            var x = new List<bool>();
            for (int j = 0; j < lines[i].Length; j++)
            {
                if (lines[i][j] == '.') x.Add(false);
                if (lines[i][j] == '#') x.Add(true);
                if (lines[i][j] == '^')
                {
                    x.Add(false);
                    position = (j, i);
                }
            }
            
            map.Add(x);
        }

        return (map, position);
    }

    private bool IsOutOfBounds(List<List<bool>> map, (int x, int y) position)
    {
        return position.x < 0 || position.y < 0 || position.y >= map.Count || position.x >= map[0].Count;
    }

    private int Rotate(int orientation)
    {
        orientation++;
        if (orientation > 3) orientation = 0;

        return orientation;
    }

    private (int x, int y) Move((int x, int y) position, int orientation)
    {
        switch (orientation)
        {
            case 0:
                return (position.x, position.y - 1);
            case 1:
                return (position.x + 1, position.y);
            case 2:
                return (position.x, position.y + 1);
            case 3:
                return (position.x - 1, position.y);
        }

        return position;
    }

    private bool HasClearWay(List<List<bool>> map, (int x, int y) position, int orientation)
    {
        var newPos = Move(position, orientation);
        return IsOutOfBounds(map, newPos) || !map[newPos.y][newPos.x];
    }
    
    private bool IsInLoop(List<List<bool>> map, (int x, int y) position, int orientation)
    {
        var visited = new HashSet<((int x, int y) position, int orientation)> { (position, orientation) };

        while (!IsOutOfBounds(map, position))
        {
            var moved = false;
            if (HasClearWay(map, position, orientation))
            {
                position = Move(position, orientation);
                moved = true;
            }
            else orientation = Rotate(orientation);

            if (moved && visited.Contains((position, orientation))) return true;
            visited.Add((position, orientation));
        }

        return false;
    }
    
    protected override int DayIndex()
    {
        return 6;
    }
    
    public override string RunPart1(System.Diagnostics.Stopwatch stopwatch)
    {
        var (map, position) = ParseInput(GetInput(stopwatch));
        var orientation = 0; // up 0 right 1 down 2 left 3
        var visitedPositions = new HashSet<(int, int)>();

        while (!IsOutOfBounds(map, position))
        {
            visitedPositions.Add(position);
            if (HasClearWay(map, position, orientation)) position = Move(position, orientation);
            else orientation = Rotate(orientation);
        }
        
        return visitedPositions.Count.ToString();
    }

    public override string RunPart2(System.Diagnostics.Stopwatch stopwatch)
    {
        var (map, position) = ParseInput(GetInput(stopwatch));

        var count = 0;
        for (var i = 0; i < map.Count; i++)
        {
            for (var j = 0; j < map[0].Count; j++)
            {
                if (map[i][j]) continue;
                if (position.Item1 == j && position.Item2 == i) continue;
                map[i][j] = true;
                if (IsInLoop(map, position, 0))
                {
                    count++;
                }
                map[i][j] = false;
            }
        }

        return count.ToString();
    }
}