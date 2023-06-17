using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Advent.Utils;


namespace Advent2020
{
    enum Rotation // Clock direction
    {
        None, Rotate90, Rotate180, Rotate270
    }

    enum Direction
    {
        North, West, South, East
    }

    class Day20: BaseDay2020
    {
        private static string[] TestLines = new string[] {
            "Tile 2311:",
            "..##.#..#.",
            "##..#.....",
            "#...##..#.",
            "####.#...#",
            "##.##.###.",
            "##...#.###",
            ".#.#.#..##",
            "..#....#..",
            "###...#.#.",
            "..###..###",
            "",
            "Tile 1951:",
            "#.##...##.",
            "#.####...#",
            ".....#..##",
            "#...######",
            ".##.#....#",
            ".###.#####",
            "###.##.##.",
            ".###....#.",
            "..#.#..#.#",
            "#...##.#..",
            "",
            "Tile 1171:",
            "####...##.",
            "#..##.#..#",
            "##.#..#.#.",
            ".###.####.",
            "..###.####",
            ".##....##.",
            ".#...####.",
            "#.##.####.",
            "####..#...",
            ".....##...",
            "",
            "Tile 1427:",
            "###.##.#..",
            ".#..#.##..",
            ".#.##.#..#",
            "#.#.#.##.#",
            "....#...##",
            "...##..##.",
            "...#.#####",
            ".#.####.#.",
            "..#..###.#",
            "..##.#..#.",
            "",
            "Tile 1489:",
            "##.#.#....",
            "..##...#..",
            ".##..##...",
            "..#...#...",
            "#####...#.",
            "#..#.#.#.#",
            "...#.#.#..",
            "##.#...##.",
            "..##.##.##",
            "###.##.#..",
            "",
            "Tile 2473:",
            "#....####.",
            "#..#.##...",
            "#.##..#...",
            "######.#.#",
            ".#...#.#.#",
            ".#########",
            ".###.#..#.",
            "########.#",
            "##...##.#.",
            "..###.#.#.",
            "",
            "Tile 2971:",
            "..#.#....#",
            "#...###...",
            "#.#.###...",
            "##.##..#..",
            ".#####..##",
            ".#..####.#",
            "#..#.#..#.",
            "..####.###",
            "..#.#.###.",
            "...#.#.#.#",
            "",
            "Tile 2729:",
            "...#.#.#.#",
            "####.#....",
            "..#.#.....",
            "....#..#.#",
            ".##..##.#.",
            ".#.####...",
            "####.#.#..",
            "##.####...",
            "##..#.##..",
            "#.##...##.",
            "",
            "Tile 3079:",
            "#.#.#####.",
            ".#..######",
            "..#.......",
            "######....",
            "####.#..#.",
            ".#...#.##.",
            "#.#####.##",
            "..#.###...",
            "..#.......",
            "..#.###..."
        };
        private static string[] Monster = new string[]
        { 
            "                  # ",
            "#    ##    ##    ###",
            " #  #  #  #  #  #   "
        };
        private static (int i, int j)[] MonstersBits = Monster
                .SelectMany((line, i) => line
                    .Select((c, j) => (value: c == '#', j: j))
                    .Where(obj => obj.value)
                    .Select(obj => (i: i, j: obj.j)))
                .ToArray();

        private Tile[] tiles;
        private MatchingResult[][][] matchingResults;
        private TileLocation?[][] tilesIndexLocation;
        private bool[][] image;

        public Day20() : base(20) {
            this.ParseTiles(this.lines);

            this.ComputeMatchingResults();
            this.ComputeTilesLocation();
            this.ComputeImage();
        }

        public override long QuestionA()
        {
            int[] indexes = this.matchingResults
                .Select((line, i) => (count: line.Sum(res => res == null ? 0 : res.Length), index: i))
                .Where(obj => obj.count == 2)
                .Select(obj => obj.index)
                .ToArray();

            long result = 1L;
            foreach (int i in indexes)
            {
                result *= this.tiles[i].id;
            }

            return result;
        }

