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
        public byte[] GetStrength(HandForEval hnd)
        {
            byte isFlush = this.IsFlush(hnd);
            byte isStraight = this.IsStraight(hnd);

            if (isFlush > 0 && isStraight > 0)
            {
                var strFlushStrength = this.GetStraightFlush(hnd, isFlush);
                if (strFlushStrength[0] != 0)
                    return strFlushStrength;
            }

            byte[] matchStrength = this.GetMatchStrength(hnd);

            if (matchStrength[0] > 6) return matchStrength;

            if (isFlush > 0) return this.GetFlushStrength(hnd, isFlush);
            if (isStraight > 0) return new byte[] { 5, isStraight };

            return matchStrength;
        }

        public byte IsFlush(HandForEval hnd)
        {
            byte numClub, numDia, numHrt, numSpd;
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
            if (numClub >= 5) return 1;
            else if (numSpd >= 5) return 2;
            else if (numHrt >= 5) return 3;
            else if (numDia >= 5) return 4;
            else return 0;
        }

        public byte IsStraight(HandForEval hnd)
        {
            bool inStraight = false;
            byte straightHigh = 0;
            byte strCnt = 1;
            for (byte i = 0; i < 6; i++)
            {
                if (!inStraight && hnd.SevCards[i].FaceValue == hnd.SevCards[i + 1].FaceValue + 1)
                {
                    strCnt++;
                    straightHigh = hnd.SevCards[i].FaceValue;
                    inStraight = true;
                } else if (inStraight && hnd.SevCards[i].FaceValue == hnd.SevCards[i + 1].FaceValue + 1)
                    strCnt++;
                else if (inStraight && hnd.SevCards[i].FaceValue == hnd.SevCards[i + 1].FaceValue)
                    ;
                else
                {
                    strCnt = 1;
                    inStraight = false;
                    straightHigh = 0;
                }

                //handle early escapes, ace to five straight and normal straight return
                if (i >= 3 && strCnt < 2) return 0;
                else if (strCnt == 4 && straightHigh == 5)
                {
                    for (byte j = 0; j < 6; j++)
                        if (hnd.SevCards[j].FaceValue == 14) return straightHigh;
                } else if (strCnt == 5) return straightHigh;
            }
            return 0;
        }

        public byte[] GetStraightFlush(HandForEval hnd, byte suit)
        {
            byte[] retStrength = new byte[2];
            byte straightHigh = 0;
            bool inStraight = false;

            byte strCnt = 1;
            for (byte i = 0; i < 6; i++)
            {
                if (!inStraight
                    && hnd.SevCards[i].FaceValue == hnd.SevCards[i + 1].FaceValue + 1
                    && hnd.SevCards[i].SuitValue == suit
                    && hnd.SevCards[i + 1].SuitValue == suit)
                {
                    strCnt++;
                    inStraight = true;
                    straightHigh = hnd.SevCards[i].FaceValue;
                } else if (inStraight
                        && hnd.SevCards[i].FaceValue == hnd.SevCards[i + 1].FaceValue + 1
                        && hnd.SevCards[i].SuitValue == suit
                        && hnd.SevCards[i + 1].SuitValue == suit)
                {
                    strCnt++;
                } else if (inStraight && hnd.SevCards[i].FaceValue == hnd.SevCards[i + 1].FaceValue)
                    ;
                else
                {
                    strCnt = 1;
                    inStraight = false;
                    straightHigh = 0;
                }

                if (strCnt == 4 && straightHigh == 5)
                {
                    for (byte j = 0; j<6;j++)
                        if (hnd.SevCards[j].FaceValue==5 && hnd.SevCards[j].SuitValue == suit)
                        {
                            retStrength[0] = 9;
                            retStrength[1] = straightHigh;
                        }
                } else if (strCnt == 5)
                {
                    retStrength[0] = 9;
                    retStrength[1] = straightHigh;
                }
            }
            return retStrength;
        }

        public byte[] GetFlushStrength(HandForEval hnd, byte suit)
        {
            byte[] retStrength = new byte[6];
            retStrength[0] = 6;

            byte retIdx = 1;
            for (byte i = 0; i < hnd.SevCards.Length; i++)
            {
                if (hnd.SevCards[i].SuitValue == suit)
                {
                    retStrength[retIdx] = hnd.SevCards[i].FaceValue;
                    retIdx++;
                }
                if (retIdx == 6)
                {
                    break;
                }
            }
            return retStrength;
        }

        public byte[] GetMatchStrength(HandForEval hnd)
        {
            //val of high pair, second best pair, set, quad
            byte[] matches = new byte[4];

            //in my model of hand strength match-type hands have a max of 6 vals
            byte[] retStrength = new byte[6];

            byte grpSize = 1;
            bool inGroup = false;

            //populate all useful matches into the match array
            for (byte i = 1; i < 7; i++)
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
                            else if (matches[0] > 0 && matches[1] > 0)
                                break;
                            else matches[0] = hnd.SevCards[i - 1].FaceValue;
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
                            //handle 2nd pair
                            if (matches[0] > 0 && matches[1] == 0)
                                matches[1] = hnd.SevCards[i - 1].FaceValue;
                            //handle 3 pairs
                            else if (matches[0] > 0 && matches[1] > 0)
                                break;
                            else matches[0] = hnd.SevCards[i - 1].FaceValue;
                            break;
                    }
                }
            }

            //create the hand strength from the matches found
            //quads
            if (matches[3] > 0)
            {
                retStrength[0] = 8;
                retStrength[1] = matches[3];

                //fetchKicker
                for (byte i = 0; i < hnd.SevCards.Length; i++)
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
                retStrength[0] = 7;
                retStrength[1] = matches[2];
                retStrength[2] = matches[0];
            }
            //trips
            else if (matches[2] > 0)
            {
                retStrength[0] = 4;
                retStrength[1] = matches[2];

                //fetch 2 Kickers
                byte idx = 2;
                for (byte i = 0; i < hnd.SevCards.Length; i++)
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
                retStrength[0] = 3;
                retStrength[1] = matches[0];
                retStrength[2] = matches[1];

                //fetchKicker
                for (byte i = 0; i < hnd.SevCards.Length; i++)
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
                retStrength[0] = 2;
                retStrength[1] = matches[0];

                //fetch 3 Kickers
                //TODO: confirm that 3 kickers are relevant to showdown by rules
                byte idx = 2;
                for (byte i = 0; i < hnd.SevCards.Length; i++)
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

        private byte[] GetHighCardStrength(HandForEval hnd)
        {
            //TODO: confirm that all 5 kickers are compared in the rules
            byte[] retStrength = new byte[6];
            retStrength[0] = 1;

            for (byte i = 0; i < 5; i++)
                retStrength[i + 1] = hnd.SevCards[i].FaceValue;

            return retStrength;
        }
    }
}
