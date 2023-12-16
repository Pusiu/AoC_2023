using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace AoC_2023
{
    public class Day16 : Day
    {
        public Day16() { isTest = false; }

        class Tile
        {
            public bool energized = false;
            public char symbol;
        }

        public override string Part1()
        {         
            return GetEnergizedTileCount(0,0,1,0).ToString();
        }

        //Not proud of this, takes way too long but it works
        int GetEnergizedTileCount(int initialX, int initialY, int dirx, int diry)
        {
                        var map = input.Split("\n").Select(line => line.Select(x => new Tile()
            {
                symbol = x
            }).ToArray()).ToArray();

            var startRays = GetRays(map,initialX,initialY, dirx, diry).Select(x => (x.x, x.y, x.dirx, x.diry, 800));
            Queue<(int x, int y, int dirx, int diry, int energy)> q = new();
            foreach (var r in startRays)
            q.Enqueue(r);

            while (q.Count > 0)
            {
                var ray = q.Dequeue();
                if (ray.x >= map.Length || ray.x < 0 || ray.y >= map[0].Length || ray.y < 0) continue;

                if (ray.energy <= 0) continue;

                var thisTile = map[ray.y][ray.x];
                thisTile.energized = true;

                var nextX = ray.x+ray.dirx;
                var nextY = ray.y+ray.diry;
                if (nextX < 0 || nextX >= map.Length || nextY < 0 || nextY >= map[0].Length) continue;
                
                var newRays = GetRays(map, nextX, nextY, ray.dirx, ray.diry);

                foreach (var newRay in newRays)
                    q.Enqueue((newRay.x, newRay.y, newRay.dirx, newRay.diry, ray.energy-1));
            }

            // for (int y = 0; y < map.Length; y++)
            // {
            //     for (int x = 0; x < map[0].Length; x++)
            //         Console.Write(map[y][x].energized ? "#" : map[y][x].symbol);
            //     Console.WriteLine();
            // }

            return map.Sum(x => x.Count(x => x.energized));
        }

        List<(int x, int y, int dirx, int diry)> GetRays(Tile[][] map, int x, int y, int dirx, int diry)
        {
var tile = map[y][x];
                                

                List<(int x, int y, int dirx, int diry)> rays =  tile.symbol switch
                {
                    '.' => new() { (x, y, dirx, diry) },
                    '|' when (dirx == 0) => new() { (x, y, 0, diry) },
                    '|' when (dirx != 0) => new() {
                        (x, y, 0, 1),
                        (x, y, 0, -1)
                    },
                    '-' when (diry == 0) => new() {
                        (x, y, dirx, 0)
                    },
                    '-' when (diry != 0) => new() {
                        (x, y, 1, 0),
                        (x, y, -1, 0)
                    },
                    //going from left, reflect upwards
                    '/' when (dirx > 0) => new() {
                        (x, y, 0, -1)
                    },
                    //going from right, reflect downwards
                    '/' when (dirx < 0) => new() {
                        (x, y, 0, 1)
                    },
                    //going from down, reflect right
                    '/' when (diry < 0) => new() {
                        (x, y, 1, 0)
                    },
                    //going from up, reflect left
                    '/' when (diry > 0) => new() {
                        (x, y, -1, 0)
                    },
                    //going from right, reflect upwards
                    '\\' when (dirx < 0) => new() {
                        (x, y, 0, -1)
                    },
                    //going from left, reflect downwards
                    '\\' when (dirx > 0) => new() {
                        (x, y, 0, 1)
                    },
                    //going from up, reflect right
                    '\\' when (diry > 0) => new() {
                        (x, y, 1, 0)
                    },
                    //going from down, reflect left
                    '\\' when (diry < 0) => new() {
                        (x, y, -1, 0)
                    },
                    _ => new()
                };

            return rays.ToList();
        }

        public override string Part2()
        {
            var split = input.Split("\n");
            var positions = split.SelectMany((line, y) => line.Select((c, x) => new {
                x,y
            })).Where(x=> x.x == 0 || x.x == split[0].Length -1 || x.y == 0 || x.y == split.Length).Select(x => new {
                x.x,
                x.y,
                dirx = x.x == 0 ? 1 : x.x == split[0].Length -1 ? -1 : 0,
                diry = x.y == 0 ? 1 : x.y == split.Length - 1 ? -1 : 0,
            }).ToList();

            int max=0;

            Parallel.For(0, positions.Count, (i) => {
                var p = positions[i];
                var m = GetEnergizedTileCount(p.x, p.y, p.dirx, p.diry);
                if (max < m)
                    max = m;
            });

            return max.ToString();
        }
    }
}