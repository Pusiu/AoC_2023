using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace AoC_2023
{
    public class Day8 : Day
    {
        record Node(string NodeName, string OnLeft, string OnRight);
        public override string Part1()
        {
            var m = input.Split("\n");
            var moves = m.First();

            var nodes = m.Skip(1).Select(x => Regex.Matches(x, @"(\w+)").Select(x => x.Value)).Where(x => x.Any())
            .Select(x => new Node(x.First(),x.ElementAt(1),x.Last()))
            .ToDictionary(x => x.NodeName);
            return GetMoveCount(nodes["AAA"], nodes, moves).ToString();
        }

        long GreatestCommonDivisor(long a, long b)
        {
            if (b == 0) return a;
            return GreatestCommonDivisor(b, a % b);
        }

        long LeastCommonMultiple(long a, long b)
        {
            return a * b / GreatestCommonDivisor(a, b);
        }

        long GetMoveCount(Node node, Dictionary<string, Node> nodes, string moves)
        {
            long moveCount = 0;
            var currentNode = node;
            while (!currentNode.NodeName.EndsWith("Z"))
            {
                int k = (int)(moveCount % moves.Length);
                currentNode = nodes[moves[k] == 'L' ? currentNode.OnLeft : currentNode.OnRight];
                moveCount++;
            }
            return moveCount;
        }

        public override string Part2()
        {
            var m = input.Split("\n");
            var moves = m.First();
            var nodes = m.Skip(1).Select(x => Regex.Matches(x, @"(\w+)").Select(x => x.Value)).Where(x => x.Any())
            .Select(x => new Node(x.First(),x.ElementAt(1),x.Last())).ToDictionary(x => x.NodeName);

            return nodes.Where(x => x.Key.EndsWith("A"))
            .Select(x => x.Value)
            .Select(x => GetMoveCount(x, nodes, moves))
            .Aggregate(LeastCommonMultiple).ToString();
        }
    }
}