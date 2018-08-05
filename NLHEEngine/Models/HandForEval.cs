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

        //TODO: determine if this is useful in any cases
        public int[] HandStrength;

        public HandForEval(params Card[] crds)
        {
            SevCards = this.SortCards(crds);
        }

        //TODO: Ensure the right winner is chosen.
        int IComparable.CompareTo(object obj)
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
            } catch (IndexOutOfRangeException ex)
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
