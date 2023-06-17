using System.Collections.Generic;
using System.Linq;

namespace Advent2020
{
    class Day09: BaseDay2020
    {
        private long[] numbers;
        public Day09(): base(9) {
            this.numbers = this.lines.Select(long.Parse).ToArray();
        }

        public override long QuestionA()
        {
            for (int i = 25; i < numbers.Length; i++)
            {
                long currentNumber = numbers[i];
                bool isValid = false;
                for (int j = i - 25; j < i - 1; j++)
                {
                    if (this.numbers.Skip(1 + j).Take(i - j).Contains(numbers[i] - numbers[j])) {
                        isValid = true;
                        break;
                    }
                }

                if (!isValid)
                {
                    return currentNumber;
                }
            }

            return -1;
        }

        public override long QuestionB()
        {
            long invalidNumber = QuestionA();
            for (int i = 0; i < numbers.Length; i++)
            {
                if (numbers[i] == invalidNumber)
                {
                    continue;
                }

                long sum = 0;
                for (int j = i; j < numbers.Length; j++)
                {
                    sum += numbers[j];
                    if (sum > invalidNumber)
                    {
                        break;
                    }
                    if (sum == invalidNumber)
                    {
                        IEnumerable<long> range = numbers.Skip(i).Take(j - i + 1).ToArray();
                        return range.Min() + range.Max();
                    }
                }
            }

            return -1;
        }
    }
}
