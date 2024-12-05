namespace Day05;

public class Day05 : DayBase
{
    private const string DemoInput =
        "47|53\n97|13\n97|61\n97|47\n75|29\n61|13\n75|53\n29|13\n97|29\n53|29\n61|53\n97|53\n61|29\n47|13\n75|47\n97|75\n47|61\n75|61\n47|29\n75|13\n53|13\n\n75,47,61,53,29\n97,61,53,29,13\n75,29,13\n75,97,47,61,53\n61,13,29\n97,13,75,29,47";
    
    protected override int DayIndex()
    {
        return 5;
    }

    (Dictionary<int, List<int>> rules, List<List<int>> updates) ParseInput(string input)
    {
        var rules = new Dictionary<int, List<int>>();
        var updates = new List<List<int>>();

        var lines = input.Split('\n');
        foreach (var line in lines)
        {
            if (line == "") continue;

            if (line.Contains('|'))
            {
                var rule = line.Split('|');
                var key = int.Parse(rule[0]);
                if (rules.ContainsKey(key))
                {
                    rules[key].Add(int.Parse(rule[1]));
                }
                else
                {
                    rules.Add(key, [int.Parse(rule[1])]);
                }
            }
            else
            {
                var update = line.Split(',').Select(int.Parse).ToList();
                updates.Add(update);
            }
        }

        return (rules, updates);
    }

    private bool CheckOrder(List<int> update, int index, List<int> rules)
    {
        for (var i = index; i >= 0; i--)
        {
            foreach (var rule in rules)
            {
                if (update[i] == rule)
                {
                    return false;
                }
            }
        }

        return true;
    }
    
    private bool CheckOrderAndFix(List<int> update, int index, List<int> rules)
    {
        for (var i = index; i >= 0; i--)
        {
            foreach (var rule in rules)
            {
                if (update[i] == rule)
                {
                    (update[i], update[index+1]) = (update[index+1], update[i]);
                    
                    return false;
                }
            }
        }

        return true;
    }
    
    public override string RunPart1()
    {
        var (rules, updates) = ParseInput(GetInput());

        var orderedUpdates = new List<int>();
        for (int i = 0; i < updates.Count; i++)
        {
            var update = updates[i];
            var isOrdered = true;
            for (int j = update.Count - 1; j > 0; j--)
            {
                if (!rules.ContainsKey(update[j]))
                {
                    continue;
                }
                var r = rules[update[j]];
                isOrdered &= CheckOrder(update, j-1, r);
            }
            
            if (isOrdered) orderedUpdates.Add(i);
        }

        var sum = 0;
        foreach (var i in orderedUpdates)
        {
            sum += updates[i][updates[i].Count / 2];
        }

        return sum.ToString();
    }

    public override string RunPart2()
    {
        var (rules, updates) = ParseInput(GetInput());

        var orderedUpdates = new List<int>();
        for (int i = 0; i < updates.Count; i++)
        {
            var update = updates[i];
            var isOrdered = true;
            for (int j = update.Count - 1; j > 0; j--)
            {
                if (!rules.ContainsKey(update[j]))
                {
                    continue;
                }
                var r = rules[update[j]];
                if (!CheckOrderAndFix(update, j - 1, r))
                {
                    isOrdered = false;
                    j++;
                }
            }
            
            if (!isOrdered) orderedUpdates.Add(i);
        }

        var sum = 0;
        foreach (var i in orderedUpdates)
        {
            sum += updates[i][updates[i].Count / 2];
        }

        return sum.ToString();
    }

}