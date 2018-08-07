using NLHEEngine.Models;

namespace NLHEEngine.Subroutines
{
    public class HandEvaluator
    {
        private const byte HIGHCARD = 1;
        private const byte ONEPAIR = 2;
        private const byte TWOPAIR = 3;
        private const byte SET = 4;
        private const byte STRAIGHT = 5;
        private const byte FLUSH = 6;
        private const byte FULLHOUSE = 7;
        private const byte QUADS = 8;
        private const byte STRAIGHTFLUSH = 9;

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

            if (matchStrength[0] > FLUSH) return matchStrength;

            if (isFlush > 0) return this.GetFlushStrength(hnd, isFlush);
            if (isStraight > 0) return new byte[] { STRAIGHT, isStraight };

            return matchStrength;
        }

        private byte IsFlush(HandForEval hnd)
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

        private byte IsStraight(HandForEval hnd)
        {
            Card[] cards = hnd.SevCards;
            bool inStraight = false;
            byte straightHigh = 0;
            byte straightCount = 1;
            for (byte i = 0; i < 6; i++)
            {
                if (!inStraight && cards[i].FaceValue == cards[i + 1].FaceValue + 1)
                {
                    straightCount++;
                    straightHigh = cards[i].FaceValue;
                    inStraight = true;
                } else if (inStraight && cards[i].FaceValue == cards[i + 1].FaceValue + 1)
                    straightCount++;
                else if (inStraight && cards[i].FaceValue == cards[i + 1].FaceValue)
                    ;
                else
                {
                    straightCount = 1;
                    inStraight = false;
                    straightHigh = 0;
                }

                //handle looking at 4 cards and no straight yet, ace to five straight and normal straight return
                if (i >= 3 && straightCount < 2) return 0;
                else if (straightCount == 4 && straightHigh == 5)
                {
                    for (byte j = 0; j < 6; j++)
                        if (cards[j].FaceValue == 14) return straightHigh;
                } else if (straightCount == 5) return straightHigh;
            }
            return 0;
        }

        private byte[] GetStraightFlush(HandForEval hnd, byte suit)
        {
            Card[] cards = hnd.SevCards;
            byte[] retStrength = new byte[2];
            byte straightHigh = 0;
            bool inStraight = false;

            byte straightCount = 1;
            for (byte i = 0; i < 6; i++)
            {
                if (!inStraight
                    && cards[i].FaceValue == cards[i + 1].FaceValue + 1
                    && cards[i].SuitValue == suit
                    && cards[i + 1].SuitValue == suit)
                {
                    straightCount++;
                    inStraight = true;
                    straightHigh = cards[i].FaceValue;
                } else if (inStraight
                        && cards[i].FaceValue == cards[i + 1].FaceValue + 1
                        && cards[i].SuitValue == suit
                        && cards[i + 1].SuitValue == suit)
                {
                    straightCount++;
                } else if (inStraight && cards[i].FaceValue == cards[i + 1].FaceValue)
                    ;
                else
                {
                    straightCount = 1;
                    inStraight = false;
                    straightHigh = 0;
                }

                if (straightCount == 4 && straightHigh == 5)
                {
                    for (byte j = 0; j<6;j++)
                        if (cards[j].FaceValue==14 && cards[j].SuitValue == suit)
                        {
                            retStrength[0] = STRAIGHTFLUSH;
                            retStrength[1] = straightHigh;
                        }
                } else if (straightCount == 5)
                {
                    retStrength[0] = STRAIGHTFLUSH;
                    retStrength[1] = straightHigh;
                }
            }
            return retStrength;
        }

        private byte[] GetFlushStrength(HandForEval hnd, byte suit)
        {
            Card[] cards = hnd.SevCards;
            byte[] retStrength = new byte[6];
            retStrength[0] = FLUSH;

            byte retIdx = 1;
            for (byte i = 0; i < cards.Length; i++)
            {
                if (cards[i].SuitValue == suit)
                {
                    retStrength[retIdx] = cards[i].FaceValue;
                    retIdx++;
                }
                if (retIdx == 6)
                {
                    break;
                }
            }
            return retStrength;
        }