        public override long QuestionB()
        {
            // this.PrintTiles();
            // this.PrintImage();
            int monsterCount = Tile.AllRotations
                .SelectMany(r => new (Rotation rot, bool isFlipped)[] { (r, true), (r, false) })
                .Select(transform => {
                    (int i, int j)[] monsterPositions = this.GetMonsterPositions(transform.rot, transform.isFlipped);
                    return this.CountMonsters(monsterPositions);
                })
                .Single(count => count > 0);
            long numberOfPieces = this.image.Sum(line => line.Count(b => b));
            return numberOfPieces - monsterCount * MonstersBits.Length;
        }

        private int CountMonsters((int i, int j)[] monsterPositions)
        {
            int maxI = monsterPositions.Select(pos => pos.i).Max();
            int maxJ = monsterPositions.Select(pos => pos.j).Max();
            int monsterCount = 0;
            for (int i = 0; i < this.image.Length - maxI; i++)
            {
                for (int j = 0; j < this.image[i].Length - maxJ; j++)
                {
                    if (this.isMonster(monsterPositions, i, j))
                    {
                        monsterCount++;
                    }
                }
            }

            return monsterCount;
        }

        private bool isMonster((int i, int j)[] monsterPositions, int deltaI, int deltaJ)
        {
            foreach ((int i, int j) in monsterPositions)
            {
                if (!this.image[deltaI + i][deltaJ + j])
                {
                    return false;
                }
            }

            return true;
        }

        private (int i, int j)[] GetMonsterPositions(Rotation rot, bool isFlipped)
        {
            var positions = isFlipped 
                ? MonstersBits.Select(pos => (i: pos.i, j: Monster[0].Length - 1 - pos.j)).ToArray()
                : MonstersBits;

            switch (rot)
            {
                case Rotation.None:
                    return positions;
                case Rotation.Rotate90:
                    return positions.Select(pos => (i: pos.j, j: Monster.Length - 1 - pos.i)).ToArray();
                case Rotation.Rotate180:
                    return positions.Select(pos => (i: Monster.Length - 1 - pos.i, j: Monster[0].Length - 1 - pos.j)).ToArray();
                case Rotation.Rotate270:
                    return positions.Select(pos => (i: Monster[0].Length - 1 - pos.j, j: pos.i)).ToArray();
                default:
                    throw new ArgumentException();
            }
        }

        private void ComputeImage()
        {
            int nbTilePerSide = (int)Math.Sqrt(this.tiles.Length);
            int tileSide = this.tiles[0].bits.Length - 2;
            int sideLength = nbTilePerSide * tileSide;
            this.image = new bool[sideLength][];
            for (int i = 0; i < sideLength; i++)
            {
                this.image[i] = new bool[sideLength];
            }

            for (int i = 0; i < this.tilesIndexLocation.Length; i++)
            {
                for (int j = 0; j < this.tilesIndexLocation[i].Length; j++)
                {
                    TileLocation location = this.tilesIndexLocation[i][j].Value;
                    Tile tile = this.tiles[location.index];
                    for (int ii = 0; ii < tile.bits.Length - 2; ii++)
                    {
                        bool[] line = tile.GetLine(ii + 1, location.rotation, location.isFlipped);
                        for (int jj = 0; jj < line.Length - 2; jj++)
                        {
                            this.image[i * tileSide + ii][j * tileSide + jj] = line[jj + 1];
                        }
                    }
                }
            }
        }

        private void ComputeTilesLocation()
        {
            int cornerIndex = this.matchingResults
                .Select((line, i) => (count: line.Sum(res => res == null ? 0 : res.Length), index: i))
                .Where(obj => obj.count == 2)
                .Select(obj => obj.index)
                .First();

            int side = (int)Math.Sqrt(this.tiles.Length);
            this.tilesIndexLocation = new TileLocation?[side][];
            for (int i = 0; i < side; i++)
            {
                this.tilesIndexLocation[i] = new TileLocation?[side];
            }

            Direction[] cornerNeighbors = this.matchingResults[cornerIndex]
                .Where(line => line != null && line.Length > 0)
                .Select(line => line[0].dir)
                .ToArray();
            Rotation cornerRotation;
            if (cornerNeighbors.Contains(Direction.North) && cornerNeighbors.Contains(Direction.West))
            {
                cornerRotation = Rotation.Rotate180;
            }
            else if (cornerNeighbors.Contains(Direction.West) && cornerNeighbors.Contains(Direction.South))
            {
                cornerRotation = Rotation.Rotate270;
            }
            else if (cornerNeighbors.Contains(Direction.South) && cornerNeighbors.Contains(Direction.East))
            {
                cornerRotation = Rotation.None;
            }
            else if (cornerNeighbors.Contains(Direction.East) && cornerNeighbors.Contains(Direction.North))
            {
                cornerRotation = Rotation.Rotate90;
            }
            else
            {
                throw new ArgumentException();
            }
                
            this.SetTileLocation(0, 0, cornerIndex, cornerRotation, false);
        }

