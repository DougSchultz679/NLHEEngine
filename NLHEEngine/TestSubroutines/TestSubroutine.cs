using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLHEEngine.Models;
using NLHEEngine.Subroutines;

namespace NLHEEngine.TestSubroutines
{
    public class TestSubroutine
    {
        public CardReporter CR = new CardReporter();

        public void TestGame()
        {
            List<Player> samplePlayers = new List<Player>();
            samplePlayers.Add(new Player("Sam", 1000, 1));
            samplePlayers.Add(new Player("Jack", 1000, 2));
            samplePlayers.Add(new Player("Jill", 1000, 3));
            samplePlayers.Add(new Player("Bob", 1000, 4));
            //samplePlayers.Add(new Player("Joe", 1000, 5));
            //samplePlayers.Add(new Player("Steve", 1000, 6));
            //samplePlayers.Add(new Player("Doug", 1000, 7));
            //samplePlayers.Add(new Player("Albert", 1000, 8));
            //samplePlayers.Add(new Player("Jimmy", 1000, 9));

            Game newGame = new Game(samplePlayers, 50);
            newGame.Deal();
            newGame.Deal();
            newGame.Deal();
            newGame.Deal();
            ShowBoard(newGame);
            ShowPlayerCards(newGame);
            newGame.ShowdownSinglePot();
            Console.Write("Winners are: ");
            foreach (var p in newGame.WinningPlayers)
                Console.WriteLine("{0}",p.Handle);

        }

        public void GameStateReporter(Game theGame)
        {

        }

        public void ShowPlayerNames(Game theGame)
        {
            string[] playerHandles = new string[theGame.PlayersAtTable.Count];
            for (int i = 0; i < playerHandles.Length; i++)
                playerHandles[i] = theGame.PlayersAtTable[i].Handle;
            Console.WriteLine("Players in the game: {0}", String.Join(", ", playerHandles));
        }

        public void ShowPlayerCards(Game theGame)
        {
            foreach (var p in theGame.PlayersAtTable)
            {
                if (p.HoleCards[0] != null)
                {
                    Console.WriteLine("{0} holds {1}{2}, {3}{4}",
                                        p.Handle,
                                        CR.GetFace(p.HoleCards[0]),
                                        CR.GetSuit(p.HoleCards[0]),
                                        CR.GetFace(p.HoleCards[1]),
                                        CR.GetSuit(p.HoleCards[1])
                                        );
                } else Console.WriteLine("{0} holds no cards yet.", p.Handle);
            }
        }

        public void ShowBoard(Game theGame)
        {
            Console.Write("Board holds: ");

            if (theGame.BoardCards[4] != null)
            {
                for (int i = 0; i < 5; i++)
                {
                    Console.Write("{0}{1}, ",
                        CR.GetFace(theGame.BoardCards[i]),
                        CR.GetSuit(theGame.BoardCards[i])
                        );
                }
            } else if (theGame.BoardCards[3] != null)
            {
                for (int i = 0; i < 4; i++)
                {
                    Console.Write("{0}{1}, ",
                        CR.GetFace(theGame.BoardCards[i]),
                        CR.GetSuit(theGame.BoardCards[i])
                        );
                }
            } else if (theGame.BoardCards[2] != null)
            {
                for (int i = 0; i < 3; i++)
                {
                    Console.Write("{0}{1}, ",
                        CR.GetFace(theGame.BoardCards[i]),
                        CR.GetSuit(theGame.BoardCards[i])
                        );

                }
            } else Console.Write("Nothing yet!\n");

            Console.WriteLine();
        }

        public void TestDeck()
        {
            Deck testDeck = new Deck();

            Console.WriteLine("Card data of test");
            foreach (var c in testDeck.Cards)
            {
                Console.WriteLine("{0}: {1}", c.SuitValue, c.FaceValue);
            }
        }

        //TODO: Convert this block to tests.
        //public void TestHandEvaluation()
        //{
        //    Console.WriteLine("In Test Hand Evaluation\n\n");
        //    HandEvaluator HE = new HandEvaluator();
        //    var testHands = MakeSevenRandomTestHands();
        //    byte isFlush;
        //    foreach (var th in testHands)
        //    {
        //        Console.WriteLine("\nNext Hand:");
        //        foreach (var c in th.SevCards)
        //            Console.WriteLine("{0}: {1}", c.SuitValue, c.FaceValue);
        //        isFlush = HE.IsFlush(th);
        //        Console.WriteLine("Straight strength: {0}", HE.IsStraight(th));
        //        Console.WriteLine("Is flush?: {0}", isFlush);
        //        Console.WriteLine("Straight flush strength: {0}",
        //            String.Join(" ", HE.GetStraightFlush(th, isFlush)));
        //        Console.WriteLine("Flush strength: {0}",
        //            String.Join(" ", HE.GetFlushStrength(th, isFlush)));
        //        Console.WriteLine("Group based hand strength: {0}",
        //            String.Join(" ", HE.GetMatchStrength(th)));
        //        Console.WriteLine("Total Strength: {0}",
        //            String.Join(" ",HE.GetStrength(th)));
        //    }
        //}

