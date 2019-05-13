using System;
using System.Collections.Generic;

namespace BlackJack
{
    public enum BoxState
    {
        Waiting,
        Deciding,
        BlackJack,
        Standing,
        TooMany,
        Double,
        EvenMoney,
        Surrender
    }

    public class Box
    {
        public string BoxName { get; set; }
        public int AvailableSplitsAmount { get; private set; } = startingAvailableSplitAmount;
        public BoxState State { get; protected set; } = BoxState.Waiting;

        // Will be set true if the box should be deleted after the hand is played e.g. after a split
        public bool IsTemporary { get; private set; }

        const int startingAvailableSplitAmount = 2;

        protected Hand hand;
        int betSize;
        bool isActive;
        bool finishedHitting;

        //******************************************************************************************

        public Box(string boxName, int betSize, Hand hand = null, bool isTemporary = false)
        {
            BoxName = boxName;
            BetSize = betSize;
            IsTemporary = isTemporary;

            this.hand = (ReferenceEquals(hand, null)) ? new Hand() : new Hand(hand);
        }

        public Box(string boxName, int betSize, Card[] cards, bool isTemporary = false)
        {
            BoxName = boxName;
            BetSize = betSize;
            IsTemporary = isTemporary;
            hand = new Hand(cards);
        }

        public Box(string boxName, int betSize, List<Card> cards, bool isTemporary = false)
        {
            BoxName = boxName;
            BetSize = betSize;
            IsTemporary = isTemporary;
            hand = new Hand(cards);
        }

        public Box(Box box)
            : this(box.BoxName, box.BetSize, box.hand)
        { }

        //******************************************************************************************

        public int BetSize
        {
            get { return betSize; }
            set
            {
                if (value > 0)
                {
                    betSize = value;
                }
            }
        }

        public int Value => hand.Value;

        public int CardsAmount => hand.CardsInHandAmount;

        public string ValueAsString
        {
            get
            {
                if (IsBlackJack)
                {
                    return "21, BlackJack!";

                }
                if (hand.IsDoubleValue)
                {
                    return Convert.ToString(Value - 10) + "/" + Convert.ToString(Value);
                }

                return Convert.ToString(Value);
            }
        }

        public bool IsDeciding
        {
            get { return isActive; }
            set
            {
                if (value == true)
                {
                    State = BoxState.Deciding;
                }

                isActive = value;
            }
        }

        public bool IsFinishedHitting
        {
            get { return finishedHitting; }
            private set
            {
                if (IsDeciding)
                {
                    IsDeciding = false;
                }

                finishedHitting = value;
            }
        }

        public bool CanDouble => hand.CanDouble;
        public bool CanSplit => AvailableSplitsAmount > 0 && hand.CanSplit;
        public bool CanTakeEvenMoney => IsBlackJack;
        public bool CanSurrender => hand.CanSurrender && (startingAvailableSplitAmount == AvailableSplitsAmount) && !IsTemporary;
        // It can only be BlackJack if the hand was not created during a split
        public bool IsBlackJack => (!IsTemporary && AvailableSplitsAmount == 2) ? hand.IsBlackJack : false;

        //******************************************************************************************

        public override string ToString()
        {
            string firstLine = BoxName + " " + Convert.ToString(BetSize) + "zl " + State;
            if (State == BoxState.Deciding)
            {
                firstLine += " <<<<<";
            }
            firstLine += "\n";

            string secondLine = hand + "\n";

            string thirdLine = "Value: ";
            if (IsFinishedHitting && !IsBlackJack)
            {
                thirdLine += Value;
            }
            else
            {
                thirdLine += ValueAsString;
            }

            return firstLine + secondLine + thirdLine;
        }

        //******************************************************************************************

        public void AddCard(Card card)
        {
            hand += card;

            if (Value == 21)
            {
                if (IsBlackJack)
                {
                    State = BoxState.BlackJack;
                }
                else
                {
                    State = BoxState.Standing;
                }
                IsFinishedHitting = true;
            }
            else if (Value > 21)
            {
                IsFinishedHitting = true;
                State = BoxState.TooMany;
            }
        }

        public void Stand()
        {
            IsFinishedHitting = true;
            State = BoxState.Standing;
        }

        public void Double(Card card)
        {
            if (CanDouble)
            {
                State = BoxState.Double;
                IsFinishedHitting = true;
                BetSize *= 2;
                AddCard(card);
            }
        }

        public Box Split()
        {
            if (CanSplit)
            {
                Hand newHand = hand.Split();

                --AvailableSplitsAmount;

                return new Box(BoxName, BetSize, newHand, true);
            }

            return null;
        }

        public void TakeEvenMoney()
        {
            if (CanTakeEvenMoney)
            {
                IsFinishedHitting = true;
                State = BoxState.EvenMoney;
            }
        }

        public int Surrender()
        {
            if (CanSurrender)
            {
                IsFinishedHitting = true;
                State = BoxState.Surrender;

                return BetSize / 2;
            }

            return 0;
        }

        //******************************************************************************************

        public static Box operator +(Box box, Card card)
        {
            Box toReturn = new Box(box);
            toReturn.AddCard(card);

            return toReturn;
        }
    }
}