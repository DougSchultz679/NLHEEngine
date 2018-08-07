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
        public byte NumSidePot { get; set; }
        public List<decimal> SidePots { get; set; }
        public List<List<Player>> SidePotMembers { get; set; }
        public List<List<Player>> SidePotWinners { get; set; }

        public List<BettingRound> BettingRounds { get; set; }

        public byte RoundState { get; set; }
        public byte DealState { get; set; }

        public Game(List<Player> _playersAtStart, decimal _bigBlindAmount)
        {
            Pot = 0.0m;
            NumSidePot = 0;
            SidePots = new List<decimal>();
            SidePotMembers = new List<List<Player>>();
            SidePotWinners = new List<List<Player>>();
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

        public void CreateSidePot(List<Player> playersForSidepot, decimal sidePotSize)
        {
            this.SidePotMembers.Add(playersForSidepot);
            this.SidePots.Add(sidePotSize);
            this.Pot = 0.0m;
            this.NumSidePot++;
        }

        //creates winnerList
        //TODO: can this be written more cleanly? 
        public void ShowdownSinglePot()
        {
            foreach (var p in this.PlayersInHand)
            {
                p.HandForShowdown = new HandForEval(this.BoardCards.Concat(p.HoleCards).ToArray());
            }

            int result, topPlayerIdx;
            topPlayerIdx = 0;
            bool checkTies = false;

            //find the player with the best hand
            for (int i = 1; i < PlayersInHand.Count; i++)
            {
                result = PlayersInHand[i].HandForShowdown.CompareTo(
                    PlayersInHand[topPlayerIdx].HandForShowdown);
                switch (result)
                {
                    case 1:
                        topPlayerIdx = i; break;
                    //TODO: Test these
                    case 0:
                        checkTies = true; break;
                    case -1:
                        break;
                }
            }
            this.WinningPlayers.Add(PlayersInHand[topPlayerIdx]);
            this.PlayersInHand.Remove(PlayersInHand[topPlayerIdx]);

            if (checkTies)
            {
                foreach (var p in PlayersInHand)
                    if (WinningPlayers[0].HandForShowdown.CompareTo(p.HandForShowdown) == 0)
                        WinningPlayers.Add(p);
            }

        }

        //TODO: test this. Note this does NOT showdown the main pot. Should it? 
        public void ShowdownMultiPot()
        {
            //Every player from the first side pot gets their hand strength evaluated
            foreach(var p in SidePotMembers[0])
            {
                p.HandForShowdown = new HandForEval(this.BoardCards.Concat(p.HoleCards).ToArray());
            }

            int result, topPlayerIdx;
            topPlayerIdx = 0;
            bool checkTies = false;
            List<Player> thisSidePotMembers = new List<Player>();
            List<Player> thisSidePotWinners = new List<Player>();

            //For each side pot we get a list of winners and populate it in order to the winnerList
            for (int i = 0; i < SidePots.Count; i++)
            {
                thisSidePotMembers = SidePotMembers[i];
                //find the player with the best hand
                for (int j = 1; i < thisSidePotMembers.Count; i++)
                {
                    result = thisSidePotMembers[j].HandForShowdown.CompareTo(
                        thisSidePotMembers[topPlayerIdx].HandForShowdown);
                    switch (result)
                    {
                        case 1:
                            topPlayerIdx = j; break;
                        //TODO: Test these
                        case 0:
                            checkTies = true; break;
                        case -1:
                            break;
                    }
                }

                thisSidePotWinners.Add(thisSidePotMembers[topPlayerIdx]);
                thisSidePotMembers.Remove(thisSidePotMembers[topPlayerIdx]);

                if (checkTies)
                {
                    foreach (var p in thisSidePotMembers)
                        if (thisSidePotWinners[0].HandForShowdown.CompareTo(p.HandForShowdown) == 0)
                            thisSidePotWinners.Add(p);
                }

                this.SidePotWinners.Add(thisSidePotWinners);

                //set to default for next side pot
                thisSidePotWinners = new List<Player>();
                topPlayerIdx = 0;
                checkTies = false;
            }
        }

        

        //TODO: this method needs to be manually called right now. Should it be part of showdown single pot?
        public void DistributeSinglePot()
        {
            int numWinners = WinningPlayers.Count;

            foreach (var p in WinningPlayers)
                p.IncreaseStack(this.Pot / numWinners);

            this.Pot = 0;
        }

        //TODO: PICKUP here. need to contend with whether 
        public void DistributeMultiPot()
        {

        }
    }
}
