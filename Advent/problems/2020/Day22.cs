using System;
using System.Collections.Generic;
using System.Linq;
using Advent.Utils;

namespace Advent2020
{
    class Day22: BaseDay2020
    {
        private static string[] TestLines = new string[]
        {
            "Player 1:",
            "9",
            "2",
            "6",
            "3",
            "1",
            "",
            "Player 2:",
            "5",
            "8",
            "4",
            "7",
            "10"
        };
        private int[] deck1, deck2;

        public Day22(): base(22) {
            this.ParseInput(this.lines);
        }

        public override long QuestionA()
        {
            return GetScoreGame(PlayRegularMatchWiningDeck);
        }

        public override long QuestionB()
        {
            return GetScoreGame(PlayRecursiveMatchWiningDeck);
        }

        private long GetScoreGame(Action<LinkedList<int>, LinkedList<int>> game)
        {
            LinkedList<int> cards1 = new LinkedList<int>(this.deck1);
            LinkedList<int> cards2 = new LinkedList<int>(this.deck2);
            game(cards1, cards2);
            LinkedList<int> winningDeck = cards1.Count > 0 ? cards1 : cards2;
            return GetScore(winningDeck);
        }

        private static long GetScore(LinkedList<int> deck)
        {
            return deck.Select((c, i) => (long)c * (deck.Count - i)).Sum();
        }

        private static void PlayRecursiveMatchWiningDeck(LinkedList<int> cards1, LinkedList<int> cards2)
        {
            List<string> seenConfigurations = new List<string>();
            while (cards1.Count > 0 && cards2.Count > 0)
            {
                string configuration = string.Join(",", cards1) + "-" + string.Join(",", cards2);
                if (seenConfigurations.Contains(configuration))
                {
                    // LOOP, player1 wins
                    return;
                }
                else
                {
                    seenConfigurations.Add(configuration);
                }

                int c1 = cards1.First.Value;
                int c2 = cards2.First.Value;
                cards1.RemoveFirst();
                cards2.RemoveFirst();

                bool player1Wins;
                if (cards1.Count >= c1 && cards2.Count >= c2)
                {
                    LinkedList<int> subDeck1 = new LinkedList<int>(cards1.Take(c1));
                    LinkedList<int> subDeck2 = new LinkedList<int>(cards2.Take(c2));
                    PlayRecursiveMatchWiningDeck(subDeck1, subDeck2);
                    player1Wins = subDeck1.Count > 0;
                }
                else
                {
                    player1Wins = c1 > c2;
                }

                if (player1Wins)
                {
                    cards1.AddLast(c1);
                    cards1.AddLast(c2);
                }
                else
                {
                    cards2.AddLast(c2);
                    cards2.AddLast(c1);
                }
            }
        }

        private static void PlayRegularMatchWiningDeck(LinkedList<int> cards1, LinkedList<int> cards2)
        {
            while (cards1.Count > 0 && cards2.Count > 0)
            {
                int c1 = cards1.First.Value;
                int c2 = cards2.First.Value;
                cards1.RemoveFirst();
                cards2.RemoveFirst();
                if (c1 > c2)
                {
                    cards1.AddLast(c1);
                    cards1.AddLast(c2);
                }
                else
                {
                    cards2.AddLast(c2);
                    cards2.AddLast(c1);
                }
            }
        }

        private void ParseInput(string[] lines)
        {
            int[][] decks = ParsingUtils.SplitGroupsByEmptyLines(lines)
                .Select(line => line[1..].Select(int.Parse).ToArray())
                .ToArray();
            this.deck1 = decks[0];
            this.deck2 = decks[1];
        }
    }
}
