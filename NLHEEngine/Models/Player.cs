using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLHEEngine.Models
{
    public class Player
    {
        public string Handle { get; set; }
        public decimal StackSize { get; set; }

        public Card[] HoleCards = new Card[2];

        //TODO determine if deal order is necessary
        public byte DealOrder { get; set; }

        public bool IsDealer { get; set; }
        public bool IsSmallBlind { get; set; }
        public bool IsBigBlind { get; set; }
        public bool IsAllIn { get; set; }

        public Player(string _handle, decimal _stacksize, byte _dealOrder)
        {
            Handle = _handle;
            IsDealer = false;
            IsSmallBlind = false;
            IsBigBlind = false;
            IsAllIn = false;
            DealOrder = _dealOrder;
        }

        //TODO: decide how to handle cases where the stack amount is smaller than the blind
        public decimal GetBlind (decimal amount)
        {
            if (amount < StackSize)
            {
                this.StackSize -= amount;
                return amount;
            } else return 0.0m;
        }

        public decimal Bet(decimal amount)
        {
            if (amount < StackSize)
            {
                this.StackSize -= amount;
                return amount;
            } else if (amount == StackSize)
            {
                this.IsAllIn = true;
                this.StackSize = 0;
                return amount;
            }
            else return 0.0m;
        }

        public bool IncreaseStack(decimal amount)
        {
            try
            {
                this.StackSize += amount;
                return true;
            } catch
            {
                return false;
            }
        }

        //TODO: confirm this is the best way to do this. 
        public void Fold()
        {
            HoleCards = new Card[2];
        }

        public void PrepForNextHand()
        {
            this.IsBigBlind = false;
            this.IsDealer = false;
            this.IsSmallBlind = false;
        }
    }
}
