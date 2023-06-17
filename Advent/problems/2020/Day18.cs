using System;
using System.Linq;
using System.Collections.Generic;
using Advent.Utils;
using System.Net.NetworkInformation;

namespace Advent2020
{
    class Day18: BaseDay2020
    {
        private static string[] TestLines = new string[]
        {
            "2 * 3 + (4 * 5)",
            "5 + (8 * 3 + 9 + 3 * 4 * 3)",
            "5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))",
            "((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2"
        };

        public Day18(): base(18) { }

        public override long QuestionA()
        {
            return this.ComputeSum(LeftToRightLineParser.Parse);
        }

        public override long QuestionB()
        {
            return this.ComputeSum(AdditionFirstLineParser.Parse);
        }

        private long ComputeSum(Func<string, long> lineParser)
        {
            return this.lines.Select(lineParser).Sum();
        }
    }

    struct LeftToRightLineParser
    {
        string line;
        int index;

        public static long Parse(string line)
        {
            return new LeftToRightLineParser(line).Parse();
        }

        public LeftToRightLineParser(string line)
        {
            this.line = line;
            this.index = 0;
        }

        public long Parse()
        {
            long returnedValue = 0;
            long currentValue = 0;
            Operation operation = Operation.Undefined;
            Action ApplyOperation = () => {
                switch (operation)
                {
                    case Operation.Addition:
                        returnedValue += currentValue;
                        break;
                    case Operation.Multiplication:
                        returnedValue *= currentValue;
                        break;
                    case Operation.Undefined:
                        break;
                }
                currentValue = 0;
                operation = Operation.Undefined;
            };

            while (this.index < this.line.Length)
            {
                char letter = this.line[this.index++];
                switch (letter)
                {
                    case ' ':
                        continue;

                    case '+':
                        ApplyOperation();
                        operation = Operation.Addition;
                        break;

                    case '*':
                        ApplyOperation();
                        operation = Operation.Multiplication;
                        break;

                    case ')':
                        ApplyOperation();
                        return returnedValue;

                    case '(':
                        long nextValue = this.Parse();
                        if (operation == Operation.Undefined)
                        {
                            returnedValue = nextValue;
                        } else
                        {
                            currentValue = nextValue;
                            ApplyOperation();
                        }
                        break;

                    default:
                        int digit = int.Parse(letter.ToString());
                        if (operation == Operation.Undefined)
                        {
                            returnedValue = 10 * returnedValue + digit;
                        }
                        else
                        {
                            currentValue = 10 * currentValue + digit;
                        }
                        break;
                }
            }

            ApplyOperation();
            return returnedValue;
        }

        enum Operation
        {
            Undefined,
            Addition,
            Multiplication
        }
    }

    struct AdditionFirstLineParser
    {
        public static long Parse(string line)
        {
            int start = line.IndexOf('(');
            if (start < 0)
            {
                return ParseNoParenthesis(line);
            }

            int depth = 1;
            int end = start + 1;
            for (; end < line.Length; end++)
            {
                if (line[end] == '(')
                {
                    depth++;
                }
                else if (line[end] == ')')
                {
                    depth--;
                }

                if (depth == 0)
                {
                    break;
                }
            }

            string inBetween = line.Substring(start + 1, end - start - 1);
            long firstEvalutation = Parse(inBetween);
            string newString = "";
            if (start > 1)
            {
                newString += line.Substring(0, start);
            }

            newString += firstEvalutation.ToString();

            if (end < line.Length - 1)
            {
                newString += line.Substring(end + 1);
            }

            return Parse(newString);
        }

        private static long ParseNoParenthesis(string line)
        {
            List<string> parts = line.Split(' ').ToList();
            int i;
            while ((i = parts.IndexOf("+")) > 0)
            {
                long a = long.Parse(parts[i - 1]);
                long b = long.Parse(parts[i + 1]);
                long sum = a + b;
                parts.RemoveAt(i);
                parts.RemoveAt(i);
                parts[i - 1] = sum.ToString();
            }

            long result = 1L;
            foreach (long number in parts.Where(c => c != "*").Select(long.Parse))
            {
                result *= number;
            }

            return result;
        }
    }
}
