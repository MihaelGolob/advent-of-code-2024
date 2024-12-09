var day = new Day10.Day10(); 

var watch1 = System.Diagnostics.Stopwatch.StartNew();
var result1 = day.RunPart1();
watch1.Stop();

var watch2 = System.Diagnostics.Stopwatch.StartNew();
var result2 = day.RunPart2();
watch2.Stop();

Console.WriteLine($"Part1 result is: {result1} ({watch1.ElapsedMilliseconds}ms)");
Console.WriteLine($"Part2 result is: {result2} ({watch2.ElapsedMilliseconds}ms)");