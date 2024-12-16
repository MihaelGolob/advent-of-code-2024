using System.Diagnostics;

namespace Day15;

public class Day15 : DayBase {
    private const string DemoInput2 = "##########\n#..O..O.O#\n#......O.#\n#.OO..O.O#\n#..O@..O.#\n#O#..O...#\n#O..O..O.#\n#.OO.O.OO#\n#....O...#\n##########\n\n<vv>^<v^>v>^vv^v>v<>v^v<v<^vv<<<^><<><>>v<vvv<>^v^>^<<<><<v<<<v^vv^v>^\nvvv<<^>^v^^><<>>><>^<<><^vv^^<>vvv<>><^^v>^>vv<>v<<<<v<^v>^<^^>>>^<v<v\n><>vv>v^v^<>><>>>><^^>vv>v<^^^>>v^v^<^^>v^^>v^<^v>v<>>v^v^<v>v^^<^^vv<\n<<v<^>>^^^^>>>v^<>vvv^><v<<<>^^^vv^<vvv>^>v<^^^^v<>^>vvvv><>>v^<<^^^^^\n^><^><>>><>^^<<^^v>>><^<v>^<vv>>v>>>^v><>^v><<<<v>>v<v<v>vvv>^<><<>^><\n^>><>^v<><^vvv<^^<><v<<<<<><^v<<<><<<^^<v<^^^><^>>^<v^><<<^>>^v<v^v<v^\n>^>>^v>vv>^<<^v<>><<><<v<<v><>v<^vv<<<>^^v^>^^>>><<^v>>v^v><^^>>^<>vv^\n<><^^>^^^<><vvvvv^v<v<<>^v<v>v<<^><<><<><<<^^<<<^<<>><<><^^^>^^<>^>v<>\n^^>vv<^v^v<vv>^<><v<^v>^^^>>>^^vvv^>vvv<>>>^<^>>>>>^<<^v>^vvv<>^<><<v>\nv^^>>><<^^<>>^v^<v^vv<>v^<<>^<^v^v><^<<<><<^<v><v<>vv>>v><v^<vv<>v^<<^";
    private const string DemoInput1 = "########\n#..O.O.#\n##@.O..#\n#...O..#\n#.#.O..#\n#...O..#\n#......#\n########\n\n<^^>>>vv<v>>v<<";
    private const string DemoInput3 = "#######\n#...#.#\n#.....#\n#..OO@#\n#..O..#\n#.....#\n#######\n\n<vv<<^^<<^^";
    
    protected override int DayIndex() {
        return 15;
    }

    private (List<List<char>> map, (int x, int y) position, string moves) ParseInput(string input) {
        var x = input.Split("\n\n");
        var map = new List<List<char>>();
        (int x, int y) position = (-1, -1);

        var rows = x[0].Split("\n");
        for (var i = 0; i < rows.Length; i++) {
            var row = new List<char>();
            for (var j = 0; j < rows[i].Length; j++) {
                if (rows[i][j] == '@') {
                    row.Add('.');
                    position = (j, i);
                }
                else {
                    row.Add(rows[i][j]);
                }
            }
            map.Add(row);
        }

        return (map, position, x[1].Replace("\n", ""));
    }
    
    private (List<List<char>> map, (int x, int y) position, string moves) ParseInput2(string input) {
        var x = input.Split("\n\n");
        var map = new List<List<char>>();
        (int x, int y) position = (-1, -1);

        var rows = x[0].Split("\n");
        for (var i = 0; i < rows.Length; i++) {
            var row = new List<char>();
            for (var j = 0; j < rows[i].Length; j++) {
                switch (rows[i][j]) {
                    case '@':
                        row.Add('.');
                        row.Add('.');
                        position = (2 * j, i);
                        break;
                    case '#':
                        row.Add('#');
                        row.Add('#');
                        break;
                    case 'O':
                        row.Add('[');
                        row.Add(']');
                        break;
                    case '.':
                        row.Add('.');
                        row.Add('.');
                        break;
                }
            }
            map.Add(row);
        }

        return (map, position, x[1].Replace("\n", ""));
    }

    private (int x, int y) GetDirection(char move) {
        if (move == '<') return (-1, 0);
        if (move == '^') return (0, -1);
        if (move == '>') return (1, 0);
        return (0, 1);
    }

    private int CalculateGpsSum(List<List<char>> map) {
        var sum = 0;
        for (int i = 0; i < map.Count; i++) {
            for (int j = 0; j < map[0].Count; j++) {
                if (map[i][j] == 'O' || map[i][j] == '[') sum += 100 * i + j;
            }
        }

        return sum;
    }

