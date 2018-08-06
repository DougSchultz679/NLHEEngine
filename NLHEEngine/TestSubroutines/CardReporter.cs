using NLHEEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLHEEngine.TestSubroutines
{
    public class CardReporter
    {
        public string GetSuit(Card c)
        {
            switch (c.SuitValue)
            {
                case 1: return "C";
                case 2: return "S";
                case 3: return "H";
                case 4: return "D";
            }
            return "";
        }

        public string GetFace(Card c)
        {
            switch (c.FaceValue)
            {
                case 14: return "A";
                case 13: return "K";
                case 12: return "Q";
                case 11: return "J";
                case 10: return "T";
                default: return c.FaceValue.ToString();
            }
        }
    }
}
