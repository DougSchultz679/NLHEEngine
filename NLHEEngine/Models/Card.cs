using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLHEEngine.Models
{
    public class Card
    {
        //TODO: determine if these enums are useful and or can be used to do useful things
        public enum Suit { Club = 1, Spade = 2, Heart = 3, Diamond = 4 }
        public enum Face { Two = 2, Three = 3, Four = 4, Five = 5, Six = 6, Seven = 7, Eight = 8, Nine = 9, Ten = 10, Jack = 11, Queen = 12, King = 13, Ace = 14 }
        public byte FaceValue { get; set; }
        public byte SuitValue { get; set; }

        public Card(byte _faceValue, byte _suitValue)
        {
            FaceValue = _faceValue;
            SuitValue = _suitValue;
        }
        
        public static IComparer<Card> sortCardsDescending()
        {
            return new compareCardFace();
        }

        private class compareCardFace : IComparer<Card>
        {
            int IComparer<Card>.Compare(Card a, Card b)
            {
                if (a.FaceValue < b.FaceValue) return 1;
                else if (b.FaceValue < a.FaceValue) return -1;
                else return 0;
            }
        }
    }


}
