using System;
using System.Collections.Generic;
using System.Linq;
using Advent.Utils;

namespace Advent2020
{
    class Day17: BaseDay2020
    {
        private bool[][] initialState;
        private HashSet<Cell> activeCells;

        public Day17(): base(17) {
            // this.lines = new string[] { ".#.", "..#", "###" };
            this.initialState = this.lines.Select(line => line.Select(cell => cell == '#').ToArray()).ToArray();
        }

        public override long QuestionA()
        {
            return this.GetNeigboursCount(cell => cell.GetNeighboursThreeDimensions());
        }

        public override long QuestionB()
        {
            return this.GetNeigboursCount(cell => cell.GetNeighboursFourDimensions());
        }

        private long GetNeigboursCount(Func<Cell, Cell[]> getNeighbours, int iterationCount = 6)
        {
            this.activeCells = new HashSet<Cell>();
            for (int i = 0; i < this.initialState.Length; i++)
            {
                for (int j = 0; j < this.initialState[i].Length; j++)
                {
                    if (this.initialState[i][j])
                    {
                        this.activeCells.Add(new Cell(j, i));
                    }
                }
            }

            for (int i = 0; i < 6; i++)
            {
                this.ComputeNextCells(getNeighbours);
            }

            return this.activeCells.Count;
        }

        private void ComputeNextCells(Func<Cell, Cell[]> getNeighbours)
        {
            Dictionary<Cell, int> neighboursCount = new Dictionary<Cell, int>();
            foreach (Cell cell in this.activeCells)
            {
                foreach(Cell neihgbour in getNeighbours(cell))
                {
                    neighboursCount.Increment(neihgbour);
                }
            }

            HashSet<Cell> nextCells = new HashSet<Cell>();
            foreach (KeyValuePair<Cell, int> neighbour in neighboursCount)
            {
                if (
                    neighbour.Value == 3 
                    || (neighbour.Value == 2 && this.activeCells.Contains(neighbour.Key))
                )
                {
                    nextCells.Add(neighbour.Key);
                }
            }

            this.activeCells = nextCells;
        }

        private void Print()
        {
            int minX = this.activeCells.Min(cell => cell.x);
            int maxX = this.activeCells.Max(cell => cell.x);
            int minY = this.activeCells.Min(cell => cell.y);
            int maxY = this.activeCells.Max(cell => cell.y);
            int minZ = this.activeCells.Min(cell => cell.z);
            int maxZ = this.activeCells.Max(cell => cell.z);
            for (int z = minZ; z <= maxZ; z++)
            {
                Console.WriteLine($"z = {z}");
                for (int y = minY; y <= maxY; y++)
                {
                    for (int x = minX; x <= maxX; x++)
                    {
                        Console.Write(this.activeCells.Contains(new Cell(x, y, z)) ? "#" : ".");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }

    struct Cell
    {
        public int x;
        public int y;
        public int z;
        public int w;

        public Cell(int x, int y, int z = 0, int w = 0)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public Cell[] GetNeighboursThreeDimensions()
        {
            Cell[] cells = new Cell[26];
            int[] delta = new int[] { -1, 0, 1 };
            int index = 0;
            foreach (int dx in delta)
            {
                foreach (int dy in delta)
                {
                    foreach (int dz in delta)
                    {
                        if (dx != 0 || dy != 0 || dz != 0)
                        {
                            cells[index++] = new Cell(x + dx, y + dy, z + dz);
                        }
                    }
                }
            }

            return cells;
        }

        public Cell[] GetNeighboursFourDimensions()
        {
            Cell[] cells = new Cell[80];
            int[] delta = new int[] { -1, 0, 1 };
            int index = 0;
            foreach (int dx in delta)
            {
                foreach (int dy in delta)
                {
                    foreach (int dz in delta)
                    {
                        foreach (int dw in delta)
                        {
                            if (dx != 0 || dy != 0 || dz != 0 || dw != 0)
                            {
                                cells[index++] = new Cell(x + dx, y + dy, z + dz, w + dw);
                            }
                        }
                    }
                }
            }

            return cells;
        }
    }
}
