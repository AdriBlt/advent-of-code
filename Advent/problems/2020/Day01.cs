using System.Linq;

namespace Advent2020
{
    class Day01: BaseDay2020
    {
        private int[] numbers;
        private int TARGET = 2020;

        public Day01(): base(1)
        {
            this.numbers = lines.Select(int.Parse).ToArray();
        }

        public override long QuestionA()
        {
            for (int i = 0; i < numbers.Length; i++)
            {
                if (numbers[i] > TARGET)
                {
                    continue;
                }

                for (int j = i + 1; j < numbers.Length; j++)
                {
                    if (numbers[i] + numbers[j] == TARGET)
                    {
                        return numbers[i] * numbers[j];
                    }
                }
            }

            return -1;
        }

        public override long QuestionB()
        {
            for (int i = 0; i < numbers.Length; i++)
            {
                if (numbers[i] > TARGET)
                {
                    continue;
                }

                for (int j = i + 1; j < numbers.Length; j++)
                {
                    if (numbers[i] + numbers[j] > TARGET)
                    {
                        continue;
                    }

                    for (int k = j + 1; k < numbers.Length; k++)
                    {
                        if (numbers[i] + numbers[j] + numbers[k] == TARGET)
                        {
                            return numbers[i] * numbers[j] * numbers[k];
                        }
                    }
                }
            }

            return -1;
        }
    }
}
