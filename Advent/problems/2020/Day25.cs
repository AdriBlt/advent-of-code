using System;

namespace Advent2020
{
    class Day25: BaseDay2020
    {
        private static string[] TestLines = new string[]
        {
            "5764801",
            "17807724",
        };

        private long cardPublicKey;
        private long doorPublicKey;

        public Day25(): base(25) {
            this.ParseInputs(this.lines);
        }

        public override long QuestionA()
        {
            long initialSubjectNumber = 7;
            long cardLoopSize = FindLoopSize(initialSubjectNumber, cardPublicKey);
            long doorLoopSize = FindLoopSize(initialSubjectNumber, doorPublicKey);
            long result1 = Encrypt(cardPublicKey, doorLoopSize);
            long result2 = Encrypt(doorPublicKey, cardLoopSize);
            long result = result1 == result2 ? result1 : throw new ArgumentOutOfRangeException();
            return result;
        }

        public override long QuestionB()
        {
            return 0;
        }

        private static long FindLoopSize(long subjectNumber, long finalKey)
        {
            long currentValue = subjectNumber;
            for (long i = 0; ; i++)
            {
                if (currentValue == finalKey)
                {
                    return i;
                }
                
                currentValue = ExecuteLoop(subjectNumber, currentValue);
            }
        }

        private static long Encrypt(long subjectNumber, long loopSize)
        {
            long currentValue = subjectNumber;
            for (long i = 0; i < loopSize; i++)
            {
                currentValue = ExecuteLoop(subjectNumber, currentValue);
            }

            return currentValue;
        }

        private static long ExecuteLoop (long subjectNumber, long currentValue)
        {
            return (currentValue * subjectNumber) % 20201227;
        }

        private void ParseInputs(string[] inputLines)
        {
            this.cardPublicKey = long.Parse(inputLines[0]);
            this.doorPublicKey = long.Parse(inputLines[1]);
        }
    }
}
