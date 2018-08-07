using NLHEEngine.DTO;
using NLHEEngine.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLHEEngine.Models
{
    public class BettingRound
    {
        bool IsPreFlop { get; set; }
        public byte PlayerToAct { get; set; }
        public decimal TopBet { get; set; }

        List<Player> BettingOrder { get; set; }
        List<Player> FoldedPlayers { get; set; }

        ActionQuery CurrentQuery { get; set; }
        List<PlayerAction> ActionsToExecute { get; set; }


        //PICKUP
        public void ExecuteRound()
        {
            while (BettingOrder[BettingOrder.Count - 1].DealOrder != PlayerToAct)
            {

            }
        }

        //PICKUP
        public void ExecuteNextQuery()
        {

        }

        //TODO: Test
        public void AdjustBettingOrderOnNewBet(byte bettor)
        {
            this.BettingOrder.Rotate(bettor + 1);
            PlayerToAct = bettor;
            PlayerToAct++;
        }

        
    }
}
