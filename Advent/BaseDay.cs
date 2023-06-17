using System;
using System.IO;
using System.Linq;

namespace Advent
{
    abstract class BaseDay
    {
        private int day;
        private int year;
        protected string[] lines;

        public BaseDay(int day, int year)
        {
            this.day = day;
            this.year = year;
            try
            {
                this.lines = File.ReadAllLines(@$"..\..\..\inputs\{year}\input{day.ToString("00")}.txt").ToArray();
            } catch (Exception)
            {
                this.lines = null;
            }
        }

        public abstract long QuestionA();

        public abstract long QuestionB();

        public void PrintSolution()
        {
            Console.WriteLine($"{this.year}/12/{this.day.ToString("00")}");
            Console.WriteLine($"-> a: {this.QuestionA()}");
            Console.WriteLine($"-> b: {this.QuestionB()}");
        }

        public static BaseDay GetDay(int year, int day)
        {
            Type type = Type.GetType($"Advent{year}.Day{day.ToString("00")}");
            return (BaseDay)Activator.CreateInstance(type);
        }

        public static void PrintAllSolutions(int year)
        {
            for (int d = 1; d <= 25; d++)
            {
                BaseDay.GetDay(year, d).PrintSolution();
                Console.WriteLine();
            }
        }
    }
}
