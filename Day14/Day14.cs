using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;

namespace Day14;

public class Day14 : DayBase {
    private const string DemoInput = "p=0,4 v=3,-3\np=6,3 v=-1,-3\np=10,3 v=-1,2\np=2,0 v=2,-1\np=0,0 v=1,3\np=3,0 v=-2,-2\np=7,6 v=-1,-3\np=3,0 v=-1,-2\np=9,3 v=2,3\np=7,3 v=-1,2\np=2,4 v=2,-3\np=9,5 v=-3,-3";
    private const int GridWidth = 101;
    private const int GridHeight = 103;
    
    protected override int DayIndex() {
        return 14;
    }

    private List<((int x, int y) p, (int x, int y) v)> ParseInput(string input) {
        var lines = input.Split('\n');
        var output = new List<((int x, int y) p, (int x, int y) v)>();
        
        foreach (var line in lines) {
            if (line == "") continue;
            
            ((int x, int y) p, (int x, int y) v) robot;
            var x = line.Split(" ");
            var p = x[0].Substring(2);
            var p0 = p.Split(",");
            robot.p = (int.Parse(p0[0]), int.Parse(p0[1]));
            var v = x[1].Substring(2);
            var v0 = v.Split(",");
            robot.v = (int.Parse(v0[0]), int.Parse(v0[1]));
            
            output.Add(robot);
        }

        return output;
    }

    private void SimulateSeconds(List<((int x, int y) p, (int x, int y) v)> robots, int seconds) {
        for (var i = 0; i < robots.Count; i++) {
            var robot = robots[i];
            var newx = (robot.p.x + robot.v.x * seconds) % GridWidth;
            var newy = (robot.p.y + robot.v.y * seconds) % GridHeight;
            robots[i] = ((newx < 0 ? GridWidth + newx : newx, newy < 0 ? GridHeight + newy : newy), robot.v);
        }
    }

    public override string RunPart1(Stopwatch stopwatch) {
        var robots = ParseInput(GetInput(stopwatch));
        SimulateSeconds(robots, 100);

        int[] quadrantSum = [0, 0, 0, 0];
        foreach (var robot in robots) {
            if (robot.p.x < GridWidth / 2 && robot.p.y < GridHeight / 2) quadrantSum[0]++;
            else if (robot.p.x > GridWidth / 2 && robot.p.y < GridHeight / 2) quadrantSum[1]++;
            else if (robot.p.x < GridWidth / 2 && robot.p.y > GridHeight / 2) quadrantSum[2]++;
            else if (robot.p.x > GridWidth / 2 && robot.p.y > GridHeight / 2) quadrantSum[3]++;
        }
        
        return (quadrantSum[0] * quadrantSum[1] * quadrantSum[2] * quadrantSum[3]).ToString();
    }

    public override string RunPart2(Stopwatch stopwatch) {
        var robots = ParseInput(GetInput(stopwatch));

        for (int i = 0; i < 10000; i++) {
            SimulateSeconds(robots, 1);
            var grid = new int[GridWidth, GridHeight];
            
            foreach (var robot in robots) {
                grid[robot.p.x, robot.p.y] = 1;
            }
            
            Bitmap bitmap = new Bitmap(GridWidth, GridHeight);
            for (int y = 0; y < GridHeight; y++)
            {
                for (int x = 0; x < GridWidth; x++)
                {
                    if (grid[x, y] > 0)
                    {
                        bitmap.SetPixel(x, y, Color.Black);
                    }
                    else
                    {
                        bitmap.SetPixel(x, y, Color.White);
                    }
                }
            }

            bitmap.Save($"grid_{i}.png", ImageFormat.Png);
        }
        Console.WriteLine($"File written to: {Directory.GetCurrentDirectory()}");
        
        return "";
    }
}