using System.Text.RegularExpressions;

namespace AoC_2023
{

    public class Day6 : Day
    {
        public Day6() { isTest = false; }
        public override string Part1()
        {
            var split = input.Split("\n").Select(x => Regex.Matches(x, @"\d+").Select(x => int.Parse(x.Value)).ToArray()).ToArray();
            List<int> possibilities = new List<int>();
            for (int i = 0; i < split[0].Length; i++)
            {
                (int raceDuration, int currentRecordDistance) = (split[0][i], split[1][i]);
                var p = 0;
                for (int j = 0; j < raceDuration; j++)
                {
                    var diff = raceDuration - j;
                    var finalPos = j * diff;
                    if (finalPos > currentRecordDistance)
                        p++;
                }
                possibilities.Add(p);
            }

            return possibilities.Aggregate((a, b) => a * b).ToString();
        }

        public override string Part2()
        {
            var split = input.Split("\n").Select(x => Regex.Matches(x, @"\d+").Select(x => int.Parse(x.Value)).ToArray()).ToArray();
            (long raceDuration, long currentRecordDistance) = (long.Parse(string.Join("", split[0])), long.Parse(string.Join("", split[1])));
            var p = 0;
            for (int j = 0; j < raceDuration; j++)
            {
                var diff = raceDuration - j;
                var finalPos = j * diff;
                if (finalPos > currentRecordDistance)
                    p++;
            }
            return p.ToString();
        }
    }
}