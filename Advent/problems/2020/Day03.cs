namespace Advent2020
{
    class Day03: BaseDay2020
    {
        public Day03(): base(3) { }

        public override long QuestionA()
        {
            return getNumberOfTrees(3, 1);
        }

        public override long QuestionB()
        {
            return getNumberOfTrees(1, 1)
                * getNumberOfTrees(3, 1)
                * getNumberOfTrees(5, 1)
                * getNumberOfTrees(7, 1)
                * getNumberOfTrees(1, 2);
        }

        private long getNumberOfTrees(int slopeRight, int slopeDown)
        {
            int count = 0;
            int position = 0;
            for (int i = 0; i < lines.Length;i += slopeDown)
            {
                string line = lines[i];
                if (line[position] == '#')
                {
                    count++;
                }

                position = (position + slopeRight) % line.Length;
            }

            return count;
        }
    }
}
