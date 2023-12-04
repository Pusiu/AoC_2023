using System.Text.RegularExpressions;
using System.Xml.Schema;

namespace AoC_2023
{
    public class Day4 : Day
    {
        public override string Part1()
        {
            return input.Split("\n", StringSplitOptions.RemoveEmptyEntries)
            .Select(x => Regex.Match(x, @"Card *(\d+): *((\d+ *)+)\| *((\d+ *)+)"))
            .Select(x => x.Groups.Values)
            .Select(x => x.Where(x => x.Captures.Count > 1))
            .Where(x => x.Count() > 1)
            .Select(x =>                         
                x.First().Captures.Select(x => int.Parse(x.Value))
                .Intersect(x.Last().Captures.Select(x => int.Parse(x.Value)))
                .Select((x, i) => 1 * i)
            )
            .Select(x => !x.Any() ? 0 : Math.Pow(2, x.LastOrDefault()))
            .Sum()
            .ToString();
        }

        class Card
        {
            public int commonNumbersCount;
            public int cardCount = 1;
        }
        public override string Part2()
        {

            var startDeck = input.Split("\n", StringSplitOptions.RemoveEmptyEntries)
            .Select(x => Regex.Match(x, @"Card *(\d+): *((\d+ *)+)\| *((\d+ *)+)"))
            .Select(x => x.Groups.Values)
                        .Select(x => x.Where(x => x.Captures.Count > 1))
                        .Where(x => x.Count() > 1)
                        .Select(x => new Card
                        {
                            commonNumbersCount = x.First().Captures.Select(x => int.Parse(x.Value))
                                                .Intersect(x.Last().Captures.Select(x => int.Parse(x.Value)))
                                                .Count(),
                        });

            return startDeck.SelectMany((c, i) => Enumerable.Range(1, c.commonNumbersCount)
                                                            .Select(j => new { i, j }))
                            .Aggregate(startDeck.ToList(), (deck, pair) =>
                            {
                                deck[pair.i + pair.j].cardCount += 1 * deck[pair.i].cardCount;
                                return deck;
                            })
                            .Sum(x => x.cardCount)
                            .ToString();
        }
    }
}