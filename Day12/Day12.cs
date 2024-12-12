namespace Day12;

public class Day12 : DayBase {
    private const string DemoInput1 = "AAAA\nBBCD\nBBCC\nEEEC";
    private const string DemoInput2 = "OOOOO\nOXOXO\nOOOOO\nOXOXO\nOOOOO\n";
    private const string DemoInput3 = "RRRRIICCFF\nRRRRIICCCF\nVVRRRCCFFF\nVVRCCCJFFF\nVVVVCJJCFE\nVVIVCCJJEE\nVVIIICJJEE\nMIIIIIJJEE\nMIIISIJEEE\nMMMISSJEEE";
    
    protected override int DayIndex() {
        return 12;
    }

    private List<List<char>> ParseInput(string input) {
        var lines = input.Split("\n");
        var output = new List<List<char>>();

        foreach (var line in lines) {
            if (line == "") continue;
            output.Add(line.ToList());
        }

        return output;
    }

    private bool IsInBounds(List<List<char>> map, int x, int y) {
        return x >= 0 && x < map[0].Count && y >= 0 && y < map.Count;
    }

    private List<(int x, int y)> DetectRegionFromPoint(List<List<char>> map, HashSet<(int x, int y)> visited, (int x, int y) startPoint) {
        var region = new List<(int x, int y)>();
        var queue = new Queue<(int x, int y)>();
        var moves = new List<(int x, int y)>([(-1, 0), (0, 1), (1, 0), (0, -1)]);
        var plant = map[startPoint.y][startPoint.x];
        
        queue.Enqueue(startPoint);
        region.Add(startPoint);
        while (queue.Count > 0) {
            var point = queue.Dequeue();
            visited.Add(point);

            foreach (var move in moves) {
                (int x, int y) nextPoint = (point.x + move.x, point.y + move.y);
                if (!visited.Contains(nextPoint) && IsInBounds(map, nextPoint.x, nextPoint.y) && map[nextPoint.y][nextPoint.x] == plant) {
                    queue.Enqueue(nextPoint);
                    region.Add(nextPoint);
                    visited.Add(nextPoint);
                }
            }
        }

        return region;
    }

    private List<List<(int x, int y)>> DetectRegions(List<List<char>> map) {
        var output = new List<List<(int x, int y)>>();
        var visited = new HashSet<(int x, int y)>();

        for (int i = 0; i < map.Count; i++) {
            for (int j = 0; j < map[0].Count; j++) {
                if (!visited.Contains((j, i))) output.Add(DetectRegionFromPoint(map, visited, (j, i)));
            }
        }

        return output;
    }

    private long CalculateRegionArea(List<(int x, int y)> region) {
        return region.Count;
    }

    private long CalculateRegionPerimeter(List<List<char>> map, List<(int x, int y)> region) {
        long count = 0;
        List<(int x, int y)> moves = new List<(int x, int y)>([(-1, 0), (0, 1), (1, 0), (0, -1)]);

        foreach (var plant in region) {
            foreach (var move in moves) {
                (int x, int y) neighbor = (plant.x + move.x, plant.y + move.y);
                if (!IsInBounds(map, neighbor.x, neighbor.y) || map[neighbor.y][neighbor.x] != map[plant.y][plant.x])
                    count++;
            }
        }

        return count;
    }
    
    private long CalculateRegionSides(List<List<char>> map, List<(int x, int y)> region) {
        long count = 0;
        var regionPlot = map[region[0].y][region[0].x];

        bool IsInRegion(int x, int y) {
            return IsInBounds(map, x, y) && map[y][x] == regionPlot;
        }

        foreach (var plot in region) {
            // outer corners
            if (!IsInRegion(plot.x - 1, plot.y) && !IsInRegion(plot.x, plot.y - 1)) count++;
            if (!IsInRegion(plot.x + 1, plot.y) && !IsInRegion(plot.x, plot.y - 1)) count++;
            if (!IsInRegion(plot.x - 1, plot.y) && !IsInRegion(plot.x, plot.y + 1)) count++;
            if (!IsInRegion(plot.x + 1, plot.y) && !IsInRegion(plot.x, plot.y + 1)) count++;
            // inner corners
            if (IsInRegion(plot.x - 1, plot.y) && IsInRegion(plot.x, plot.y - 1) && !IsInRegion(plot.x - 1, plot.y - 1)) count++;
            if (IsInRegion(plot.x + 1, plot.y) && IsInRegion(plot.x, plot.y - 1) && !IsInRegion(plot.x + 1, plot.y - 1)) count++;
            if (IsInRegion(plot.x - 1, plot.y) && IsInRegion(plot.x, plot.y + 1) && !IsInRegion(plot.x - 1, plot.y + 1)) count++;
            if (IsInRegion(plot.x + 1, plot.y) && IsInRegion(plot.x, plot.y + 1) && !IsInRegion(plot.x + 1, plot.y + 1)) count++;
        }
        
        return count;
    }
    
    public override string RunPart1() {
        var map = ParseInput(GetInput());
        var regions = DetectRegions(map);

        long cost = 0;
        for (var i = 0; i < regions.Count; i++) {
            var region = regions[i];
            var area = CalculateRegionArea(region);
            var perimeter = CalculateRegionPerimeter(map, region);
            // Console.WriteLine($"region {i}/{regions.Count}: area: {area}, perimeter: {perimeter}");
            cost += area * perimeter;
        }
        
        return cost.ToString();
    }

    public override string RunPart2() {
        var map = ParseInput(GetInput());
        var regions = DetectRegions(map);

        long cost = 0;
        for (var i = 0; i < regions.Count; i++) {
            var region = regions[i];
            var area = CalculateRegionArea(region);
            var sides = CalculateRegionSides(map, region);
            // Console.WriteLine($"region {i}/{regions.Count}: area: {area}, sides: {sides}");
            cost += area * sides;
        }
        
        return cost.ToString();
    }
}