using NLHEEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLHEEngine.Subroutines
{
    public class HandEvaluator
    {
        //this is the grand pooh bah method
        //public int[] GetStrength(HandForEval hnd)
        //{

        //}


        public bool IsFlush(HandForEval hnd)
        {
            int numClub, numDia, numHrt, numSpd;
            numClub = numDia = numHrt = numSpd = 0;
            foreach (Card c in hnd.SevCards)
            {
                switch (c.SuitValue)
                {
                    case 1: numClub++; break;
                    case 2: numSpd++; break;
                    case 3: numHrt++; break;
                    case 4: numDia++; break;
                }
            }
            if (numClub >= 5 || numDia >= 5 || numHrt >= 5 || numSpd >= 5) return true;
            else return false;
        }

        public bool IsStraight(HandForEval hnd)
        {
            int strCnt = 1;
            for (int i = 0; i < hnd.SevCards.Length; i++)
            {
                if (hnd.SevCards[i].FaceValue == hnd.SevCards[i + 1].FaceValue + 1)
                {
                    strCnt++;
                } else
                {
                    strCnt = 1;
                }

                if (i >= 3 && strCnt < 2) return false;
                if (strCnt == 5) return true;
            }
            return false;
        }

        public int[] GetMatchStrength(HandForEval hnd)
        {
            //val of high pair, second best pair, set, quad
            int[] matches = new int[4];

            // TODO: make sure this size will work with the best high card hand. 
            //in my model of hand strength match-type hands have a max of 5 vals
            int[] retStrength = new int[6];

            int grpSize = 1;
            bool inGroup = false;

            //populate all useful matches into the match array
            for (int i = 1; i < 7; i++)
            {
                if (!inGroup && hnd.SevCards[i - 1].FaceValue == hnd.SevCards[i].FaceValue)
                {
                    inGroup = true;
                    grpSize++;
                } else if (inGroup && hnd.SevCards[i - 1].FaceValue == hnd.SevCards[i].FaceValue)
                    grpSize++;
                else if (inGroup && hnd.SevCards[i - 1].FaceValue != hnd.SevCards[i].FaceValue)
                {
                    switch (grpSize)
                    {
                        case 4:
                            matches[3] = hnd.SevCards[i-1].FaceValue;
                            break;
                        case 3:
                            //handle a 2nd group of trips
                            if (matches[2] > 0 && matches[0] == 0)
                            {
                                matches[0] = hnd.SevCards[i-1].FaceValue;
                                break;
                            } else
                            {
                                matches[2] = hnd.SevCards[i-1].FaceValue;
                                break;
                            }
                        case 2:
                            if (matches[0] > 0 && matches[1] == 0)
                                matches[1] = hnd.SevCards[i-1].FaceValue;
                            else matches[0] = hnd.SevCards[i-1].FaceValue;
                            break;
                    }
                    inGroup = false;
                    grpSize = 1;
                }

                //TODO: Figure out if this can be expressed better
                if (i == 6 && inGroup)
                {
                    switch (grpSize)
                    {
                        case 4:
                            matches[3] = hnd.SevCards[i - 1].FaceValue;
                            break;
                        case 3:
                            //handle a 2nd group of trips
                            if (matches[2] > 0 && matches[0] == 0)
                            {
                                matches[0] = hnd.SevCards[i - 1].FaceValue;
                                break;
                            } else
                            {
                                matches[2] = hnd.SevCards[i - 1].FaceValue;
                                break;
                            }
                        case 2:
                            if (matches[0] > 0 && matches[1] == 0)
                                matches[1] = hnd.SevCards[i - 1].FaceValue;
                            else matches[0] = hnd.SevCards[i - 1].FaceValue;
                            break;
                    }
                }
            }

            //create the hand strength from the matches we found
            //quads
            if (matches[3] > 0)
            {
                retStrength[0] = 2;
                retStrength[1] = matches[3];

                //fetchKicker
                for (int i = 0; i < hnd.SevCards.Length; i++)
                {
                    if (hnd.SevCards[i].FaceValue != matches[3])
                    {
                        retStrength[2] = hnd.SevCards[i].FaceValue;
                        break;
                    }
                }
            }
            //boat
            else if (matches[2] > 0 && matches[0] > 0)
            {
                retStrength[0] = 3;
                retStrength[1] = matches[2];
                retStrength[2] = matches[0];
                //yay, no kicker noise
            }
            //trips
            else if (matches[2] > 0)
            {
                retStrength[0] = 6;
                retStrength[1] = matches[2];

                //fetch 2 Kickers
                int idx = 2;
                for (int i = 0; i < hnd.SevCards.Length; i++)
                {
                    if (hnd.SevCards[i].FaceValue != matches[2])
                    {
                        retStrength[idx] = hnd.SevCards[i].FaceValue;
                        idx++;
                    }
                    if (idx == 4) break;
                }
            }
            //2 pair
            else if (matches[1] > 0)
            {
                retStrength[0] = 7;
                retStrength[1] = matches[0];
                retStrength[2] = matches[1];

                //fetchKicker
                for (int i = 0; i < hnd.SevCards.Length; i++)
                {
                    if (hnd.SevCards[i].FaceValue != matches[1] &&
                        hnd.SevCards[i].FaceValue != matches[0])
                    {
                        retStrength[3] = hnd.SevCards[i].FaceValue;
                        break;
                    }
                }
            }
            //single pair
            else if (matches[0] > 0)
            {
                retStrength[0] = 8;
                retStrength[1] = matches[0];

                //fetch 3 Kickers
                //TODO: confirm that 3 kickers are relevant to showdown by rules
                int idx = 2;
                for (int i = 0; i < hnd.SevCards.Length; i++)
                {
                    if (hnd.SevCards[i].FaceValue != matches[0])
                    {
                        retStrength[idx] = hnd.SevCards[i].FaceValue;
                        idx++;
                    }
                    if (idx == 5) break;
                }
            } else
            {
                retStrength = this.GetHighCardStrength(hnd);
            }
            return retStrength;
        }

        private int[] GetHighCardStrength(HandForEval hnd)
        {
            //TODO: confirm that all 5 kickers are compared
            int[] retStrength = new int[6];
            retStrength[0] = 9;

            for (int i = 0; i < 5; i++)
                retStrength[i + 1] = hnd.SevCards[i].FaceValue;

            return retStrength;
        }
    }
}
