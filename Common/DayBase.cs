public abstract class DayBase
{
    public abstract string RunPart1();
    public abstract string RunPart2();
    
    protected abstract int DayIndex();
    
    protected string GetInput() {
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

        return response;
    }
}