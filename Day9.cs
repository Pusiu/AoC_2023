using System.Text.RegularExpressions;

namespace AoC_2023
{
    public class Day9 : Day
    {
        public Day9() { isTest = false; }
        public override string Part1()
        {
            return Solve(false).ToString();
        }

        long Solve(bool previous = false)
        {
            return input
                .Split("\n")
                .Select(x => Regex.Matches(x, @"-?\d+").Select(x => long.Parse(x.Value)).ToList())
                .Select(line =>
                {
                    var subSequences = new List<List<long>> { line };
                    while (line.Any(x => x != 0))
                    {
                        var newSeq = line.Skip(1).Select((x, i) => x - line[i]).ToList();
                        subSequences.Add(newSeq);
                        line = newSeq;
                    }
                    subSequences.Last().Insert(previous ? 0 : subSequences.Last().Count, 0);
                    for (int i = subSequences.Count - 2; i >= 0; i--)
                    {
                        subSequences[i].Insert(previous ? 0 : subSequences[i].Count,
                            (previous ? subSequences[i].First() : subSequences[i].Last()) +
                            (previous ? -1 : 1) *
                            (previous ? subSequences[i + 1].First() : subSequences[i + 1].Last()));
                    }
                    return previous ? subSequences.First().First() : subSequences.First().Last();
                })
                .Sum();
        }

        public override string Part2()
        {
            return Solve(true).ToString();
        }
    }
}