        private void SetTileLocation(int i, int j, int index, Rotation rotation, bool isFlipped)
        {
            if (this.tilesIndexLocation[i][j] != null)
            {
                return;
            }

            this.tilesIndexLocation[i][j] = new TileLocation(index, rotation, isFlipped);

            (MatchingResult result, int index)[] neighbours = this.matchingResults[index]
                .Select((line, k) => (line: line, index: k))
                .Where(obj => obj.line != null && obj.line.Length > 0)
                .Select(obj => (result: obj.line.Single(), index: obj.index))
                .ToArray();
            foreach ((MatchingResult result, int nIndex) in neighbours)
            {
                Direction direction = result.dir;
                if (isFlipped)
                {
                    direction = direction.Flip();
                }
                direction = direction.Rotate(rotation);
                int ni = i;
                int nj = j;
                switch (direction)
                {
                    case Direction.North:
                        ni--;
                        break;
                    case Direction.West:
                        nj--;
                        break;
                    case Direction.South:
                        ni++;
                        break;
                    case Direction.East:
                        nj++;
                        break;
                    default:
                        throw new ArgumentException();
                }

                Rotation nRot = rotation.Rotate(result.rot, isFlipped);
                
                bool nFlipped = isFlipped != result.isFlipped;
                this.SetTileLocation(ni, nj, nIndex, nRot, nFlipped);
            }
        }

        private void ComputeMatchingResults()
        {
            this.matchingResults = new MatchingResult[this.tiles.Length][][];
            for (int i = 0; i < this.tiles.Length; i++)
            {
                matchingResults[i] = new MatchingResult[this.tiles.Length][];
                Tile tileA = this.tiles[i];
                for (int j = 0; j < this.tiles.Length; j++)
                {
                    if (i == j)
                    {
                        continue;
                    }

                    Tile tileB = this.tiles[j];
                    matchingResults[i][j] = tileA.GetPossibleFits(tileB);
                }
            }
        }

        private void ParseTiles(string[] lines)
        {
            Regex tileRegex = new Regex(@"^Tile (\d+):$");
            List<Tile> tiles = new List<Tile>();
            this.tiles = ParsingUtils.SplitGroupsByEmptyLines(lines)
                .Select((string[] group) => {
                    long id = tileRegex.Match(group[0]).Groups[1].ToLong();
                    bool[][] bits = group[1..].Select(line => line.Select(c => c == '#').ToArray()).ToArray();
                    return new Tile(id, bits);
                })
                .ToArray();
        }

        private void PrintTiles()
        {
            foreach (TileLocation?[] tileLines in this.tilesIndexLocation)
            {
                for (int i = 0; i < this.tiles[0].bits.Length; i++)
                {
                    string str = "";
                    foreach (TileLocation? tile in tileLines)
                    {
                        bool[] line = this.tiles[tile.Value.index].GetLine(i, tile.Value.rotation, tile.Value.isFlipped);
                        str += string.Join("", line.Select(b => b ? "#" : ".").ToArray()) + " ";
                    }

                    Console.WriteLine(str);
                }

                Console.WriteLine();
            }
        }

        private void PrintImage()
        {
            foreach (bool[] line in this.image)
            {
                string str = "";
                foreach (bool b in line)
                {
                    str += b ? "#" : ".";
                }
                Console.WriteLine(str);
            }
        }
    }

    struct MatchingResult
    {
        public Direction dir;
        public Rotation rot;
        public bool isFlipped;

        public MatchingResult(Direction dir, Rotation rot, bool isFlipped)
        {
            this.dir = dir;
            this.rot = rot;
            this.isFlipped = isFlipped;
        }
    }

    struct TileLocation
    {
        public int index;
        public Rotation rotation;
        public bool isFlipped;

        public TileLocation(int index, Rotation rotation, bool isFlipped)
        {
            this.index = index;
            this.rotation = rotation;
            this.isFlipped = isFlipped;
        }
    }

