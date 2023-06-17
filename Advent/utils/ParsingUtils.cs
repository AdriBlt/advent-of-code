using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Advent.Utils
{
    static class ParsingUtils
    {
        public static string[][] SplitGroupsByEmptyLines(string[] lines)
        {
            List<string[]> groups = new List<string[]>();
            List<string> currentGroup = new List<string>();

            foreach (string line in lines)
            {
                if (line.Trim() != "")
                {
                    currentGroup.Add(line);
                }
                else if (currentGroup.Count > 0)
                {
                    groups.Add(currentGroup.ToArray());
                    currentGroup.Clear();
                }
            }

            if (currentGroup.Count > 0)
            {
                groups.Add(currentGroup.ToArray());
            }

            return groups.ToArray();
        }

        public static int ToInt(this Group group)
        {
            return int.Parse(group.ToString());
        }

        public static long ToLong(this Group group)
        {
            return long.Parse(group.ToString());
        }
    }
}
