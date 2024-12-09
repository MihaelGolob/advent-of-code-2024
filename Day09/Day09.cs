namespace Day09;

public class Day09 : DayBase
{
    private const string DemoInput = "2333133121414131402";
    
    protected override int DayIndex()
    {
        return 9;
    }

    private List<int> ParseInput(string input)
    {
        var output = new List<int>();
        var id = 0;
        for (var i = 0; i < input.Length; i++)
        {
            if (input[i] == '\n') break;
            
            if (i % 2 == 0) output.AddRange(Enumerable.Repeat(id++, int.Parse(input[i].ToString())));
            else output.AddRange(Enumerable.Repeat(-1, int.Parse(input[i].ToString())));
        }

        return output;
    }

    private List<(int id, int num, bool wasMoved)> ParseInput2(string input)
    {
        var output = new List<(int id, int num, bool wasMoved)>();
        var id = 0;
        for (var i = 0; i < input.Length; i++)
        {
            if (input[i] == '\n') break;
            
            if (i % 2 == 0) output.Add((id++, int.Parse(input[i].ToString()), false));
            else output.Add((-1, int.Parse(input[i].ToString()), false));
        }

        return output;
    }

    public override string RunPart1()
    {
        var diskMap = ParseInput(GetInput());
        int backIndex = diskMap.Count - 1;
        long checkSum = 0;

        for (int i = 0; i <= backIndex; i++)
        {
            if (diskMap[i] == -1)
            {
                diskMap[i] = diskMap[backIndex];
                diskMap[backIndex] = -1;
                do
                {
                    backIndex--;
                } while (diskMap[backIndex] == -1);
            }

            checkSum += i * diskMap[i];
        }

        return checkSum.ToString();
    }

    private ((int id, int num, bool wasMoved) filled, int empty)? FillEmpty(
        List<(int id, int num, bool wasMoved)> diskMap, int numEmpty, int i)
    {
        var backIndex = diskMap.Count - 1;
        do
        {
            if (diskMap[backIndex].id != -1 && !diskMap[backIndex].wasMoved && diskMap[backIndex].num <= numEmpty)
            {
                diskMap[backIndex] = (diskMap[backIndex].id, diskMap[backIndex].num, true);
                return (diskMap[backIndex], numEmpty - diskMap[backIndex].num);
            }

            backIndex--;
        } while (i <= backIndex);

        return null;
    }
    
    public override string RunPart2()
    {
        var diskMap = ParseInput2(GetInput());
        var rearranged = new List<(int id, int num, bool wasMoved)>();

        for (int i = 0; i < diskMap.Count; i++)
        {
            if (diskMap[i].id != -1 && !diskMap[i].wasMoved)
            {
                rearranged.Add(diskMap[i]);
                continue;
            }

            var empty = diskMap[i].num;
            do
            {
                var res = FillEmpty(diskMap, empty, i);
                if (res == null) break;
                empty = res.Value.empty;
                rearranged.Add(res.Value.filled);
            } while (empty > 0);
            rearranged.Add((-1, empty, false));
        }

        long checksum = 0;
        var index = 0;
        for (int i = 0; i < rearranged.Count; i++)
        {
            if (rearranged[i].id == -1)
            {
                index += rearranged[i].num;
                continue;
            }

            for (int j = 0; j < rearranged[i].num; j++)
            {
                checksum += rearranged[i].id * index;
                index++;
            }
        }

        return checksum.ToString();
    }
}