        private byte[] GetMatchStrength(HandForEval hnd)
        {
            Card[] cards = hnd.SevCards;
            //val of high pair, second best pair, set, quad
            byte[] matches = new byte[4];

            //in my model of hand strength match-type hands have a max of 6 vals
            byte[] retStrength = new byte[6];

            byte grpSize = 1;
            bool inGroup = false;

            //populate all useful matches into the match array
            for (byte i = 1; i < 7; i++)
            {
                if (!inGroup && cards[i - 1].FaceValue == cards[i].FaceValue)
                {
                    inGroup = true;
                    grpSize++;
                } else if (inGroup && cards[i - 1].FaceValue == cards[i].FaceValue)
                    grpSize++;
                else if (inGroup && cards[i - 1].FaceValue != cards[i].FaceValue)
                {
                    switch (grpSize)
                    {
                        case 4:
                            matches[3] = cards[i - 1].FaceValue;
                            break;
                        case 3:
                            //handle a 2nd group of trips
                            if (matches[2] > 0 && matches[0] == 0)
                            {
                                matches[0] = cards[i - 1].FaceValue;
                                break;
                            } else
                            {
                                matches[2] = cards[i - 1].FaceValue;
                                break;
                            }
                        case 2:
                            if (matches[0] > 0 && matches[1] == 0)
                                matches[1] = cards[i - 1].FaceValue;
                            else if (matches[0] > 0 && matches[1] > 0)
                                break;
                            else matches[0] = cards[i - 1].FaceValue;
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
                            matches[3] = cards[i - 1].FaceValue;
                            break;
                        case 3:
                            //handle a 2nd group of trips
                            if (matches[2] > 0 && matches[0] == 0)
                            {
                                matches[0] = cards[i - 1].FaceValue;
                                break;
                            } else
                            {
                                matches[2] = cards[i - 1].FaceValue;
                                break;
                            }
                        case 2:
                            //handle 2nd pair
                            if (matches[0] > 0 && matches[1] == 0)
                                matches[1] = cards[i - 1].FaceValue;
                            //handle 3 pairs
                            else if (matches[0] > 0 && matches[1] > 0)
                                break;
                            else matches[0] = cards[i - 1].FaceValue;
                            break;
                    }
                }
            }

            //assign the hand strength from the matches found
            //quads
            if (matches[3] > 0)
            {
                retStrength[0] = QUADS;
                retStrength[1] = matches[3];

                //fetchKicker
                for (byte i = 0; i < cards.Length; i++)
                {
                    if (cards[i].FaceValue != matches[3])
                    {
                        retStrength[2] = cards[i].FaceValue;
                        break;
                    }
                }
            }
            //boat
            else if (matches[2] > 0 && matches[0] > 0)
            {
                retStrength[0] = FULLHOUSE;
                retStrength[1] = matches[2];
                retStrength[2] = matches[0];
            }
            //trips
            else if (matches[2] > 0)
            {
                retStrength[0] = SET;
                retStrength[1] = matches[2];

                //fetch 2 Kickers
                byte idx = 2;
                for (byte i = 0; i < cards.Length; i++)
                {
                    if (cards[i].FaceValue != matches[2])
                    {
                        retStrength[idx] = cards[i].FaceValue;
                        idx++;
                    }
                    if (idx == 4) break;
                }
            }
            //2 pair
            else if (matches[1] > 0)
            {
                retStrength[0] = TWOPAIR;
                retStrength[1] = matches[0];
                retStrength[2] = matches[1];

                //fetchKicker
                for (byte i = 0; i < cards.Length; i++)
                {
                    if (cards[i].FaceValue != matches[1] &&
                        cards[i].FaceValue != matches[0])
                    {
                        retStrength[3] = cards[i].FaceValue;
                        break;
                    }
                }
            }
            //single pair
            else if (matches[0] > 0)
            {
                retStrength[0] = ONEPAIR;
                retStrength[1] = matches[0];

                //fetch 3 Kickers
                byte idx = 2;
                for (byte i = 0; i < cards.Length; i++)
                {
                    if (cards[i].FaceValue != matches[0])
                    {
                        retStrength[idx] = cards[i].FaceValue;
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
            byte[] retStrength = new byte[6];
            retStrength[0] = HIGHCARD;

            for (byte i = 0; i < 5; i++)
                retStrength[i + 1] = hnd.SevCards[i].FaceValue;

            return retStrength;
        }
    }
}
