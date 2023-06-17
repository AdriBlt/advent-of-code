using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Advent.Utils;

namespace Advent2020
{
    class Day04: BaseDay2020
    {
        public Day04(): base(4) { }

        public override long QuestionA()
        {
            return parsePassports().Count(passport => passport.HasRequiredKeys());
        }

        public override long QuestionB()
        {
            return parsePassports().Count(passport => passport.ValidatePassport());
        }

        private Dictionary<string, string>[] parsePassports()
        {
            return ParsingUtils.SplitGroupsByEmptyLines(lines).Select((string[] group) => {
                Dictionary<string, string> passport = new Dictionary<string, string>();
                string[] fields = string.Join(" ", group).Split(" ");
                foreach (string field in fields)
                {
                    string[] info = field.Split(":");
                    if (info.Length == 2)
                    {
                        passport.Add(info[0], info[1]);
                    }
                }
                return passport;
            }).ToArray();
        }
    }

    static class PassportExtensions
    {
        private static readonly string[] RequiredKeys = new string[] { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid" };

        private static readonly Regex HexRegex = new Regex("^#([a-f0-9]{6})$");
        private static readonly string[] AllowedEyeColors = new string[] { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" };

        private static bool ValidateInt(string str, int min, int max) => int.TryParse(str, out int number) && number >= min && number <= max;

        private static bool ValidateByr(string byr) => ValidateInt(byr, 1920, 2002);
        private static bool ValidateIyr(string iyr) => ValidateInt(iyr, 2010, 2020);
        private static bool ValidateEyr(string eyr) => ValidateInt(eyr, 2020, 2030);
        private static bool ValidateHgt(string hgt)
        {
            if (hgt.EndsWith("cm"))
            {
                return ValidateInt(hgt.Substring(0, hgt.Length - 2), 150, 193);
            }

            if (hgt.EndsWith("in"))
            {
                return ValidateInt(hgt.Substring(0, hgt.Length - 2), 59, 76);
            }

            return false;
        }
        private static bool ValidateHcl(string hcl) => HexRegex.IsMatch(hcl);
        private static bool ValidateEcl(string ecl) => AllowedEyeColors.Contains(ecl);
        private static bool ValidatePid(string pid) => pid.Length == 9 && long.TryParse(pid, out long id) && id >= 0 && id < 1000000000;

        private static bool ValidateField(this Dictionary<string, string> passport, string fieldKey, Func<string, bool> validationFunction) => 
            passport.TryGetValue(fieldKey, out string fieldValue) && validationFunction(fieldValue);

        public static bool HasRequiredKeys(this Dictionary<string, string> passport) => 
            RequiredKeys.All(passport.ContainsKey);

        public static bool ValidatePassport(this Dictionary<string, string> passport) =>
            passport.ValidateField("byr", ValidateByr) &&
            passport.ValidateField("iyr", ValidateIyr) &&
            passport.ValidateField("eyr", ValidateEyr) &&
            passport.ValidateField("hgt", ValidateHgt) &&
            passport.ValidateField("hcl", ValidateHcl) &&
            passport.ValidateField("ecl", ValidateEcl) &&
            passport.ValidateField("pid", ValidatePid);
    }
}
