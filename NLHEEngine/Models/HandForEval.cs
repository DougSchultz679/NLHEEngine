using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLHEEngine.Models
{
    public class HandForEval
    {
        public Card[] SevCards = new Card[7];

        public int[] HandStrength;

        public HandForEval(params Card[] crds)
        {
            SevCards = this.SortCards(crds);
        }

        //TODO: Ensure the right winner is chosen. basically implements CompareTo
        int Showdown(object obj)
        {
            HandForEval B = (HandForEval)obj;
            try
            {
                for (int i = 0; i < 5; i++)
                {
                    if (this.HandStrength[i] < B.HandStrength[i])
                        return 1;
                    else if (this.HandStrength[i] > B.HandStrength[i])
                        return -1;
                }
            } catch(IndexOutOfRangeException ex)
            {
                return 0;
            }
            return 0;
        }


        public Card[] SortCards(params Card[] crds)
        {
            List<Card> RetCrds = new List<Card>(crds);
            RetCrds.Sort(Card.sortCardsDescending());
            return RetCrds.ToArray();
        }
    }
}
