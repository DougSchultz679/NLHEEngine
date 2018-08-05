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
        public void TestDeck()
        {
            Deck testDeck = new Deck();

            Console.WriteLine("Card data of test");
            foreach (var c in testDeck.Cards)
            {
                Console.WriteLine("{0}: {1}", c.SuitValue, c.FaceValue);
            }
        }

        public void TestSort()
        {
            Console.WriteLine("In Test Sort\n\n");
            HandEvaluator HE = new HandEvaluator();
            var testHands = MakeExplicitTestHands();
            foreach(var th in testHands)
            {
                Console.WriteLine("Next Hand:");
                foreach (var c in th.SevCards)
                    Console.WriteLine("{0}: {1}", c.SuitValue, c.FaceValue);

                Console.WriteLine("Is straight?: {0}", HE.IsStraight(th));
                Console.WriteLine("Is flush?: {0}", HE.IsFlush(th));
            }
            

        }

        public HandForEval[] MakeExplicitTestHands()
        {
            return new HandForEval[]{
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
