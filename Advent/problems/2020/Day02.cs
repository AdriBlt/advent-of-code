using System;
using System.Linq;
using System.Text.RegularExpressions;
using Advent.Utils;

namespace Advent2020
{
    class Day02: BaseDay2020
    {
        Regex lineRegex = new Regex(@"(\d+)-(\d+) (.): (.*)");

        public Day02(): base(2) { }

        public override long QuestionA()
        {
            Func<string, bool> isValid = (string line) => {
                Match res = lineRegex.Match(line);
                int min = res.Groups[1].ToInt();
                int max = res.Groups[2].ToInt();
                string letter = res.Groups[3].ToString();
                string password = res.Groups[4].ToString();
                int count = password.Count(l => l.ToString() == letter);
                return count >= min && count <= max;
            };
            return lines.Count(isValid);
        }

        public override long QuestionB()
        {
            Func<string, bool> isValid = (string line) => {
                Match res = lineRegex.Match(line);
                int pos1 = int.Parse(res.Groups[1].ToString());
                int pos2 = int.Parse(res.Groups[2].ToString());
                string letter = res.Groups[3].ToString();
                string password = res.Groups[4].ToString();
                return (password[pos1 - 1].ToString() == letter) ^ (password[pos2 - 1].ToString() == letter);
            };
            return lines.Count(isValid);
        }
    }
}