    struct Tile
    {
        public static Direction[] AllDirections = new Direction[] { Direction.North, Direction.West, Direction.South, Direction.East };
        public static Rotation[] AllRotations = new Rotation[] { Rotation.None, Rotation.Rotate90, Rotation.Rotate180, Rotation.Rotate270 };

        public long id;
        public bool[][] bits;
        public long[] borders;
        public long[] bordersReversed;

        public Tile(long id, bool[][] bits)
        {
            this.id = id;
            this.bits = bits;

            long borderNorth = this.bits[0].ToLong();
            long borderWest = this.bits.Select(line => line[0]).Reverse().ToLong();
            long borderSouth = this.bits[this.bits.Length - 1].Reverse().ToLong();
            long borderEast = this.bits.Select(line => line[line.Length - 1]).ToLong();
            this.borders = new long[] { borderNorth, borderWest, borderSouth, borderEast };

            long borderNorthReversed = this.bits[0].Reverse().ToLong();
            long borderWestReversed = this.bits.Select(line => line[0]).ToLong();
            long borderSouthReversed = this.bits[this.bits.Length - 1].ToLong();
            long borderEastReversed = this.bits.Select(line => line[line.Length - 1]).Reverse().ToLong();
            this.bordersReversed = new long[] { borderNorthReversed, borderWestReversed, borderSouthReversed, borderEastReversed };
        }

        public MatchingResult[] GetPossibleFits(Tile tile)
        {
            List<MatchingResult> fits = new List<MatchingResult>();
            for (int d = 0; d < AllDirections.Length; d++)
            {
                Direction dir = AllDirections[d];
                long borderValue = this.borders[d];
                for (int r = 0; r < AllRotations.Length; r++)
                {
                    Rotation rot = AllRotations[r];
                    int otherBorderIndex = (d + 2 + r) % tile.borders.Length;
                    long otherBorderValue = tile.bordersReversed[otherBorderIndex];
                    if (borderValue == otherBorderValue)
                    {
                        fits.Add(new MatchingResult(dir, rot, false));
                    }

                    int otherBorderReversedIndex = otherBorderIndex % 2 == 0
                        ? otherBorderIndex
                        : (otherBorderIndex + 2) % tile.borders.Length;
                    long otherBorderReversedValue = tile.borders[otherBorderReversedIndex];
                    if (borderValue == otherBorderReversedValue)
                    {
                        fits.Add(new MatchingResult(dir, rot, true));
                    }
                }
            }

            return fits.ToArray();
        }

        internal bool[] GetLine(int i, Rotation rotation, bool isFlipped)
        {
            bool[][] bools = isFlipped
                ? this.bits.Select(line => line.Reverse().ToArray()).ToArray()
                : this.bits;

            switch (rotation)
            {
                case Rotation.None:
                    return bools[i];
                case Rotation.Rotate90:
                    return bools.Select(line => line[i]).Reverse().ToArray();
                case Rotation.Rotate180:
                    return bools[this.bits.Length - 1 - i].Reverse().ToArray();
                case Rotation.Rotate270:
                    return bools.Select(line => line[line.Length - 1 - i]).ToArray();
                default:
                    throw new ArgumentException();
            }
        }
    }

    static class TileExtensions
    {
        public static Direction Flip(this Direction direction)
        {
            switch (direction)
            {
                case Direction.North:
                    return Direction.North;
                case Direction.West:
                    return Direction.East;
                case Direction.South:
                    return Direction.South;
                case Direction.East:
                    return Direction.West;
                default:
                    throw new ArgumentException();
            }
        }

        public static Direction Rotate(this Direction dir, Rotation rot)
        {
            int d = (int)dir;
            int r = (int)rot;
            return Tile.AllDirections[(Tile.AllDirections.Length + d - r) % Tile.AllDirections.Length];
        }

        public static Rotation Rotate(this Rotation rota, Rotation rotb, bool isFlipped = false)
        {
            int a = (int)rota;
            int b = (int)rotb;
            int index;
            if (isFlipped)
            {
                index = Tile.AllRotations.Length + a - b;
            }
            else
            {
                index = a + b;
            }
            return Tile.AllRotations[index % Tile.AllRotations.Length];
        }
    }
}
