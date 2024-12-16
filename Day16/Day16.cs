using System.Diagnostics;

namespace Day16;

public class Day16 : DayBase {

    private const string DemoInput1 = "###############\n#.......#....E#\n#.#.###.#.###.#\n#.....#.#...#.#\n#.###.#####.#.#\n#.#.#.......#.#\n#.#.#####.###.#\n#...........#.#\n###.#.#####.#.#\n#...#.....#.#.#\n#.#.#.###.#.#.#\n#.....#...#.#.#\n#.###.#.#.#.#.#\n#S..#.....#...#\n###############";
    private const string DemoInput2 = "#################\n#...#...#...#..E#\n#.#.#.#.#.#.#.#.#\n#.#.#.#...#...#.#\n#.#.#.#.###.#.#.#\n#...#.#.#.....#.#\n#.#.#.#.#.#####.#\n#.#...#.#.#.....#\n#.#.#####.#.###.#\n#.#.#.......#...#\n#.#.###.#####.###\n#.#.#...#.....#.#\n#.#.#.#####.###.#\n#.#.#.........#.#\n#.#.#.#########.#\n#S#.............#\n#################";
    
    protected override int DayIndex() {
        return 16;
    }

    private (List<List<char>> map, (int x, int y) start, (int x, int y) end) ParseInput(string input) {
        var map = new List<List<char>>();
        (int x, int y) start = (0, 0);
        (int x, int y) end = (0, 0);
        var lines = input.Split("\n");

        for (var i = 0; i < lines.Length; i++) {
            var row = new List<char>();
            for (var j = 0; j < lines[i].Length; j++) {
                if (lines[i][j] == 'S') start = (j, i);
                else if (lines[i][j] == 'E') end = (j, i);
                row.Add(lines[i][j]);
            }
            map.Add(row);
        }

        return (map, start, end);
    }

    private (int, int) CheapestPath(List<List<char>> map, (int x, int y) start, (int x, int y) end) {
        var visited = new HashSet<(int x, int y, int direction)>();
        var queue = new PriorityQueue<(int x, int y, int direction), int>();
        var minDistances = new Dictionary<(int x, int y, int direction), int>();
        var previous = new Dictionary<(int x, int y, int direction), HashSet<(int x, int y, int direction)>>();
        // directions: 0 -- east, 1 -- south, 2 -- west, 3 -- north

        queue.Enqueue((start.x, start.y, 0), 0);
        minDistances.Add((start.x, start.y, 0), 0);

        while (queue.Count > 0) {
            var current = queue.Dequeue();

            void CheckNeighbour((int x, int y, int direction) n, int cost) {
                if (map[n.y][n.x] == '#') return;
                
                var distance = minDistances[current] + cost;
                if (minDistances.TryGetValue(n, out var minDist)) {
                    if (minDist > distance) {
                        minDistances[n] = distance;
                        previous[n] = [current];
                    } else if (minDist == distance) previous[n].Add(current);
                }
                else {
                    minDistances.Add(n, distance);
                    previous[n] = [current];
                }

                if (!visited.Contains(n)) queue.Enqueue(n, distance);
            }
            
            CheckNeighbour((current.x, current.y, (current.direction + 1) % 4), 1000); // rotate cw
            CheckNeighbour((current.x, current.y, current.direction - 1 < 0 ? 3 : current.direction - 1), 1000); // rotate ccw
            switch (current.direction) {
                case 0:
                    CheckNeighbour((current.x + 1, current.y, current.direction), 1);
                    break;
                case 1:
                    CheckNeighbour((current.x, current.y + 1, current.direction), 1);
                    break;
                case 2:
                    CheckNeighbour((current.x - 1, current.y, current.direction), 1);
                    break;
                case 3:
                    CheckNeighbour((current.x, current.y - 1, current.direction), 1);
                    break;
            }

            visited.Add(current);
        }

        var min = int.MaxValue;
        var minDirection = -1;
        for (int i = 0; i < 3; i++) {
            if (minDistances.TryGetValue((end.x, end.y, i), out var x))
                if (x < min) {
                    min = x;
                    minDirection = i;
                }
        }

        var countTiles = new HashSet<(int x, int y)>();
        var q = new Queue<(int x, int y, int direction)>();
        q.Enqueue((end.x, end.y, minDirection));

        while (q.Count > 0) {
            var current = q.Dequeue();
            countTiles.Add((current.x, current.y));
            
            if (current.x == start.x && current.y == start.y) continue;
            foreach (var n in previous[current]) {
                q.Enqueue(n);
            }
        }

        return (min, countTiles.Count);
    }

    public override string RunPart1(Stopwatch stopwatch) {
        var (map, start, end) = ParseInput(DemoInput2);
        var price = CheapestPath(map, start, end);

        return price.Item1.ToString();
    }

    public override string RunPart2(Stopwatch stopwatch) {
        var (map, start, end) = ParseInput(GetInput(stopwatch));
        var price = CheapestPath(map, start, end);

        return price.Item2.ToString();
    }
}