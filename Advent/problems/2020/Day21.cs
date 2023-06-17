using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Advent.Utils;

namespace Advent2020
{
    class Day21: BaseDay2020
    {
        private static string[] TestLines = new string[] {
            "mxmxvkd kfcds sqjhc nhms (contains dairy, fish)",
            "trh fvjkl sbzzf mxmxvkd (contains dairy)",
            "sqjhc fvjkl (contains soy)",
            "sqjhc mxmxvkd sbzzf (contains fish)"
        };
        
        private (string[] ingredients, string[] allergens)[] foodList;

        private Dictionary<string, string> allergensIngredients;

        public Day21(): base(21) {
            this.ParseInput(this.lines);
            this.ComputeAllergenIngredients();
        }

        public override long QuestionA()
        {
            string[] allergenIngredients = this.allergensIngredients.Keys.ToArray();
            long goodIngredientsCount = 0L;
            foreach ((string[] ingredients, string[] allergens) in this.foodList)
            {
                goodIngredientsCount += ingredients.Count(ingr => !allergenIngredients.Contains(ingr));
            }

            return goodIngredientsCount;
        }

        public override long QuestionB()
        {
            string canonicalDangerousList = string.Join(",",
                this.allergensIngredients.OrderBy(pair => pair.Value).Select(pair => pair.Key));
            Console.WriteLine("Answer => " + canonicalDangerousList);
            return 0;
        }

        private void ComputeAllergenIngredients()
        {
            this.allergensIngredients = new Dictionary<string, string>();
            Dictionary<string, List<string>> allergensCandidates = new Dictionary<string, List<string>>();
            foreach ((string[] ingredients, string[] allergens) in this.foodList)
            {
                foreach (string allergen in allergens)
                {
                    if (!allergensCandidates.ContainsKey(allergen))
                    {
                        allergensCandidates.Add(allergen, ingredients.ToList());
                    }
                    else
                    {
                        List<string> intersection = allergensCandidates[allergen].Intersect(ingredients).ToList();
                        allergensCandidates.AddOrReplace(allergen, intersection);
                        if (intersection.Count == 0)
                        {
                            throw new Exception();
                        }
                    }

                    if (allergensCandidates[allergen].Count == 1)
                    {
                        this.allergensIngredients[allergensCandidates[allergen][0]] = allergen;
                    }
                }
            }

            string[] allAllergens = allergensCandidates.Keys.ToArray();
            while (allergensCandidates.Max(pair => pair.Value.Count > 1))
            {
                string[] assignedIngredients = this.allergensIngredients.Keys.ToArray();
                foreach (string ing in assignedIngredients)
                {
                    string assignedAllergen = this.allergensIngredients[ing];
                    foreach (string allergen in allergensCandidates.Keys)
                    {
                        if (assignedAllergen == allergen)
                        {
                            continue;
                        }

                        if (allergensCandidates[allergen].Remove(ing)
                            && allergensCandidates[allergen].Count == 1)
                        {
                            this.allergensIngredients[allergensCandidates[allergen][0]] = allergen;
                        }
                    }
                }
            }
        }

        private void ParseInput(string[] lines)
        {
            List<(string[] ingredients, string[] allergens)> food = new List<(string[] ingredients, string[] allergens)>();
            Regex foodRegex = new Regex(@"^([a-z ]*) (\(contains (.*)\))$");
            foreach (string line in lines)
            {
                Match match = foodRegex.Match(line);
                string[] ingredients = match.Groups[1].ToString().Split(" ");
                string[] allergens = match.Groups[3].ToString().Split(", ");
                food.Add((ingredients, allergens));
            }

            this.foodList = food.ToArray();
        }
    }
}
