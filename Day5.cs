using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace AoC_2023
{
    public class Day5 : Day
    {
        public Day5() { isTest = false; }

        class Mapping
        {
            public string sourceMapName;
            public string destinationMapName;
            public List<Range> ranges = new List<Range>();
        }

        class Range
        {
            public long sourceStart;
            public long destinationStart;
            public long length;
        }

        public override string Part1()
        {
            var split = input.Split("\n");

            var seeds = Regex.Matches(split.First(), @"\d+").Select(x => long.Parse(x.Value));

            List<Mapping> mappings = CreateMap(split.Skip(1).ToList());
            var lowest = long.MaxValue;
            foreach (var seed in seeds)
            {
                var val = Solve(seed, mappings);
                if (val < lowest)
                    lowest = val;
            }

            return lowest.ToString();
        }

        long Solve(long seed, List<Mapping> mappings)
        {
            var sourceName = "seed";
            var currentMapper = mappings.FirstOrDefault(x => x.sourceMapName == sourceName);
            var currentValue = seed;
            // Console.Write($"Seed {seed}, ");
            do
            {
                var mappedValue = currentValue;
                var k = currentMapper.ranges.FirstOrDefault(x => currentValue >= x.sourceStart && currentValue <= x.sourceStart + x.length);
                if (k != null)
                {
                    var diff = currentValue - k.sourceStart;
                    mappedValue = k.destinationStart + diff;
                }
                // Console.Write($"{currentMapper.destinationMapName} {mappedValue}, ");
                currentMapper = mappings.FirstOrDefault(x => x.sourceMapName == currentMapper.destinationMapName);
                currentValue = mappedValue;
            } while (currentMapper != null);
            // Console.WriteLine();

            return currentValue;
        }

        List<Mapping> CreateMap(List<string> lines)
        {
            List<Mapping> mappings = new List<Mapping>();
            Mapping lastMapping = null;
            foreach (var line in lines)
            {
                var mapMatch = Regex.Match(line, @"(\w+)-to-(\w+)");
                if (mapMatch.Success)
                {
                    lastMapping = new Mapping()
                    {
                        sourceMapName = mapMatch.Groups[1].Value,
                        destinationMapName = mapMatch.Groups[2].Value
                    };
                    mappings.Add(lastMapping);
                }
                else
                {
                    var numbers = Regex.Matches(line, @"\d+").Select(x => long.Parse(x.Value)).ToArray();
                    if (numbers.Any())
                    {
                        var r = new Range()
                        {
                            destinationStart = numbers[0],
                            sourceStart = numbers[1],
                            length = numbers[2],
                        };
                        lastMapping.ranges.Add(r);
                    }
                }
            }
            return mappings;
        }

        public override string Part2()
        {
            return base.Part2();
        }
    }
}