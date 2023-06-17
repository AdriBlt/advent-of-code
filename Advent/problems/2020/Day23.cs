using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent2020
{
    class Day23: BaseDay2020
    {
        private static string TestInput = "389125467";
        private string input = "853192647";

        public Day23(): base(23) {}

        public override long QuestionA()
        {
            CircularLinkedList<long> list = ParseInput(input);
            list.PlayGame(100);
            list.FindValue(1);
            list.RemoveCurrentValue();
            return long.Parse(list.Print());
        }

        public override long QuestionB()
        {
            CircularLinkedList<long> list = ParseInput(input, (List<long> inputList) => {
                long maxValue = inputList.Max();
                for (long k = maxValue + 1; k <= 1000000; k++) {
                    inputList.Add(k);
                }
            });

            list.PlayGame(10000000);
            list.FindValue(1);
            long a = list.NextValue();
            long b = list.NextValue();
            return a * b;
        }

        private static CircularLinkedList<long> ParseInput(string input, Action<List<long>>? inputModification = null)
        {
            List<long> list = input.Select((char c) => long.Parse(c.ToString())).ToList();
            inputModification?.Invoke(list);
            return new CircularLinkedList<long>(list, value => value - 1);
        }
    }

    class CircularLinkedList<T>
    {
        private LinkedList<T> list;
        private LinkedListNode<T> currentNode;
        private LinkedListNode<T>[] indexes;
        private Func<T, long> getIndex;
        
        public CircularLinkedList(IEnumerable<T> collection, Func<T, long> getIndex) {
            this.list = new LinkedList<T>(collection);
            this.currentNode = this.list.First;
            this.getIndex = getIndex;
            this.indexes = new LinkedListNode<T>[collection.Count()];
            this.ForEachNode(node => this.indexes[getIndex(node.Value)] = node);
        }

        public T CurrentValue => this.currentNode.Value;

        public int Count => this.list.Count;

        public T NextValue()
        {
            this.currentNode = this.currentNode.Next != null
                ? this.currentNode.Next
                : this.list.First;
            return this.CurrentValue;
        }

        public T RemoveCurrentValue()
        {
            LinkedListNode<T> node = this.currentNode;
            this.NextValue();
            this.list.Remove(node);
            return node.Value;
        }
        
        public void AddAfterCurrentValue(T value)
        {
            var node = this.list.AddAfter(this.currentNode, value);
            this.indexes[getIndex(node.Value)] = node;
        }

        public void ForEach(Action<T> callback)
        {
            for (long i = 0; i < this.list.Count; i++)
            {
                callback(this.CurrentValue);
                this.NextValue();
            }
        }

        public void ForEachNode(Action<LinkedListNode<T>> callback)
        {
            for (long i = 0; i < this.list.Count; i++)
            {
                callback(this.currentNode);
                this.NextValue();
            }
        }

        public void SetCurrentNode(LinkedListNode<T> node)
        {
            this.currentNode = node;
        }

        public bool FindValue(T value)
        {
            long index = this.getIndex(value);
            if (index < 0 || index >= this.indexes.Length)
            {
                return false;
            }

            this.SetCurrentNode(this.indexes[index]);
            return true;
        }

        public string Print()
        {
            string str = "";
            this.ForEach(value => str += value);
            return str;
        }

        public void AddLast(T value)
        {
            this.list.AddLast(value);
        }
    }

    static class LongCircularLinkedListExtensions
    {
        public static void PlayGame(this CircularLinkedList<long> list, long iterations)
        {
            long listSize = list.Count;

            for (long i = 0; i < iterations; i++)
            {
                long currentValue = list.CurrentValue;
                list.NextValue();

                // Remove 3 Cups
                long a = list.RemoveCurrentValue();
                long b = list.RemoveCurrentValue();
                long c = list.RemoveCurrentValue();

                // Select Destination Cup
                long destinationValue = ComputeDestinationValue(listSize, currentValue, a, b, c);
                list.FindValue(destinationValue);

                // Re-inject cups
                list.AddAfterCurrentValue(c);
                list.AddAfterCurrentValue(b);
                list.AddAfterCurrentValue(a);

                // Select Next Cup
                list.FindValue(currentValue);
                list.NextValue();
            }
        }

        private static long ComputeDestinationValue(long listSize, long currentValue, params long[] removedValues)
        {
            for (long k = currentValue - 1; ; k--)
            {
                if (k == 0)
                {
                    k = listSize;
                }

                if (!removedValues.Contains(k))
                {
                    return k;
                }
            }
        }

        public static long GetMaxValue(this CircularLinkedList<long> list)
        {
            long max = list.CurrentValue;
            list.ForEach(value => {
                if (value > max)
                {
                    max = list.CurrentValue;
                }
            });

            return max;
        }
    }
}
