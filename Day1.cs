using System.Text.RegularExpressions;

namespace AoC_2023
{
    public class Day1 : Day
    {
        public override string Part1()
        {
            var re = new Regex(@"(\d)");
            return input.Split("\n", options: StringSplitOptions.RemoveEmptyEntries)
            .Select(x => re.Matches(x).Select(x => x.Groups.Values.First()))            
            .Select(x => int.Parse(x.First().Value + x.Last().Value))
            .Aggregate((a, b) => a + b)
            .ToString();    
        }

        public override string Part2()
        {
            var re = new Regex(@"(?=(\d|one|two|three|four|five|six|seven|eight|nine))");
            
            return input
            .Split("\n", options: StringSplitOptions.RemoveEmptyEntries)
            .Select(x => re.Matches(x)
                .Select(x => x.Groups.Values.Skip(1).First().Value)
                .Select(x => x switch {
                        "one" => "1",
                        "two" => "2",
                        "three" => "3",
                        "four" => "4",
                        "five" => "5",
                        "six" => "6",
                        "seven" => "7",
                        "eight" => "8",
                        "nine" => "9",
                        _ => x
                })                
                .ToList())
            .Select(x => int.Parse(x.First() + x.Last()))
            .Aggregate((a,b) => a+b)
            .ToString();
        }
    }
}