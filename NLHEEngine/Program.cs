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

            //  ****** Test Showdown loop *******
            //string input = "y";
            //while (input.Equals("y"))
            //{
            //    TS.TestShowdown();
            //    Console.WriteLine("Run Showdown Test? y/n");
            //    input = Console.ReadLine();
            //}

            //TS.TestDeck();
            //TS.TestHandEvaluation();

            string input = "y";
            while (input.Equals("y"))
            {
                TS.TestGame();
                Console.WriteLine("Run Showdown Test? y/n");
                input = Console.ReadLine();
            }
            

            //Console.WriteLine( "***** In game loop ***** ");

            Console.ReadLine();
        }
    }
}
