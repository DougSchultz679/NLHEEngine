using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLHEEngine.Models
{
    public class Game
    {
        public int Id { get; set; }
        public List<Player> PlayersAtTable { get; set; }
        public List<Player> PlayersInHand { get; set; }
        public List<Player> WinningPlayers { get; set; }

        public decimal BigBlindAmount { get; set; }
        public decimal SmallBlindAmount { get; set; }

        public Deck GameDeck { get; set; }
        public Card[] BoardCards { get; set; }
        public int CurrBoardIdx { get; set; }

        public decimal Pot { get; set; }
        public List<decimal> SidePots { get; set; }
        public List<List<Player>> SidePotMembers { get; set; }

        public byte DealState { get; set; }

        public Game(List<Player> _playersAtStart, decimal _bigBlindAmount)
        {
            Pot = 0.0m;
            SidePots = new List<decimal>();
            BigBlindAmount = _bigBlindAmount;
            SmallBlindAmount = BigBlindAmount / 2;
            PlayersAtTable = _playersAtStart;
            PlayersInHand = _playersAtStart;
            WinningPlayers = new List<Player>();
            DealState = 0;
            GameDeck = new Deck();
            CurrBoardIdx = 0;
            BoardCards = new Card[5];
        }

        //PICKUP
        //creates winnerList
        public void ShowdownSinglePot()
        {

        }

        public void Deal()
        {
            List<Player> playersToDeal = this.PlayersInHand;
            int numPlayers = playersToDeal.Count();
            int currPlayer;

            switch (DealState)
            {
                // new deal
                case 0:
                    int currHole = 0;
                    for (int i = 0; i < 2 * numPlayers; i++)
                    {
                        currPlayer = i % numPlayers;
                        PlayersInHand[currPlayer].HoleCards[currHole] = this.GameDeck.DealCard();

                        if (i == numPlayers - 1)
                            currHole++;
                    }
                    break;
                //flop
                case 1:
                    this.GameDeck.BurnCard();
                    for (; this.CurrBoardIdx < 3; this.CurrBoardIdx++)
                    {
                        this.BoardCards[CurrBoardIdx] = this.GameDeck.DealCard();
                    }
                    break;
                //turn & river
                default:
                    this.GameDeck.BurnCard();
                    this.BoardCards[CurrBoardIdx] = this.GameDeck.DealCard();
                    this.CurrBoardIdx++;
                    break;
            }
            this.DealState++;
        }

        //TODO: side pots need to be dealt with
        public void DistributeSinglePot()
        {
            int numWinners = WinningPlayers.Count;

            foreach (var p in WinningPlayers)
                p.IncreaseStack(this.Pot / numWinners);

            this.Pot = 0;
        }

        

        
    }
}
