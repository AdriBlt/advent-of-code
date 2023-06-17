using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Advent.Utils;

namespace Advent2020
{
    class Day16: BaseDay2020
    {
        private Rule[] rules;
        private int[][] allTickets;
        private int[] myTicket;
        private int[][] validTickets;
        private List<int>[] fieldsRules;
        private List<int>[] ruleIndex;

        public Day16(): base(16) {
            this.ParseInput();
        }

        public override long QuestionA()
        {
            long errorRate = 0;
            List<int[]> validOnes = new List<int[]>();
            foreach (int[] ticket in this.allTickets)
            {
                long ticketErrorRate = this.GetTicketErrorRate(ticket);
                if (ticketErrorRate > 0)
                {
                    errorRate += ticketErrorRate;
                }
                else
                {
                    validOnes.Add(ticket);
                }
            }

            this.validTickets = validOnes.ToArray();
            return errorRate;
        }

        public override long QuestionB()
        {
            for (int index = 0; index < this.myTicket.Length; index++)
            {
                for (int r = 0; r < this.rules.Length; r++)
                {
                    Rule rule = this.rules[r];
                    foreach (int[] ticket in this.validTickets)
                    {
                        if (!rule.IsValid(ticket[index])) {
                            bool updateIndex = this.fieldsRules[index].Remove(r)
                                && this.fieldsRules[index].Count == 1;
                            bool updateRule = this.ruleIndex[r].Remove(index)
                                && this.ruleIndex[r].Count == 1;

                            if (updateIndex)
                            {
                                this.AssignFieldRule(index, this.fieldsRules[index][0]);
                            }

                            if (updateRule)
                            {
                                this.AssignFieldRule(this.ruleIndex[r][0], r);
                            }
                        }
                    }
                }
            }

            int[] departuresFields = this.rules.Select((rule, i) => (rule.Name, this.ruleIndex[i]))
                .Where(rule => rule.Name.StartsWith("departure "))
                .Select(rule => rule.Item2[0])
                .ToArray();

            long result = 1L;
            foreach (int i in departuresFields)
            {
                result *= this.myTicket[i];
            }

            return result;
        }

        private void AssignFieldRule(int fieldIndex, int ruleIndex)
        {
            for (int i = 0; i < this.fieldsRules.Length; i++)
            {
                if (i != fieldIndex)
                {
                    if (this.fieldsRules[i].Remove(ruleIndex)
                        && this.fieldsRules[i].Count == 1)
                    {
                        AssignFieldRule(i, this.fieldsRules[i][0]);
                    }
                }
                else
                {
                    this.fieldsRules[i] = new List<int>(1) { ruleIndex };
                }
            }

            for (int i = 0; i < this.ruleIndex.Length; i++)
            {
                if (i != ruleIndex)
                {
                    if (this.ruleIndex[i].Remove(fieldIndex)
                        && this.ruleIndex[i].Count == 1)
                    {
                        AssignFieldRule(this.ruleIndex[i][0], i);
                    }
                }
                else
                {
                    this.ruleIndex[i] = new List<int>(1) { fieldIndex };
                }
            }
        }

        private long GetTicketErrorRate(int[] ticket)
        {
            long errorRate = 0;
            foreach (int value in ticket)
            {
                if (IsInvalidForAllRules(value))
                {
                    errorRate += value;
                    break;
                }
            }

            return errorRate;

        }
        private bool IsInvalidForAllRules(int value)
        {
            foreach (Rule rule in this.rules)
            {
                if (rule.IsValid(value))
                {
                    return false;
                }
            }

            return true;
        }

        private void ParseInput()
        {
            //this.lines = new string[]
            //{
            //    "class: 0-1 or 4-19",
            //    "row: 0-5 or 8-19",
            //    "seat: 0-13 or 16-19",
            //    "",
            //    "your ticket:",
            //    "11,12,13",
            //    "",
            //    "nearby tickets:",
            //    "3,9,18",
            //    "15,1,5",
            //    "5,14,9"
            //};
            Regex ruleRegex = new Regex(@"^([a-z\ ]+): (\d+)\-(\d+) or (\d+)\-(\d+)$");
            List<Rule> rules = new List<Rule>();
            int i = 0;
            for (; i < this.lines.Length; i++)
            {
                if (this.lines[i].Trim() == "")
                {
                    break;
                }

                Match match = ruleRegex.Match(this.lines[i]);
                rules.Add(new Rule(
                    match.Groups[1].ToString(),
                    match.Groups[2].ToInt(),
                    match.Groups[3].ToInt(),
                    match.Groups[4].ToInt(),
                    match.Groups[5].ToInt()
                ));
            }

            this.rules = rules.ToArray();

            i += 2; // "your ticket:"
            myTicket = lines[i].Split(',').Select(int.Parse).ToArray();

            i += 3; // "nearby tickets:"
            List<int[]> tickets = new List<int[]>();
            for (; i < this.lines.Length; i++)
            {
                tickets.Add(lines[i].Split(',').Select(int.Parse).ToArray());
            }

            this.allTickets = tickets.ToArray();

            int[] range = Enumerable.Range(0, this.myTicket.Length).Select((o, i) => i).ToArray();
            this.fieldsRules = this.myTicket.Select(r => range.ToList()).ToArray();
            this.ruleIndex = this.rules.Select(r => range.ToList()).ToArray();
        }
    }

    public struct Rule
    {
        string name;
        int a, b, c, d;

        public Rule(string name, int a, int b, int c, int d)
        {
            this.name = name;
            this.a = a;
            this.b = b;
            this.c = c;
            this.d = d;
        }

        public string Name { get { return this.name; } }

        public bool IsValid(int value)
        {
            return (a <= value && value <= b)
                || (c <= value && value <= d);
        }
    }
}
