using System.Diagnostics;

namespace Day18;

public class Day18 : DayBase {
    private const string DemoInput = "5,4\n4,2\n4,5\n3,0\n2,1\n6,3\n2,4\n1,5\n0,6\n3,3\n2,6\n5,1\n1,2\n5,5\n2,5\n6,5\n1,4\n0,4\n6,4\n1,1\n6,1\n1,0\n0,5\n1,6\n2,0";
    private const int Size = 71;
    
    protected override int DayIndex() {
        return 18;
    }

    private List<(int x, int y)> ParseInput(string input) {
        var output = new List<(int x, int y)>();
        var lines = input.Split("\n");

        foreach (var line in lines) {
            if (line == "") continue;
            var x = line.Split(",");
            output.Add((int.Parse(x[0]), int.Parse(x[1])));
        }

        return output;
    }

    private List<List<bool>> CreateMap(List<(int x, int y)> objects) {
        var map = new List<List<bool>>(Size);
        for (var i = 0; i < Size; i++) {
            var row = new List<bool>(Size);
            for (var j = 0; j < Size; j++) {
                row.Add(true);
            }
            map.Add(row);
        }

        foreach (var o in objects) {
            map[o.y][o.x] = false;
        }

        return map;
    }

    private bool IsInBounds((int x, int y) position) => position.x >= 0 && position.x < Size && position.y >= 0 && position.y < Size;

    private bool IsValid(List<List<bool>> map, (int x, int y) position) {
        return IsInBounds(position) && map[position.y][position.x];
    }

    private int FindShortestPath(List<List<bool>> map, (int x, int y) start, (int x, int y) end) {
        var queue = new Queue<(int x, int y)>();
        var visited = new HashSet<(int x, int y)>();
        var previous = new Dictionary<(int x, int y), (int x, int y)>();

        queue.Enqueue(start);
        while (queue.Count > 0) {
            var current = queue.Dequeue();
            if (current.x == end.x && current.y == end.y) {
                break;
            }

            List<(int x, int y)> moves = [(1, 0), (-1, 0), (0, 1), (0, -1)];

            foreach (var move in moves) {
                var next = (current.x + move.x, current.y + move.y);
                if (!visited.Contains(next) && IsValid(map, next)) {
                    queue.Enqueue(next);
                    previous[next] = current;
                    visited.Add(next);
                }    
            }
        }

        if (!previous.ContainsKey(end)) return -1;

        var p = end;
        int count = 0;
        while (p != start) {
            p = previous[p];
            count++;
        }

        return count;
    }

    public override string RunPart1(Stopwatch stopwatch) {
        var bytes = ParseInput(GetInput(stopwatch));
        var map = CreateMap(bytes.Slice(0, 1024));
        var numSteps = FindShortestPath(map, (0, 0), (70, 70));

        return numSteps.ToString();
    }

    public override string RunPart2(Stopwatch stopwatch) {
        var bytes = ParseInput(GetInput(stopwatch));
        var result = "";
        
        for (int i = 0; i < bytes.Count; i++) {
            var map = CreateMap(bytes.Slice(0, i+1));
            var numSteps = FindShortestPath(map, (0, 0), (70, 70));

            if (numSteps == -1) {
                result = $"{bytes[i].x},{bytes[i].y}";
                break;
            }
        }

        return result;
    }
}