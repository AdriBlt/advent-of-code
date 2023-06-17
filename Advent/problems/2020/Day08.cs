using System.Collections.Generic;
using System.Linq;

namespace Advent2020
{
    class Day08: BaseDay2020
    {
        public Day08(): base(8) { }

        public override long QuestionA()
        {
            return executeInstructions(parseInstructions()).acc;
        }

        public override long QuestionB()
        {
            (string key, int value)[] instructions = parseInstructions();
            for (int i = 0; i < instructions.Length; i++)
            {
                (string key, int _) = instructions[i];
                if (key == "acc")
                {
                    continue;
                }

                instructions[i].key = key == "jmp" ? "nop" : "jmp";

                (long acc, bool hasLooped) = executeInstructions(instructions);
                
                instructions[i].key = key;

                if (hasLooped)
                {
                    continue;
                }
                else
                {
                    return acc;
                }
            }

            return -1;
        }

        private (long acc, bool hasLooped) executeInstructions((string key, int value)[] instructions)
        {
            List<int> visitedIndexes = new List<int> { 0 };
            long acc = 0;
            int index = 0;
            while (true)
            {
                if (index >= instructions.Length)
                {
                    return (acc, false);
                }

                (string key, int value) = instructions[index];
                int nextIndex = index;
                if (key == "acc")
                {
                    acc += value;
                    nextIndex++;
                }
                else if (key == "jmp")
                {
                    nextIndex += value;
                }
                else
                {
                    nextIndex++;
                }

                if (visitedIndexes.Contains(nextIndex))
                {
                    return (acc, true);
                }
                else
                {
                    visitedIndexes.Add(nextIndex);
                    index = nextIndex;
                }

            }
        }

        private (string key, int value)[] parseInstructions()
        {
            return this.lines.Select(line =>
            {
                string[] fields = line.Split(" ");
                return (fields[0], int.Parse(fields[1]));
            }).ToArray();
        }
    }
}