        public void TestShowdown()
        {
            Console.WriteLine("In Test Showdown\n");

            HandEvaluator HE = new HandEvaluator();
            HandForEval[] testHands = MakeRandomTestHands();

            for (int i = 0; i < testHands.Length - 1; i += 2)
            {
                Console.WriteLine("Next Showdown");
                Card c1, c2;
                for (int j = 0; j < 7; j++)
                {
                    c1 = testHands[i].SevCards[j];
                    c2 = testHands[i + 1].SevCards[j];
                    Console.WriteLine("{0}: {1}\t {2}: {3}",
                        c1.SuitValue, c1.FaceValue, c2.SuitValue, c2.FaceValue);
                }
                Console.WriteLine("Showdown Winner: {0}",
                    testHands[i].CompareTo(testHands[i + 1]));
                Console.WriteLine("A Hand Strength: {0}", String.Join(" ", testHands[i].HandStrength));
                Console.WriteLine("B Hand Strength: {0}", String.Join(" ", testHands[i + 1].HandStrength));

            }
        }

        public HandForEval[] MakeRandomTestHands()
        {
            Deck newDeck = new Deck();
            int deckIdx = 0;
            HandForEval[] retSet = new HandForEval[6];

            for (int i = 0; i < retSet.Length; i++)
            {
                retSet[i] = new HandForEval(
                    newDeck.Cards[deckIdx],
                    newDeck.Cards[deckIdx + 1],
                    newDeck.Cards[deckIdx + 2],
                    newDeck.Cards[deckIdx + 3],
                    newDeck.Cards[deckIdx + 4],
                    newDeck.Cards[deckIdx + 5],
                    newDeck.Cards[deckIdx + 6]
                    );
                deckIdx += 7;
            }
            return retSet;
        }

        public HandForEval[] MakeExplicitTestHands()
        {
            return new HandForEval[]{
                //str flush wheel
                new HandForEval(
                new Card(14, 1),
                new Card(9, 1),
                new Card(2, 1),
                new Card(3, 1),
                new Card(4, 1),
                new Card(5, 1),
                new Card(9, 4)
                ),
                //wheel
                new HandForEval(
                new Card(14, 1),
                new Card(9, 1),
                new Card(2, 1),
                new Card(3, 2),
                new Card(4, 4),
                new Card(5, 3),
                new Card(9, 4)
                ),
                //straight with 2 pair
                new HandForEval(
                new Card(5, 1),
                new Card(6, 1),
                new Card(7, 1),
                new Card(7, 2),
                new Card(8, 4),
                new Card(9, 3),
                new Card(6, 4)
                ),
                //straight with set
                new HandForEval(
                new Card(6, 1),
                new Card(7, 1),
                new Card(7, 1),
                new Card(7, 2),
                new Card(8, 4),
                new Card(9, 3),
                new Card(10, 4)
                ),
                 //straight with pair
                new HandForEval(
                new Card(5, 1),
                new Card(6, 1),
                new Card(7, 1),
                new Card(7, 2),
                new Card(8, 4),
                new Card(9, 3),
                new Card(14, 4)
                ),
                 //3 pairs
                new HandForEval(
                new Card(5, 1),
                new Card(5, 1),
                new Card(7, 1),
                new Card(7, 2),
                new Card(2, 4),
                new Card(14, 3),
                new Card(14, 4)
                ),
                //2 sets
                new HandForEval(
                new Card(5, 1),
                new Card(5, 1),
                new Card(7, 1),
                new Card(7, 2),
                new Card(7, 4),
                new Card(5, 3),
                new Card(14, 4)
                ),
                //pair of 2s
                new HandForEval(
                new Card(2, 1),
                new Card(2, 1),
                new Card(7, 1),
                new Card(8, 1),
                new Card(9, 1),
                new Card(14, 3),
                new Card(14, 4)
                ),
                //straight Flush
                new HandForEval(
                new Card(5, 1),
                new Card(6, 1),
                new Card(7, 1),
                new Card(8, 1),
                new Card(9, 1),
                new Card(14, 3),
                new Card(14, 4)
                ),
                // broken flush
                new HandForEval(
                new Card(7, 1),
                new Card(8, 1),
                new Card(4, 1),
                new Card(2, 1),
                new Card(13, 1),
                new Card(14, 3),
                new Card(14, 4)
                ),
                //straight to 6
                new HandForEval(
                new Card(2, 1),
                new Card(3, 1),
                new Card(4, 2),
                new Card(5, 3),
                new Card(6, 4),
                new Card(12, 3),
                new Card(9, 4)
                ),
                //4ofakind
                new HandForEval(
                new Card(5, 1),
                new Card(3, 1),
                new Card(9, 1),
                new Card(9, 2),
                new Card(9, 3),
                new Card(9, 4),
                new Card(14, 4)
                ),
                //boat
                new HandForEval(
                new Card(5, 1),
                new Card(6, 1),
                new Card(7, 1),
                new Card(7, 2),
                new Card(7, 4),
                new Card(14, 3),
                new Card(14, 4)
                )
            };
        }

    }
}
