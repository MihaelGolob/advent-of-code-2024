public abstract class DayBase
{
    protected abstract int DayIndex();
    public abstract string RunPart1(System.Diagnostics.Stopwatch stopwatch);
    public abstract string RunPart2(System.Diagnostics.Stopwatch stopwatch);
    
    protected string GetInput(System.Diagnostics.Stopwatch stopwatch) {
        stopwatch.Stop();
        var url = $"https://adventofcode.com/2024/day/{DayIndex().ToString()}/input";

        var sessionIdFile = File.ReadAllLines("secrets.txt");
        var session = $"session={sessionIdFile[0]}";
        var response = String.Empty;

        using (HttpClient client = new HttpClient()) {
            try {
                client.DefaultRequestHeaders.Add("Cookie", session);
                response = client.GetStringAsync(url).Result;
            } catch (HttpRequestException e)
            {
                Console.WriteLine($"Error occured: {e}"); 
            }
        }

        stopwatch.Start();
        return response;
    }
}