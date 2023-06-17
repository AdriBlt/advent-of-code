using System.Collections.Generic;

namespace Advent.Utils
{
    static class BinaryUtils
    {
        public static bool[] ToBinary(this long number, int size)
        {
            bool[] binaries = new bool[size];
            long remaining = number;
            for (int i = size -1; i >= 0; i--)
            {
                binaries[i] = remaining % 2 == 1;
                remaining /= 2;
            }

            return binaries;
        }

        public static long ToLong(this IEnumerable<bool> binaries)
        {
            long number = 0;
            foreach (bool bin in binaries)
            {
                number <<= 1;
                if (bin)
                {
                    number++;
                }
            }

            return number;
        }
    }
}
