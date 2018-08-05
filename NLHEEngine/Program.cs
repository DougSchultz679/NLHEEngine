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

            TS.TestSort();

            Console.ReadLine();
        }
    }
}
