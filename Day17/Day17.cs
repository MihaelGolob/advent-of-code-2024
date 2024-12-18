using System.Diagnostics;
using System.Net.Sockets;

namespace Day17;

public class Day17 : DayBase {
    private const string DemoInput = "Register A: 729\nRegister B: 0\nRegister C: 0\n\nProgram: 0,1,5,4,3,0";
    private const string DemoInput2 = "Register A: 2024\nRegister B: 0\nRegister C: 0\n\nProgram: 0,3,5,4,3,0";
    
    protected override int DayIndex() {
        return 17;
    }
    
    private (List<long> registers, List<long> program) ParseInput(string input) {
        var split = input.Split("\n\n");
        var registers = new List<long>();
        var program = new List<long>();

        foreach (var line in split[0].Split("\n")) {
            var splitLine = line.Split(": ");
            registers.Add(long.Parse(splitLine[1]));
        }
        
        foreach (var line in split[1].Substring(9).Split(",")) {
            program.Add(long.Parse(line));
        }

        return (registers, program);
    }

    private List<long> SimulateProgram(List<long> registers, List<long> program, bool terminate = false) {
        var a = registers[0];
        var b = registers[1];
        var c = registers[2];

        var output = new List<long>();

        long GetComboOperand(long operand) {
            if (operand >= 0 && operand <= 3) return operand;
            if (operand == 4) return a;
            if (operand == 5) return b;
            if (operand == 6) return c;
            return -1;    
        }

        var instPoint = 0;
        while (instPoint < program.Count) {
            if (terminate && output.Count > program.Count) break;
            if (a < 0 || b < 0 || c < 0) Console.WriteLine("Overflow!");
             
            var instruction = program[instPoint];
            var operand = program[instPoint + 1];
            switch (instruction) {
                case 0:
                    a = (long) (a / (Math.Pow(2, GetComboOperand(operand))));
                    break;
                case 1:
                    b = b ^ operand;
                    break;
                case 2:
                    b = GetComboOperand(operand) % 8;
                    break;
                case 3:
                    if (a != 0) {
                        instPoint = (int)operand;
                        continue;
                    }
                    break;
                case 4:
                    b ^= c;
                    break;
                case 5:
                    output.Add(GetComboOperand(operand) % 8);
                    break;
                case 6:
                    b = (long) (a / (Math.Pow(2, GetComboOperand(operand))));
                    break;
                case 7:
                    c = (long) (a / (Math.Pow(2, GetComboOperand(operand))));
                    break;
            }

            instPoint += 2;
        }

        return output;
    }

    public override string RunPart1(Stopwatch stopwatch) {
        var (registers, program) = ParseInput(GetInput(stopwatch));
        var result = SimulateProgram(registers,program);

        return string.Join(",", result);
    }

    public override string RunPart2(Stopwatch stopwatch) {
        var (registers, program) = ParseInput(DemoInput2);

        List<long> Evaluate(long a) {
            return SimulateProgram([a, 0, 0], program);
        }

        long solution = -1;
        void FindA(long a, long i) {
            var result = Evaluate(a);

            if (result.SequenceEqual(program)) {
                if (solution == -1) solution = a;
                return;
            }
            if (i == 0 || result.SequenceEqual(program.TakeLast((int)i)))
                for (var j = 0; j < 8; j++)
                    FindA(8 * a + j, i + 1);
        }

        FindA(0, 0);
        return solution.ToString();
    }
}