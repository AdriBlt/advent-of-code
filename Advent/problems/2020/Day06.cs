using System;
using System.Linq;
using Advent.Utils;

namespace Advent2020
{
    class Day06: BaseDay2020
    {
        public Day06(): base(6) { }

        public override long QuestionA()
        {
            return GetGroupAnswersSum(ParseGroupAnswerAnyone);
        }

        public override long QuestionB()
        {
            return GetGroupAnswersSum(ParseGroupAnswerEveryone);
        }

        private long GetGroupAnswersSum(Func<string[], string> groupAnswersParser)
        {
            return ParsingUtils.SplitGroupsByEmptyLines(lines)
                .Select(groupAnswersParser)
                .Sum(answer => answer.Length);
        }

        private string ParseGroupAnswerAnyone(string[] individualAnswers)
        {
            string compactAnswers = string.Concat(string.Concat(individualAnswers).OrderBy(letter => letter));
            string answer = "";
            char lastLetter = '-';
            foreach (char letter in compactAnswers)
            {
                if (letter == lastLetter)
                {
                    continue;
                }

                answer += letter;
                lastLetter = letter;
            }

            return answer;
        }

        private string ParseGroupAnswerEveryone(string[] individualAnswers)
        {
            string answer = "";
            string groupAnswer = ParseGroupAnswerAnyone(individualAnswers);
            foreach (char letter in groupAnswer)
            {
                if (individualAnswers.All(answer => answer.Contains(letter)))
                {
                    answer += letter;
                }
            }

            return answer;
        }
    }
}
