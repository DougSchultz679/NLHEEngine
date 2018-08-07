using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLHEEngine.DTO
{
    public class PlayerAction
    {
        public int PlayerId { get; set; }
        public byte ActionType { get; set; }
        public byte DecisionType { get; set; }

        public decimal CashToPot { get; set; }

        public bool AllIn { get; set; }

        //PICKUP
    }
}
