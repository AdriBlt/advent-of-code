using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Advent.Utils;

namespace Advent2020
{
    class Day24: BaseDay2020
    {
        private static string[] TestLines = new string[]
        {
            "sesenwnenenewseeswwswswwnenewsewsw",
            "neeenesenwnwwswnenewnwwsewnenwseswesw",
            "seswneswswsenwwnwse",
            "nwnwneseeswswnenewneswwnewseswneseene",
            "swweswneswnenwsewnwneneseenw",
            "eesenwseswswnenwswnwnwsewwnwsene",
            "sewnenenenesenwsewnenwwwse",
            "wenwwweseeeweswwwnwwe",
            "wsweesenenewnwwnwsenewsenwwsesesenwne",
            "neeswseenwwswnwswswnw",
            "nenwswwsewswnenenewsenwsenwnesesenew",
            "enewnwewneswsewnwswenweswnenwsenwsw",
            "sweneswneswneneenwnewenewwneswswnese",
            "swwesenesewenwneswnwwneseswwne",
            "enesenwswwswneneswsenwnewswseenwsese",
            "wnwnesenesenenwwnenwsewesewsesesew",
            "nenewswnwewswnenesenwnesewesw",
            "eneswnwswnwsenenwnwnwwseeswneewsenese",
            "neswnwewnwnwseenwseesewsenwsweewe",
            "wseweeenwnesenwwwswnew",
        };

        private HexDirection[][] inputs;
        private HashSet<(int x, int y)> turnedTiles;

        public Day24(): base(24) {
            ParseInputs(this.lines);
        }

        public override long QuestionA()
        {
            this.ComputeInitialTiles();
            return this.turnedTiles.Count;
        }

        public override long QuestionB()
        {
            for (int i = 0; i < 100; i++)
            {
                this.FlipTiles();
            }

            return this.turnedTiles.Count;
        }

        private void FlipTiles()
        {
            Dictionary<(int x, int y), int> tilesCount = new Dictionary<(int x, int y), int>();
            foreach((int x, int y) position in this.turnedTiles)
            {
                position.GetNeighbors().ForEach(tilesCount.Increment);
            }

            HashSet<(int x, int y)> nextTiles = new HashSet<(int x, int y)>();
            tilesCount.ForEach(pair =>
            {
                bool isBlack = this.turnedTiles.Contains(pair.Key);
                int count = pair.Value;
                bool willBeBlack = isBlack
                    ? count == 1 || count == 2
                    : count == 2;
                if (willBeBlack)
                {
                    nextTiles.Add(pair.Key);
                }
            });

            this.turnedTiles = nextTiles;
        }

        private void ComputeInitialTiles()
        {
            this.turnedTiles = new HashSet<(int x, int y)>();
            foreach (HexDirection[] directions in this.inputs)
            {
                (int x, int y) position = (0, 0);
                foreach (HexDirection direction in directions)
                {
                    position = position.Move(direction);
                }

                (int x, int y) tile = (position.x, position.y);
                if (!this.turnedTiles.Remove(tile))
                {
                    this.turnedTiles.Add(tile);
                }
            }
        }

        private void ParseInputs(string[] inputLines)
        {
            this.inputs = inputLines.Select(ParseLine).ToArray();
        }

        private static HexDirection[] ParseLine(string line)
        {
            List<HexDirection> list = new List<HexDirection>();
            for (int i = 0; i < line.Length; i++)
            {
                string dir = line[i].ToString();
                if (dir != "e" && dir != "w")
                {
                    i++;
                    dir += line[i];
                }

                list.Add(dir.ParseHexDirection());
            }

            return list.ToArray();
        }
    }

    enum HexDirection { East, NorthEast, NorthWest, West, SouthWest, SouthEast }

    static class HexPositionExtensions
    {
        public static HexDirection ParseHexDirection(this string dir)
        {
            switch (dir)
            {
                case "e": return HexDirection.East;
                case "ne": return HexDirection.NorthEast;
                case "nw": return HexDirection.NorthWest;
                case "w": return HexDirection.West;
                case "sw": return HexDirection.SouthWest;
                case "se": return HexDirection.SouthEast;
                default: throw new ArgumentException();
            }
        }

        public static (int x, int y) Move(this (int x, int y) position, HexDirection direction)
        {
            bool isEvenY = position.y % 2 == 0;
            switch (direction)
            {
                case HexDirection.East:
                    return (position.x + 1, position.y);
                case HexDirection.NorthEast:
                    return isEvenY 
                        ? (position.x, position.y + 1)
                        : (position.x + 1, position.y + 1);
                case HexDirection.NorthWest:
                    return isEvenY
                        ? (position.x - 1, position.y + 1)
                        : (position.x, position.y + 1);
                case HexDirection.West:
                    return (position.x - 1, position.y);
                case HexDirection.SouthWest:
                    return isEvenY
                        ? (position.x - 1, position.y - 1)
                        : (position.x, position.y - 1);
                case HexDirection.SouthEast:
                    return isEvenY
                        ? (position.x, position.y - 1)
                        : (position.x + 1, position.y - 1);
                default:
                    throw new ArgumentException();
            }
        }

        public static (int x, int y)[] GetNeighbors(this (int x, int y) position)
        {
            (int x, int y)[] neighbors = new (int x, int y)[6];
            neighbors[0] = (position.x + 1, position.y); // EAST
            neighbors[1] = (position.x - 1, position.y); // WEST
            neighbors[2] = (position.x, position.y + 1); // NORTH
            neighbors[3] = (position.x, position.y - 1); // SOUTH

            if (position.y % 2 == 0)
            {
                neighbors[4] = (position.x - 1, position.y + 1); // NORTH 2
                neighbors[5] = (position.x - 1, position.y - 1); // SOUTH 2
            }
            else
            {
                neighbors[4] = (position.x + 1, position.y + 1); // NORTH 2
                neighbors[5] = (position.x + 1, position.y - 1); // SOUTH 2
            }

            return neighbors;
        }
    }
}
