using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent2020
{
    class Day10: BaseDay2020
    {
        private int[] numbers;
        private Dictionary<int, long> solutions = new Dictionary<int, long>();

        public Day10(): base(10) {
            this.numbers = this.lines.Select(int.Parse).ToArray();
            Array.Sort(this.numbers);
        }

        public override long QuestionA()
        {
            long diff1 = 0;
            long diff3 = 1; // Last input
            int lastValue = 0;
            for (int i = 0; i < this.numbers.Length; i++)
            {
                int diff = this.numbers[i] - lastValue;
                lastValue = this.numbers[i];
                if (diff == 1)
                {
                    diff1++;
                }
                else if (diff == 3)
                {
                    diff3++;
                }
            }

            return diff1 * diff3;
        }

        public override long QuestionB()
        {
            return this.searchSolutions();
        }

        private long searchSolutions(int currentIndex = -1)
        {
            if (currentIndex == this.numbers.Length - 1)
            {
                return 1;
            }

            long solutions = 0;
            int currentValue = currentIndex < 0 ? 0 : this.numbers[currentIndex];
            int nextIndex = currentIndex + 1;
            while (nextIndex < this.numbers.Length)
            {
                if (this.numbers[nextIndex] - currentValue < 4)
                {
                    int nextValue = this.numbers[nextIndex];
                    if (!this.solutions.ContainsKey(nextValue))
                    {
                        this.solutions.Add(nextValue, searchSolutions(nextIndex));
                    }

                    solutions += this.solutions[nextValue];
                    nextIndex++;
                }
                else
                {
                    break;
                }
            }

            return solutions;
        }
    }
}
