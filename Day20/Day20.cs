using System.Diagnostics;

namespace Day20;

public class Day20 : DayBase {
    private const string DemoInput = "###############\n#...#...#.....#\n#.#.#.#.#.###.#\n#S#...#.#.#...#\n#######.#.#.###\n#######.#.#...#\n#######.#.###.#\n###..E#...#...#\n###.#######.###\n#...###...#...#\n#.#####.#.###.#\n#.#...#.#.#...#\n#.#.#.#.#.#.###\n#...#...#...###\n###############";
    
    protected override int DayIndex() {
        return 20;
    }

    private List<List<char>> ParseInput(string input) {
        var output = new List<List<char>>();
        foreach (var row in input.Split("\n")) {
            if (row == "") continue;
            output.Add(row.ToList());
        }

        return output;
    }

    private (Dictionary<(int x, int y), int>, List<(int x, int y)>) FindPath(List<List<char>> map) {
        (int x, int y) start = (-1, -1);
        for (var i = 0; i < map.Count; i++) {
            for (var j = 0; j < map[0].Count; j++) {
                if (map[i][j] == 'S') {
                    start = (j, i);
                    break;
                }
            }
            if (start != (-1, -1)) break;
        }

        var path = new Dictionary<(int x, int y), int>();
        var pathList = new List<(int x, int y)>();
        var current = start;
        List<(int x, int y)> moves = [(-1, 0), (1, 0), (0, 1), (0, -1)];

        while (map[current.y][current.x] != 'E') {
            pathList.Add(current);
            path.Add(current, pathList.Count - 1);

            foreach (var move in moves) {
                (int x, int y) next = (current.x + move.x, current.y + move.y);
                if (!path.ContainsKey(next) && (map[next.y][next.x] == '.' || map[next.y][next.x] == 'E')) {
                    current = next;
                    break;
                } 
            }
        }
        pathList.Add(current);
        path.Add(current, pathList.Count - 1);

        return (path, pathList);
     }
     
    public override string RunPart1(Stopwatch stopwatch) {
        var map = ParseInput(GetInput(stopwatch));
        var (path, pathList) = FindPath(map);

        bool IsInBounds((int x, int y) a) {
            return a.x >= 0 && a.y >= 0 && a.x < map[0].Count && a.y < map.Count;
        }

        var sum = 0;
        Dictionary<int, int> count = new(); // for debugging
        foreach (var current in pathList) {
            if (IsInBounds((current.x + 2, current.y)) && map[current.y][current.x + 1] == '#' && map[current.y][current.x + 2] == '.') {
                var saved = path[current] - path[(current.x + 2, current.y)] - 2;
                if (saved > 0 && !count.TryAdd(saved, 1)) count[saved]++;
                if (saved >= 100) sum++;
            }
            if (IsInBounds((current.x - 2, current.y)) && map[current.y][current.x - 1] == '#' && map[current.y][current.x - 2] == '.') {
                var saved = path[current] - path[(current.x - 2, current.y)] - 2;
                if (saved > 0 && !count.TryAdd(saved, 1)) count[saved]++;
                if (saved >= 100) sum++;
            } 
            if (IsInBounds((current.x, current.y + 2)) && map[current.y + 1][current.x] == '#' && map[current.y + 2][current.x] == '.') {
                var saved = path[current] - path[(current.x, current.y + 2)] - 2;
                if (saved > 0 && !count.TryAdd(saved, 1)) count[saved]++;
                if (saved >= 100) sum++;
            }
            if (IsInBounds((current.x, current.y - 2)) && map[current.y - 1][current.x] == '#' && map[current.y - 2][current.x] == '.') {
                var saved = path[current] - path[(current.x, current.y - 2)] - 2;
                if (saved > 0 && !count.TryAdd(saved, 1)) count[saved]++;
                if (saved >= 100) sum++;
            }
        }
        
        return sum.ToString();
    }

    public override string RunPart2(Stopwatch stopwatch) {
        var map = ParseInput(GetInput(stopwatch));
        var (path, pathList) = FindPath(map);

        bool IsInBounds((int x, int y) a) {
            return a.x >= 0 && a.y >= 0 && a.x < map[0].Count && a.y < map.Count;
        }

        var memo = new Dictionary<((int x, int y) pos, int moves), Dictionary<(int x, int y), int>>();
        Dictionary<(int x, int y), int> GetAllCheats((int x, int y) start, (int x, int y) current, int moves) {
            if (memo.ContainsKey((current, moves))) return memo[(current, moves)];
            if (!IsInBounds(current)) return [];
            
            var result = new Dictionary<(int x, int y), int>();
            if (current != start && map[current.y][current.x] != '#') {
                result.Add(current, 20 - moves);
            } 
            if (moves <= 0) {
                return [];
            }

            foreach (var (dx, dy) in new[] { (1, 0), (-1, 0), (0, 1), (0, -1) }) {
                var next = (current.x + dx, current.y + dy);
                var cheats = GetAllCheats(start, next, moves - 1);
                foreach (var cheat in cheats) {
                    if (result.TryGetValue(cheat.Key, out var existing)) {
                        result[cheat.Key] = Math.Min(existing, cheat.Value);
                    } else {
                        result[cheat.Key] = cheat.Value;
                    }
                }
            }
            
            if (current != start) memo.Add((current, moves), result);
            return result;
        }

        var count = new Dictionary<int, int>();
        long sum = 0;
        foreach (var current in pathList) {
            var cheats = GetAllCheats(current, current, 20);
            foreach (var cheat in cheats) {
                var saved = path[cheat.Key] - path[current] - cheat.Value;
                if (saved >= 50 && !count.TryAdd(saved, 1)) count[saved]++;
                if (saved >= 100) sum++;
            }
        }

        return sum.ToString();
    }
}