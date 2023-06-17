using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Advent.Utils;

namespace Advent2020
{
    class Day07: BaseDay2020
    {
        private string myBag = "shiny gold";
        private Dictionary<string, Dictionary<string, int>> rules;

        public Day07(): base(7)
        {
            rules = ParseRules();
        }

        public override long QuestionA()
        {
            List<string> bags = new List<string>();
            bags.Add(myBag);

            int index = 0;
            while (index < bags.Count)
            {
                bags.AddRange(rules.Where(pair => pair.Value.ContainsKey(bags[index])).Select(pair => pair.Key).Where(key => !bags.Contains(key)));
                index++;
            }

            return bags.Count - 1;
        }

        public override long QuestionB()
        {
            Stack<(string, long)> bags = new Stack<(string, long)>();
            bags.Push((myBag, 1));

            long count = 0;
            while (bags.Count > 0)
            {
                (string name, long amount) = bags.Pop();
                Dictionary<string, int> rule = rules[name];
                foreach (KeyValuePair<string, int> subBag in rule)
                {
                    long addedBags = amount * subBag.Value;
                    bags.Push((subBag.Key, addedBags));
                    count += addedBags;
                }
            }

            return count;
        }

        private Dictionary<string, Dictionary<string, int>> ParseRules() {
            Regex lineRegex = new Regex(@"^(.+) bags contain (.+)\.$");
            Regex constraintRegex = new Regex(@"^(\d+) (.+) bags?$");
            string noConstraints = "no other bags";
            Dictionary<string, Dictionary<string, int>> rules = new Dictionary<string, Dictionary<string, int>>();
            foreach (string line in lines)
            {
                Match res = lineRegex.Match(line);
                string bagColor = res.Groups[1].ToString();
                string constraints = res.Groups[2].ToString();
                Dictionary<string, int> rule = new Dictionary<string, int>();
                if (constraints != noConstraints)
                {
                    string[] subBags = constraints.Split(", ");
                    foreach (string bag in subBags)
                    {
                        Match bagRes = constraintRegex.Match(bag);
                        int bagCount = bagRes.Groups[1].ToInt();
                        string bagName = bagRes.Groups[2].ToString();
                        rule.Add(bagName, bagCount);
                    }
                }

                rules.Add(bagColor, rule);
            }

            return rules;
        }
    }
}
