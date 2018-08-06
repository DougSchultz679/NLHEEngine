using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLHEEngine.TestSubroutines;

namespace NLHEEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(" ***** NLHE Engine ***** ");

            TestSubroutine TS = new TestSubroutine();

            //TS.TestDeck();
            //TS.TestHandEvaluation();
            //TS.TestShowdown();
            TS.TestGame();

            //Console.WriteLine( "***** In game loop ***** ");

            Console.ReadLine();
        }
    }
}
