using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NLHEEngine.Models.Card;

namespace NLHEEngine.Models
{
    public class Deck
    {
        public Card[] Cards = new Card[52];
        public int NextCard { get; set; }

        public Deck()
        {
            Cards = MakeDeck();
        }

        private Card[] MakeDeck()
        {
            Card[] newDeck = NewDeck();

            Card[] shuffledDeck = new Card[52];

            int[] deckOrder = produceOrder();

            for (int i = 0; i < 52; i++)
            {
                shuffledDeck[i] = newDeck[deckOrder[i] - 1];
            }

            return shuffledDeck;

            Card[] NewDeck()
            {
                Card[] Deck = new Card[52];
                int currFace, currSuit;
                currFace = 2;
                currSuit = 1;

                for (int i = 0; i < Deck.Length; i++)
                {
                    if (currFace > 14)
                    {
                        currFace = 2;
                        ++currSuit;
                    }

                    Deck[i] = new Card(currFace, currSuit);
                    ++currFace;
                }
                return Deck;
            }

            int[] produceOrder()
            {
                int[] Order = new int[52];
                int ctr, TryIndex;
                Random rng = new Random(DateTimeOffset.UtcNow.Second);
                ctr = TryIndex = 0;

                //somewhat randomly distribute the order of the cards
                while (ctr < 50)
                {
                    TryIndex = rng.Next(0, 51);
                    if (Order[TryIndex] == 0)
                    {
                        Order[TryIndex] = ctr;
                        ++ctr;
                    }
                }

                // make sure we have an entry for all order values 
                for (int j = 0; j < 52; j++)
                {
                    if (Order[j] == 0)
                    {
                        Order[j] = ctr;
                        ctr++;
                    }
                }

                //shuffle last 10 cards into the deck
                var rng2 = new Random(DateTime.Now.Minute);
                int idx, tmp;
                for (int k = 51; k > 41; k--)
                {
                    idx = rng2.Next(0, 51);
                    tmp = Order[k];
                    Order[k] = Order[idx];
                    Order[idx] = tmp;
                }

                return Order;
            }
        }

    }
}
