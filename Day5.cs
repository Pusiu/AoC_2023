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
                var mapperId=0;
                var currentValue = seed;
                do
                {
                    var mappedValue = currentValue;
                    var k = mappings.Skip(mapperId).First().ranges.FirstOrDefault(x => currentValue >= x.sourceStart && currentValue <= x.sourceStart + x.length);
                    if (k != null)
                    {
                        var diff = currentValue - k.sourceStart;
                        mappedValue = k.destinationStart + diff;
                    }                
                    currentValue = mappedValue;
                    mapperId++;
                } while (mapperId < mappings.Count);
                
                
                if (currentValue < lowest)
                    lowest = currentValue;
            }

            return lowest.ToString();
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
            var split = input.Split("\n");
            var seeds = Regex.Matches(split.First(), @"\d+").Select(x => long.Parse(x.Value)).ToList();

            List<Mapping> mappings = CreateMap(split.Skip(1).ToList());
            List<Range> seedRanges = new();
            mappings.Add(new Mapping() { ranges = seedRanges });
            for (int i = 0; i < seeds.Count; i += 2)
            {
                seedRanges.Add(new Range()
                {
                    destinationStart = seeds[i],
                    length = seeds[i + 1]
                });
            }
            long lowest = -1;
            mappings.Reverse();
            for (long i = 0; i < long.MaxValue; i++)
            {
                var mapperId = 0;
                var currentValue = i;
                do
                {
                    var mappedValue = currentValue;
                    var k = mappings.Skip(mapperId).First().ranges.FirstOrDefault(x => currentValue >= x.destinationStart && currentValue <= x.destinationStart + x.length);
                    if (k != null)
                    {
                        var diff = currentValue - k.destinationStart;
                        mappedValue = k.sourceStart + diff;
                    }
                    currentValue = mappedValue;
                    mapperId++;
                } while (mapperId < mappings.Count);
                var f = seedRanges.FirstOrDefault(x => x.destinationStart <= currentValue && currentValue < x.destinationStart + x.length);
                if (f != null)
                {
                    lowest = i;
                    break;
                }
            }

            return lowest.ToString();
        }
    }
}