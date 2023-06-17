using System.Linq;

namespace Advent2020
{
    class Day05: BaseDay2020
    {
        public Day05(): base(5) { }

        public override long QuestionA()
        {
            return lines.Select(getSeatId).Max();
        }

        public override long QuestionB()
        {
            long[] ids = lines.Select(getSeatId).ToArray();
            return Enumerable.Range(0, 128 * 8).Where(id => ids.Contains(id - 1) && !ids.Contains(id) && ids.Contains(id + 1)).Single();
        }

        private long getSeatId(string sequence)
        {
            long rowId = getValue(sequence.Substring(0, 7).Select(letter => letter == 'B').ToArray(), 127);
            long columnId = getValue(sequence.Substring(7, 3).Select(letter => letter == 'R').ToArray(), 7);
            return rowId * 8 + columnId;
        }

        private long getValue(bool[] targetUpper, long maxValue)
        {
            long min = 0;
            long max = maxValue;
            foreach (bool isUpper in targetUpper) {
                if (isUpper)
                {
                    min = (min + max + 1) / 2;
                } else
                {
                    max = (min + max - 1) / 2;
                }
            }
            // assert min == max
            return min;
        }
    }
}
