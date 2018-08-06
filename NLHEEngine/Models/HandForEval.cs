using NLHEEngine.Subroutines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLHEEngine.Models
{
    public class HandForEval : IComparable
    {
        public Card[] SevCards = new Card[7];
        private HandEvaluator _handEvaluator = new HandEvaluator();

        public byte[] HandStrength;

        public HandForEval(params Card[] crds)
        {
            SevCards = this.SortCards(crds);
            HandStrength = _handEvaluator.GetStrength(this);
        }

        public int CompareTo(object obj)
        {
            HandForEval B = (HandForEval)obj;
            try
            {
                for (int i = 0; i < 6; i++)
                {
                    if (this.HandStrength[i] > B.HandStrength[i])
                        return 1;
                    else if (B.HandStrength[i] > this.HandStrength[i])
                        return -1;
                }
            } catch (IndexOutOfRangeException ex)
            {
                return 0;
            }
            return 0;
        }

        private Card[] SortCards(params Card[] crds)
        {
            List<Card> RetCrds = new List<Card>(crds);
            RetCrds.Sort(Card.sortCardsDescending());
            return RetCrds.ToArray();
        }
    }
}
