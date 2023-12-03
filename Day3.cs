using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace AoC_2023
{
    public class Day3 : Day
    {
        public override string Part1()
        {
            var map = input.Split("\n").Select(x => x.ToArray()).ToArray();

            bool isSymbol (char c) => !char.IsDigit(c) && c != '.';

            return map.Select((x, i) => new
            {
                Matches = Regex.Matches(string.Join(null, map[i]), @"\d+").Select(x => new
                {
                    Value = int.Parse(x.Value),
                    Range = Enumerable.Range(x.Index, x.Length)
                }),
                YIndex = i
            })
            .SelectMany(line => line.Matches.Where(match => match.Range.Any(xIndex => map switch
            {
                var k when line.YIndex < k.Length && xIndex + 1 < k[line.YIndex].Length && isSymbol(k[line.YIndex][xIndex + 1]) => true,
                var k when line.YIndex < k.Length && xIndex - 1 >= 0 && isSymbol(k[line.YIndex][xIndex - 1]) => true,
                var k when line.YIndex + 1 < k.Length && isSymbol(k[line.YIndex + 1][xIndex]) => true,
                var k when line.YIndex - 1 >= 0 && isSymbol(k[line.YIndex - 1][xIndex]) => true,

                var k when line.YIndex + 1 < k.Length && xIndex + 1 < k[line.YIndex].Length && isSymbol(k[line.YIndex + 1][xIndex + 1]) => true, //lower right
                var k when line.YIndex + 1 < k.Length && xIndex - 1 >= 0 && isSymbol(k[line.YIndex + 1][xIndex - 1]) => true, //lower left
                var k when line.YIndex - 1 >= 0 && xIndex - 1 >= 0 && isSymbol(k[line.YIndex - 1][xIndex - 1]) => true, //upper left
                var k when line.YIndex - 1 >= 0 && xIndex + 1 < k[line.YIndex].Length && isSymbol(k[line.YIndex - 1][xIndex + 1]) => true, //upper right
                _ => false
            })))
            .Sum(x => x.Value)
            .ToString();
        }

        List<int> GetAdjecentNumbers(char[][] map, int x, int y)
        {
            List<int> numbers = new();
            var dirs = new (int x, int y)[] {
                (1, 1),
                (1, 0),
                (1, -1),
                (0, 1),
                (0, -1),
                (-1, 1),
                (-1, 0),
                (-1, -1),
            };
            var validDirs = dirs.Where(dir => y + dir.y >= 0 && y + dir.y < map.Length && x + dir.x >= 0 && x + dir.x < map[y].Length && char.IsDigit(map[dir.y + y][dir.x + x])).ToList();
            //only dirs where y+1 and y-1 can be a part of the same number
            if (validDirs.Contains((0, 1)))
            {
                validDirs.RemoveAll(x => x.y == 1 && (x.x == -1 || x.x == 1));
            }
            if (validDirs.Contains((0, -1)))
            {
                validDirs.RemoveAll(x => x.y == -1 && (x.x == -1 || x.x == 1));
            }
            foreach (var dir in validDirs)
            {
                StringBuilder sb = new(map[y + dir.y][x + dir.x].ToString());
                var xx = dir.x - 1;
                while (x + xx >= 0) //to left
                {
                    if (char.IsDigit(map[y + dir.y][x + xx]))
                        sb.Insert(0, map[y + dir.y][x + xx]);
                    else
                        break;
                    xx--;
                }
                xx = dir.x + 1;
                while (x + xx < map[y + dir.y].Length) //to right
                {
                    if (char.IsDigit(map[y + dir.y][x + xx]))
                        sb.Append(map[y + dir.y][x + xx]);
                    else
                        break;
                    xx++;
                }
                numbers.Add(int.Parse(sb.ToString()));
            }
            return numbers;
        }

        public override string Part2()
        {
            var map = input.Split("\n").Select(x => x.ToArray()).ToArray();
            return map.Select((x, i) =>
            new
            {
                Index = i,
                Matches = Regex.Matches(string.Join(null, map[i]), @"\*")
            })
            .Where(x => x.Matches.Count > 0)
            .Select(y => y.Matches.Select(x => GetAdjecentNumbers(map, x.Index, y.Index))
                                    .Where(x => x.Count == 2))
                                    .SelectMany(x => x)
                                    .Select(x => x.Aggregate((a, b) => a * b))
            .Sum()
            .ToString();
        }
    }
}