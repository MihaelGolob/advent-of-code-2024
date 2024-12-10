namespace Day10;

public record Point(int x, int y);

public class Day10 : DayBase {

    private const string DemoInput = "89010123\n78121874\n87430965\n96549874\n45678903\n32019012\n01329801\n10456732";
    
    protected override int DayIndex() {
        return 10;
    }

    private List<List<int>> ParseInput(string input) {
        var output = new List<List<int>>();
        var lines = input.Split('\n');
        
        foreach (var line in lines) {
            if (line == "") continue;
            
            var tmp = new List<int>();
            foreach (var c in line) {
                if (c == '.') tmp.Add(11);
                else tmp.Add(int.Parse(c.ToString()));
            }
            output.Add(tmp);
        }

        return output;
    }

    private bool IsInBounds(List<List<int>> grid, Point p) {
        return p.x >= 0 && p.x < grid[0].Count && p.y >= 0 && p.y < grid.Count;
    }

    private int FindPaths(List<List<int>> grid, Point start, List<Point> legalMoves) {
        var queue = new Queue<Point>();
        var visited = new HashSet<Point>();
        queue.Enqueue(start);
        
        var count = 0;

        while (queue.Count > 0) {
            var current = queue.Dequeue();
            if (!visited.Contains(current) && grid[current.y][current.x] == 9) {
                count++;
                visited.Add(current);
                continue;
            }
            visited.Add(current);
            
            foreach (var move in legalMoves) {
                var nextPoint = new Point(current.x + move.x, current.y + move.y);
                if (IsInBounds(grid, nextPoint) 
                    && grid[nextPoint.y][nextPoint.x] - grid[current.y][current.x] == 1 
                    && !visited.Contains(nextPoint)) 
                    queue.Enqueue(nextPoint);
            }
        }

        return count;
    }
    
    private int FindPathRating(List<List<int>> grid, Point start, List<Point> legalMoves) {
        var queue = new Queue<Point>();
        var visited = new HashSet<Point>();
        queue.Enqueue(start);
        
        var count = 0;

        while (queue.Count > 0) {
            var current = queue.Dequeue();
            if (grid[current.y][current.x] == 9) {
                count++;
                continue;
            }
            visited.Add(current);
            
            foreach (var move in legalMoves) {
                var nextPoint = new Point(current.x + move.x, current.y + move.y);
                if (IsInBounds(grid, nextPoint) 
                    && grid[nextPoint.y][nextPoint.x] - grid[current.y][current.x] == 1 
                    && !visited.Contains(nextPoint)) 
                    queue.Enqueue(nextPoint);
            }
        }

        return count;
    }

    public override string RunPart1() {
        var grid = ParseInput(GetInput());
        var legalMoves = new List<Point>([new Point(-1, 0), new Point(1, 0), new Point(0, 1), new Point(0, -1)]);

        var sum = 0;
        for (var i = 0; i < grid.Count; i++) {
            for (var j = 0; j < grid[0].Count; j++) {
                if (grid[i][j] == 0) {
                    var count = FindPaths(grid, new Point(j, i), legalMoves);
                    sum += count;
                }
            }
        }
        
        return sum.ToString();
    }

    public override string RunPart2() {
        var grid = ParseInput(GetInput());
        var legalMoves = new List<Point>([new Point(-1, 0), new Point(1, 0), new Point(0, 1), new Point(0, -1)]);

        var sum = 0;
        for (var i = 0; i < grid.Count; i++) {
            for (var j = 0; j < grid[0].Count; j++) {
                if (grid[i][j] == 0) {
                    var count = FindPathRating(grid, new Point(j, i), legalMoves);
                    sum += count;
                }
            }
        }
        
        return sum.ToString();
    }
}