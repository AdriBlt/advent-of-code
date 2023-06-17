using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Advent.Utils;

namespace Advent2020
{
    class Day14: BaseDay2020
    {
        private string MaskPrefix = "mask = ";
        private Regex memoryRegex = new Regex(@"^mem\[(\d+)\] = (\d+)$");
        private Dictionary<long, long> memory = new Dictionary<long, long>();
        private string mask = "";

        public Day14(): base(14) { }

        public override long QuestionA()
        {
            return ComputeMemorySum((index, value) => {
                long modifiedValue = ApplyMaskVersion1(value, mask);
                memory.AddOrReplace(index, modifiedValue);
            });
        }

        public override long QuestionB()
        {
            return ComputeMemorySum((index, value) => {
                long[] impactedIndexes = ApplyMaskVersion2(index, mask);
                foreach (long idx in impactedIndexes)
                {
                    memory.AddOrReplace(idx, value);
                }
            });
        }

        private long ComputeMemorySum(Action<long, long> updateMemory)
        {
            this.Reset();
            foreach (string line in this.lines)
            {
                if (line.StartsWith(MaskPrefix))
                {
                    mask = line.Substring(MaskPrefix.Length);
                    continue;
                }

                Match match = memoryRegex.Match(line);
                long index = match.Groups[1].ToLong();
                long value = match.Groups[2].ToLong();
                updateMemory(index, value);
            }

            return memory.Values.Sum();
        }

        private void Reset()
        {
            this.memory.Clear();
            this.mask = "";
        }

        private static long ApplyMaskVersion1(long value, string mask)
        {
            if (mask.Length == 0)
            {
                return value;
            }

            bool[] binaries = value.ToBinary(mask.Length);
            for (int i = 0; i < binaries.Length; i++)
            {
                if (mask[i] == '1')
                {
                    binaries[i] = true;
                }
                else if (mask[i] == '0')
                {
                    binaries[i] = false;
                }
            }

            return binaries.ToLong();
        }

        private static long[] ApplyMaskVersion2(long index, string mask)
        {
            if (mask.Length == 0)
            {
                return new long[] { index };
            }

            List<bool[]> indexes = new List<bool[]>
            {
                0L.ToBinary(mask.Length)
            };
            bool[] binaries = index.ToBinary(mask.Length);
            for (int i = 0; i < binaries.Length; i++)
            {
                if (mask[i] == '1')
                {
                    for (int j = 0; j < indexes.Count; j++)
                    {
                        indexes[j][i] = true;   
                    }
                }
                else if (mask[i] == '0')
                {
                    for (int j = 0; j < indexes.Count; j++)
                    {
                        indexes[j][i] = binaries[i];
                    }
                }
                else if (mask[i] == 'X')
                {
                    indexes.AddRange(indexes.Select(index1 =>
                        {
                            bool[] index2 = (bool[])index1.Clone();
                            index2[i] = true;
                            return index2;
                        }).ToArray());
                }
            }

            return indexes.Select(idx => idx.ToLong()).ToArray();
        }
    }
}
