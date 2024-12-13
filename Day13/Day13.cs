namespace Day13;

struct ClawMachine((long x, long y) buttonA, (long x, long y) buttonB, (long x, long y) prize) {
    public (long x, long y) ButtonA = buttonA;
    public (long x, long y) ButtonB = buttonB;
    public (long x, long y) Prize = prize;

    public bool IsAtPrize(long buttonAPresses, long buttonBPresses) {
        return ButtonA.x * buttonAPresses + ButtonB.x * buttonBPresses == Prize.x
               && ButtonA.y * buttonAPresses + ButtonB.y * buttonBPresses == Prize.y;
    }
}

public class Day13 : DayBase {
    private const string DemoInput = "Button A: X+94, Y+34\nButton B: X+22, Y+67\nPrize: X=8400, Y=5400\n\nButton A: X+26, Y+66\nButton B: X+67, Y+21\nPrize: X=12748, Y=12176\n\nButton A: X+17, Y+86\nButton B: X+84, Y+37\nPrize: X=7870, Y=6450\n\nButton A: X+69, Y+23\nButton B: X+27, Y+71\nPrize: X=18641, Y=10279\n";
    
    protected override int DayIndex() {
        return 13;
    }

    private List<ClawMachine> ParseInput(string input, long prizeOffset) {
        var result = new List<ClawMachine>();
        var machines = input.Split("\n\n");

        foreach (var m in machines) {
            var lines = m.Split("\n");
            lines[0] = lines[0].Replace(",", string.Empty);
            lines[1] = lines[1].Replace(",", string.Empty);
            lines[2] = lines[2].Replace(",", string.Empty);
            var buttonALine = lines[0].Split(" ");
            var buttonA = (long.Parse(buttonALine[2].Substring(2)), long.Parse(buttonALine[3].Substring(2)));
            var buttonBLine = lines[1].Split(" ");
            var buttonB = (long.Parse(buttonBLine[2].Substring(2)), long.Parse(buttonBLine[3].Substring(2)));
            var prizeLine = lines[2].Split(" ");
            var prize = (long.Parse(prizeLine[1].Substring(2)) + prizeOffset, long.Parse(prizeLine[2].Substring(2)) + prizeOffset);
            
            result.Add(new ClawMachine(buttonA, buttonB, prize));
        }

        return result;
    }

    private (long buttonA, long buttonB) FindButtonPresses(ClawMachine machine) {
        (long buttonA, long buttonB) result = (-1, -1);

        for (long i = 0; i <= 100; i++) {
            for (long j = 0; j <= 100; j++) {
                if (machine.IsAtPrize(i, j))
                    return (i, j);
            }
        }

        return result;
    }

    private (long buttonA, long buttonB) FindButtonPressesAlgebraic(ClawMachine machine) {
        double a = (machine.Prize.y - machine.ButtonB.y * ((double)machine.Prize.x / machine.ButtonB.x)) /
            (machine.ButtonA.y - machine.ButtonB.y * ((double)machine.ButtonA.x / machine.ButtonB.x));
        double b = (double)machine.Prize.x / machine.ButtonB.x - ((double)machine.ButtonA.x / machine.ButtonB.x) * a;

        if (!(a % 1 < 0.001 || a % 1 > 0.99) || !(b % 1 < 0.001 || b % 1 > 0.99)) return (-1, -1);
        return ((long)Math.Round(a), (long)Math.Round(b));
    }

    public override string RunPart1(System.Diagnostics.Stopwatch stopwatch) {
        var machines = ParseInput(GetInput(stopwatch), 0);

        long cost = 0;
        foreach (var m in machines) {
            var buttons = FindButtonPresses(m);
            if (buttons.buttonA == -1 && buttons.buttonB == -1) continue;
            
            var addCost = buttons.buttonA * 3 + buttons.buttonB;
            // Console.WriteLine($"Presses for prize {count}: {buttons.buttonA} & {buttons.buttonB} -> cost: {addCost}");
            cost += addCost;
        }
        
        return cost.ToString();
    }

    public override string RunPart2(System.Diagnostics.Stopwatch stopwatch) {
        var machines = ParseInput(GetInput(stopwatch), 10000000000000);

        long cost = 0;
        foreach (var m in machines) {
            var buttons = FindButtonPressesAlgebraic(m);
            if (buttons.buttonA < 0 || buttons.buttonB < 0) continue;
            
            var addCost = buttons.buttonA * 3 + buttons.buttonB;
            // Console.WriteLine($"Presses for prize {count}: {buttons.buttonA} & {buttons.buttonB} -> cost: {addCost}");
            cost += addCost;
        }
        
        return cost.ToString();
    }
}