using NLHEEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLHEEngine.Subroutines
{
    public class HandEvaluator
    {
        public bool IsFlush(HandForEval hnd)
        {
            int numClub, numDia, numHrt, numSpd;
            numClub = numDia = numHrt = numSpd = 0;
            foreach (Card c in hnd.SevCards)
            {
                switch (c.SuitValue)
                {
                    case 1: numClub++; break;
                    case 2: numSpd++; break;
                    case 3: numHrt++; break;
                    case 4: numDia++; break;
                }
            }
            if (numClub >= 5 || numDia >= 5 || numHrt >= 5 || numSpd >= 5) return true;
            else return false;
        }

        public bool IsStraight(HandForEval hnd)
        {
            int strCnt = 1;
            for (int i = 0; i < hnd.SevCards.Length; i++)
            {
                if (hnd.SevCards[i].FaceValue == hnd.SevCards[i + 1].FaceValue + 1)
                {
                    strCnt++;
                } else
                {
                    strCnt = 1;
                }

                if (i >= 3 && strCnt < 2) return false;
                if (strCnt == 5) return true;
            }
            return false;
        }
    }
}
