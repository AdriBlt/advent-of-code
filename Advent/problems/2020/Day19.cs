using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Advent.Utils;

namespace Advent2020
{
    class Day19: BaseDay2020
    {
        static string[] TestInput = new string[] 
        {
            "0: 4 1 5",
            "1: 2 3 | 3 2",
            "2: 4 4 | 5 5",
            "3: 4 5 | 5 4",
            "4: \"a\"",
            "5: \"b\"",
            "",
            "ababbb",
            "bababa",
            "abbbab",
            "aaabbb",
            "aaaabbb"
        };

        Regex leaftRule = new Regex("^\"(\\w+)\"$");
        Regex startsWithLeaftRule = new Regex("^\"(\\w+)\"(.*)");
        string[] rules;
        string[][] validMessagesForRules;
        string[] messages;

        public Day19(): base(19) {
            this.ParseInput(this.lines);
        }

        public override long QuestionA()
        {
            this.validMessagesForRules = new string[this.rules.Length][];
            this.validMessagesForRules[0] = ParseRule(this.rules[0]);
            return this.messages.Count(this.validMessagesForRules[0].Contains);
        }

        public override long QuestionB()
        {
            this.rules[8] = "42 | 42 8";
            this.rules[11] = "42 31 | 42 11 31";
            return this.messages.Count(message => this.IsValidMessage(message, this.rules[0]));
        }

        private bool IsValidMessage(string message, string rule)
        {
            LinkedList<(string message, string rule)> pathsToExplore = new LinkedList<(string message, string rule)>();
            pathsToExplore.AddLast((message, rule));

            while (pathsToExplore.Count > 0)
            {
                (string msg, string r) = pathsToExplore.First();
                pathsToExplore.RemoveFirst();
                (string message, string rule)[] newPaths = GetValidationResult(msg, r);
                foreach ((string newM, string newR) in newPaths)
                {
                    if (newM.Trim() == "" && newR.Trim() == "")
                    {
                        return true;
                    }

                    pathsToExplore.AddLast((newM, newR));
                }
            }

            return false;
        }

        private (string message, string rule)[] GetValidationResult(string message, string rule)
        {
            if (rule == "")
            {
                if (message.Trim() == "")
                {
                    return new (string message, string rule)[] { ("", "") };
                }
                else
                {
                    return new (string message, string rule)[] { };
                }
            }

            Match leaf = startsWithLeaftRule.Match(rule);
            if (leaf.Success)
            {
                string firstChar = leaf.Groups[1].ToString();
                if (message.StartsWith(firstChar))
                {
                    return new (string message, string rule)[] {
                        (message.Substring(firstChar.Length), leaf.Groups[2].ToString().Trim())
                    };
                }
                else
                {
                    return new (string message, string rule)[] {};
                }
            }

            if (rule.Contains(" | "))
            {
                List<(string message, string rule)> result = new List<(string message, string rule)>();
                foreach (string subRule in rule.Split(" | "))
                {
                    result.AddRange(GetValidationResult(message, subRule));
                }

                return result.ToArray();
            }

            int space = rule.IndexOf(' ');
            if (space < 0)
            {
                int ruleId = int.Parse(rule);
                return new (string message, string rule)[] { (message, this.rules[ruleId]) };
            }

            string prefix = rule.Substring(0, space);
            string suffix = rule.Substring(space + 1);
            List<(string message, string rule)> allResult = new List<(string message, string rule)>();
            (string message, string rule)[] prefixResults = GetValidationResult(message, prefix);
            foreach ((string msg, string r) in prefixResults)
            {
                foreach (string subRule in r.Split(" | "))
                {
                    allResult.Add((msg, subRule + " " + suffix));
                }
            }

            return allResult.ToArray();
        }

        private string[] ParseRule(string rule)
        {
            Match leaf = leaftRule.Match(rule);
            if (leaf.Success)
            {
                return new string[]
                {
                    leaf.Groups[1].ToString()
                };
            }

            if (rule.Contains(" | "))
            {
                List<string> validMessages = new List<string>();
                string[] subRules = rule.Split(" | ");
                foreach (string subRule in subRules)
                {
                    validMessages.AddRange(ParseRule(subRule));
                }

                return validMessages.ToArray();
            }

            int space = rule.IndexOf(' ');
            if (space < 0)
            {
                int ruleId = int.Parse(rule);
                if (this.validMessagesForRules[ruleId] == null)
                {
                    this.validMessagesForRules[ruleId] = ParseRule(this.rules[ruleId]);
                }

                return this.validMessagesForRules[ruleId];
            }

            string prefix = rule.Substring(0, space);
            string suffix = rule.Substring(space + 1);
            string[] validMessagePrefix = ParseRule(prefix);
            string[] validMessageSuffix = ParseRule(suffix);
            List<string> validMessagesPreSuf = new List<string>();
            foreach (string pre in validMessagePrefix)
            {
                foreach (string suf in validMessageSuffix)
                {
                    validMessagesPreSuf.Add(pre + suf);
                }
            }

            return validMessagesPreSuf.ToArray();
        }

        private void ParseInput(string[] input)
        {
            List<(int id, string rule)> rules = new List<(int, string)>();
            int i;
            Regex ruleRegex = new Regex(@"^(\d+): (.+)$"); 
            for (i = 0; i < input.Length; i++)
            {
                if (input[i].Trim() == "")
                {
                    break;
                }

                Match result = ruleRegex.Match(input[i]);
                rules.Add((result.Groups[1].ToInt(), result.Groups[2].ToString()));
            }

            List<string> messages = new List<string>();
            for (i++; i < input.Length; i++)
            {
                messages.Add(input[i]);
            }

            this.rules = new string[rules.Count];
            foreach ((int id, string rule) in rules)
            {
                this.rules[id] = rule;
            }

            this.messages = messages.ToArray();
        }
    }
}
