using System;
using System.Linq;

namespace Advent2020
{
    enum State { Floor, Empty, Occupied }

    class Day11: BaseDay2020
    {
        private State[][] initialState;

        public Day11(): base(11) {
            this.initialState = this.lines.Select(line => line.Select(ParseState).ToArray()).ToArray();
        }

        public override long QuestionA()
        {
            return GetFinalSeatCount(GetNextStateA);
        }

        public override long QuestionB()
        {
            return GetFinalSeatCount(GetNextStateB);
        }

        private long GetFinalSeatCount(Func<State, int, int , State[][], State> nextStateFunction)
        {
            State[][] matrix = this.initialState;
            while (true)
            {
                State[][] newMatrix = GetNextMatrix(matrix, nextStateFunction);
                if (newMatrix == null)
                {
                    break;
                }
                else
                {
                    matrix = newMatrix;
                }
            }

            return matrix.Sum(row => row.Count(seat => seat == State.Occupied));
        }

        private static State[][] GetNextMatrix(State[][] matrix, Func<State, int, int, State[][], State> nextStateFunction)
        {
            bool hasMatrixChanged = false;
            State[][] newMatrix = matrix.Select((row, i) => row.Select((seat, j) => {
                State newState = nextStateFunction(seat, i, j, matrix);
                if (newState != seat)
                {
                    hasMatrixChanged = true;
                }

                return newState;
            }).ToArray()).ToArray();

            if (hasMatrixChanged)
            {
                return newMatrix;
            }

            return null;
        }

        private static State GetNextStateA(State currentState, int i, int j, State[][] matrix)
        {
            if (currentState == State.Floor)
            {
                return State.Floor;
            }

            int numberOfOccupiedSeats = GetNumberOfSurroundingOccupiedSeats(i, j, matrix);
            if (currentState == State.Empty)
            {
                return numberOfOccupiedSeats == 0 ? State.Occupied : State.Empty;
            }

            if (currentState == State.Occupied)
            {
                return numberOfOccupiedSeats > 3 ? State.Empty : State.Occupied;
            }

            throw new ArgumentException();
        }

        private static int GetNumberOfSurroundingOccupiedSeats(int i, int j, State[][] matrix)
        {
            int count = 0;
            for (int di = -1; di < 2; di++)
            {
                for (int dj = -1; dj < 2; dj++)
                {
                    if (di == 0 && dj == 0)
                    {
                        continue;
                    }

                    count += IsOccupiedSeat(i + di, j + dj, matrix) ? 1 : 0;
                }
            }

            return count;
        }

        private static State GetNextStateB(State currentState, int i, int j, State[][] matrix)
        {
            if (currentState == State.Floor)
            {
                return State.Floor;
            }

            int numberOfOccupiedSeats = GetNumberOfVisibleOccupiedSeats(i, j, matrix);
            if (currentState == State.Empty)
            {
                return numberOfOccupiedSeats == 0 ? State.Occupied : State.Empty;
            }

            if (currentState == State.Occupied)
            {
                return numberOfOccupiedSeats > 4 ? State.Empty : State.Occupied;
            }

            throw new ArgumentException();
        }

        private static int GetNumberOfVisibleOccupiedSeats(int i, int j, State[][] matrix)
        {
            int count = 0;
            for (int di = -1; di < 2; di++)
            {
                for (int dj = -1; dj < 2; dj++)
                {
                    if (di == 0 && dj == 0)
                    {
                        continue;
                    }

                    count += GetVisibleState(i, j, di, dj, matrix) == State.Occupied ? 1 : 0;
                }
            }

            return count;
        }

        private static bool IsOccupiedSeat(int i, int j, State[][] matrix)
        {
            return GetState(i, j, matrix) == State.Occupied;
        }

        private static State? GetVisibleState(int i, int j, int di, int dj, State[][] matrix)
        {
            int ii = i;
            int jj = j;
            while (true)
            {
                ii += di;
                jj += dj;
                State? state = GetState(ii, jj, matrix);
                if (state == State.Floor)
                {
                    continue;
                } else
                {
                    return state;
                }
            }
        }

        private static State? GetState(int i, int j, State[][] matrix)
        {
            if (i >= 0 && i < matrix.Length && j >= 0 && j < matrix[i].Length)
            {
                return matrix[i][j];
            }
            
            return null;
        }

        private static State ParseState(char letter)
        {
            switch(letter)
            {
                case '.': return State.Floor;
                case 'L': return State.Empty;
                case '#': return State.Occupied;
                default: throw new ArgumentException();
            }
        }
    }
}
