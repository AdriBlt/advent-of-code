using System.Collections.Generic;
using System.Linq;

namespace Advent2020
{
    class Day15: BaseDay2020
    {
        private string input = "0,1,5,10,3,12,19";
        private long[] numbers;
        private Dictionary<long, long> lastIterations = new Dictionary<long, long>();
        private long iteration = 1;
        private long lastAge = 0;

        public Day15(): base(15) {
            this.numbers = input.Split(',').Select(long.Parse).ToArray();
        }

        public override long QuestionA()
        {
            return this.ComputeTurnNumber(2020);
        }

        public override long QuestionB()
        {
            return this.ComputeTurnNumber(30000000);
        }

        private long ComputeTurnNumber(long returnedIteration)
        {
            //Console.WriteLine(this.ComputeTurnNumber(new long[] { 0, 3, 6 }, returnedIteration));
            //Console.WriteLine(this.ComputeTurnNumber(new long[] { 1, 3, 2 }, returnedIteration));
            //Console.WriteLine(this.ComputeTurnNumber(new long[] { 2, 1, 3 }, returnedIteration));
            //Console.WriteLine(this.ComputeTurnNumber(new long[] { 1, 2, 3 }, returnedIteration));
            //Console.WriteLine(this.ComputeTurnNumber(new long[] { 2, 3, 1 }, returnedIteration));
            //Console.WriteLine(this.ComputeTurnNumber(new long[] { 3, 2, 1 }, returnedIteration));
            //Console.WriteLine(this.ComputeTurnNumber(new long[] { 3, 1, 2 }, returnedIteration));
            return this.ComputeTurnNumber(this.numbers, returnedIteration);
        }

        private long ComputeTurnNumber(long[] startingNumbers, long returnedIteration)
        {
            this.Reset();
            foreach (long number in startingNumbers)
            {
                this.PlayOneIteration(number);
            }

            while (iteration < returnedIteration) {
                this.PlayOneIteration(this.lastAge);
            }

            return this.lastAge;
        }

        private void PlayOneIteration(long number)
        {
            if (lastIterations.TryGetValue(number, out long previousValue))
            {
                this.lastAge = this.iteration - previousValue;
                this.lastIterations.Remove(number);
            }
            else
            {
                this.lastAge = 0;
            }

            this.lastIterations.Add(number, this.iteration);
            this.iteration++;
        }

        private void Reset()
        {
            this.lastIterations.Clear();
            this.iteration = 1;
            this.lastAge = 0;
        }
    }
}