    public override string RunPart1(Stopwatch stopwatch) {
        var (map, position, moves) = ParseInput(GetInput(stopwatch));

        foreach (var move in moves) {
            (int x, int y) direction = GetDirection(move);
            if (map[position.y + direction.y][position.x + direction.x] == '.') position = (position.x + direction.x, position.y + direction.y);
            else if (map[position.y + direction.y][position.x + direction.x] == 'O') {
                var boxesToMove = new List<(int x, int y)>();
                (int x, int y) tempPosition = (position.x + direction.x, position.y + direction.y);
                do {
                   boxesToMove.Add(tempPosition);
                   tempPosition = (tempPosition.x + direction.x, tempPosition.y + direction.y);
                } while (map[tempPosition.y][tempPosition.x] == 'O');

                if (map[tempPosition.y][tempPosition.x] == '.') {
                    var last = boxesToMove.Last();
                    map[last.y + direction.y][last.x + direction.x] = 'O';
                    position = (position.x + direction.x, position.y + direction.y);
                    map[position.y][position.x] = '.';
                }
            }
        }

        
        return CalculateGpsSum(map).ToString();
    }

    private bool IsClear(List<List<char>> map, (int x, int y) position, int direction) {
        if (map[position.y][position.x] == '.') return true;
        if (map[position.y][position.x] == '#') return false;
        
        if (map[position.y][position.x] == '[')
            return IsClear(map, (position.x, position.y + direction), direction) && IsClear(map, (position.x + 1, position.y + direction), direction);
        return IsClear(map, (position.x, position.y + direction), direction) && IsClear(map, (position.x - 1, position.y + direction), direction);
    }

    private void MoveBoxes(List<List<char>> map, (int x, int y) position, int direction, char bracket) {
        if (map[position.y][position.x] == '.') {
            map[position.y][position.x] = bracket;
            return;
        }

        if (map[position.y][position.x] == '[') {
            MoveBoxes(map, (position.x, position.y + direction), direction, '[');
            MoveBoxes(map, (position.x + 1, position.y + direction), direction, ']');

            map[position.y][position.x] = bracket;
            map[position.y][position.x + 1] = '.';
        }
        else if (map[position.y][position.x] == ']') {
            MoveBoxes(map, (position.x, position.y + direction), direction, ']');
            MoveBoxes(map, (position.x - 1, position.y + direction), direction, '[');
            
            map[position.y][position.x] = bracket;
            map[position.y][position.x - 1] = '.';
        }
    }

    public override string RunPart2(Stopwatch stopwatch) {
        var (map, position, moves) = ParseInput2(GetInput(stopwatch));

        foreach (var move in moves) {
            (int x, int y) direction = GetDirection(move);
            if (map[position.y + direction.y][position.x + direction.x] == '.') position = (position.x + direction.x, position.y + direction.y);
            else if (map[position.y + direction.y][position.x + direction.x] == '[' || map[position.y + direction.y][position.x + direction.x] == ']') {
                (int x, int y) tempPosition = (position.x + direction.x, position.y + direction.y);
                if (direction.y == 0) {
                    do {
                        tempPosition = (tempPosition.x + direction.x, tempPosition.y + direction.y);
                    } while (map[tempPosition.y][tempPosition.x] == '[' || map[tempPosition.y][tempPosition.x] == ']');

                    if (map[tempPosition.y][tempPosition.x] == '.') {
                        while (tempPosition != position) {
                            map[tempPosition.y][tempPosition.x] = map[tempPosition.y - direction.y][tempPosition.x - direction.x];
                            tempPosition = (tempPosition.x - direction.x, tempPosition.y - direction.y);
                        }
                        position = (position.x + direction.x, position.y + direction.y);
                    }

                    continue;
                }

                if (IsClear(map, tempPosition, direction.y)) {
                    MoveBoxes(map, tempPosition, direction.y, '.');
                    position = (position.x + direction.x, position.y + direction.y);
                }
            }
        }
        
        // map[position.y][position.x] = '@';
        // for (int i = 0; i < map.Count; i++) {
        //     for (int j = 0; j < map[0].Count; j++) {
        //         Console.Write(map[i][j]);
        //     }
        //     Console.WriteLine();
        // }
        // Console.WriteLine();
        // Console.WriteLine();
        // map[position.y][position.x] = '.';

        return CalculateGpsSum(map).ToString();
    }
}