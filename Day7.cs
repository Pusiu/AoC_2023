using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace AoC_2023
{
    public class Day7 : Day
    {
        public Day7() { isTest = false; }

        class Hand
        {
            public char[] cards;
            public enum HandType { FiveOfAKind, FourOfAKind, FullHouse, ThreeOfAKind, TwoPair, OnePair, HighCard };
            public HandType type;

            public Hand(string cards, bool useJokers = false)
            {
                this.cards = cards.ToArray();
                var g = this.cards.GroupBy(x => x).ToDictionary(x => x.Key);

                type = g.Keys.Count switch
                {
                    1 => HandType.FiveOfAKind,
                    2 when g.Any(x => x.Value.Count() == 3) && g.Any(x => x.Value.Count() == 2) => HandType.FullHouse,
                    2 => HandType.FourOfAKind,
                    3 when g.Where(x => x.Value.Count() == 2).Count() == 2 => HandType.TwoPair,
                    3 => HandType.ThreeOfAKind,
                    4 => HandType.OnePair,
                    5 => HandType.HighCard,
                    _ => HandType.HighCard
                };
                if (useJokers)
                {
                    var jokerCount = cards.Contains('J') ? g.First(x => x.Key == 'J').Value.Count() : 0;
                    var groupsWithoutJockers = g.Where(x => x.Key != 'J');
                    type = jokerCount switch
                    {
                        4 => HandType.FiveOfAKind,
                        3 when groupsWithoutJockers.Count() == 1 => HandType.FiveOfAKind,
                        3 when groupsWithoutJockers.Count() == 2 => HandType.FourOfAKind,
                        3 when groupsWithoutJockers.Any(x => x.Value.Count() == 2) => HandType.FullHouse,
                        2 when groupsWithoutJockers.Count() == 1 => HandType.FiveOfAKind,
                        2 when groupsWithoutJockers.Count() == 2 => HandType.FourOfAKind,
                        2 when groupsWithoutJockers.Count() == 3 => HandType.ThreeOfAKind,
                        1 when groupsWithoutJockers.Count() == 1 => HandType.FiveOfAKind,
                        1 when groupsWithoutJockers.Any(x => x.Value.Count() == 3) => HandType.FourOfAKind,
                        1 when groupsWithoutJockers.Count(x => x.Value.Count() == 2) == 2 => HandType.FullHouse,
                        1 when groupsWithoutJockers.Count() == 3 => HandType.ThreeOfAKind,
                        1 when groupsWithoutJockers.Count() == 4 => HandType.OnePair,
                        _ => type
                    };
                }
            }
        }

        readonly List<Hand.HandType> types = Enum.GetValues(typeof(Hand.HandType)).Cast<Hand.HandType>().ToList();
        Func<Hand, Hand, int> HandComparer(bool useJoker)
        {
            return (x, y) =>
            {
                var k = types.IndexOf(y.type) - types.IndexOf(x.type);
                if (k == 0)
                {
                    int toNumber(char c) => c switch
                    {
                        'T' => 10,
                        'J' when useJoker => 1,
                        'J' => 11,
                        'Q' => 12,
                        'K' => 13,
                        'A' => 14,
                        _ => int.Parse(c.ToString())
                    };
                    var comparer = Comparer<int>.Default;
                    for (int i = 0; i < x.cards.Length; i++)
                    {
                        var a = toNumber(x.cards[i]);
                        var b = toNumber(y.cards[i]);
                        k = comparer.Compare(a, b);
                        if (k != 0)
                        {
                            break;
                        }
                    }
                }
                return k;
            };
        }

        public override string Part1()
        {
            return input.Split("\n").Select(x => Regex.Match(x, @"(\w+) (\d+)").Groups.Values).Select(x => new
            {
                Hand = new Hand(
                x.ElementAt(1).Value),
                Bid = int.Parse(x.ElementAt(2).Value)
            }).OrderBy(x => x.Hand, Comparer<Hand>.Create((x, y) => HandComparer(false)(x, y)))
            .Select((x, i) => new
            {
                x.Hand,
                x.Bid,
                Rank = i + 1
            }).Sum((a) => a.Bid * a.Rank).ToString();
        }

        public override string Part2()
        {
            return input.Split("\n").Select(x => Regex.Match(x, @"(\w+) (\d+)").Groups.Values).Select(x => new
            {
                Hand = new Hand(
                x.ElementAt(1).Value, true),
                Bid = int.Parse(x.ElementAt(2).Value)
            }).OrderBy(x => x.Hand, Comparer<Hand>.Create((x, y) => HandComparer(true)(x, y)))
            .Select((x, i) => new
            {
                x.Hand,
                x.Bid,
                Rank = i + 1
            }).Sum((a) => a.Bid * a.Rank).ToString();
        }
    }
}