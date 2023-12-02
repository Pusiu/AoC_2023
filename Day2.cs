using System.Text.RegularExpressions;

namespace AoC_2023
{
    public class Day2 : Day
    {
        public override string Part1()
        {
            return Solve().sumOfValidGameIds.ToString();
        }

        public override string Part2()
        {
            return Solve().sumOfPower.ToString();
        }

        (int sumOfValidGameIds, int sumOfPower) Solve()
        {
            return new Regex(@"Game (\d+): (.+)").Matches(input)
            .Select(x => new
            {
                GameId = int.Parse(x.Groups[1].Value),
                Rounds = x.Groups[2].Value.Split(";")
                        .Select(x => new Regex(@"(\d+) (\w+)").Matches(x)
                                    .Select(x => new
                                    {
                                        CubeCount = int.Parse(x.Groups[1].Value),
                                        CubeColor = x.Groups[2].Value,
                                        IsValid = x switch
                                        {
                                            var m when m.Groups[2].Value == "red" && int.Parse(m.Groups[1].Value) <= 12 => true,
                                            var m when m.Groups[2].Value == "green" && int.Parse(m.Groups[1].Value) <= 13 => true,
                                            var m when m.Groups[2].Value == "blue" && int.Parse(m.Groups[1].Value) <= 14 => true,
                                            _ => false
                                        }
                                    }))
            })
            .Select(x => new
            {
                x.GameId,
                x.Rounds,
                MinGreens = x.Rounds.Where(x => x.Any(x => x.CubeColor == "green")).Select(x => x.Where(x => x.CubeColor == "green").Select(x => x.CubeCount).First()).Max(),
                MinReds = x.Rounds.Where(x => x.Any(x => x.CubeColor == "red")).Select(x => x.Where(x => x.CubeColor == "red").Select(x => x.CubeCount).First()).Max(),
                MinBlues = x.Rounds.Where(x => x.Any(x => x.CubeColor == "blue")).Select(x => x.Where(x => x.CubeColor == "blue").Select(x => x.CubeCount).First()).Max(),
            })
            .Select(x => new
            {
                x.GameId,
                x.Rounds,
                x.MinReds,
                x.MinGreens,
                x.MinBlues,
                Power = x.MinReds * x.MinGreens * x.MinBlues,
                IsValidGame = x.Rounds.All(x => x.All(x => x.IsValid == true))
            }).Aggregate((gameIdSum: 0, powerSum: 0), (acc, game) => (
                game.IsValidGame ? acc.gameIdSum + game.GameId : acc.gameIdSum,
                acc.powerSum + game.Power
            ));
        }
    }
}