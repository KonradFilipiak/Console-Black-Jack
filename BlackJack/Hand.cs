using System.Collections.Generic;

namespace BlackJack
{
    public class Hand
    {
        public int CardsInHandAmount { get; private set; } = 0;

        // It is true if the hand's value is not decided yet due to an ace in it
        // For example hand: Ac5s, Value: 6/16
        public bool IsDoubleValue { get; private set; }

        int value = 0;
        internal List<Card> cards = new List<Card>();

        //******************************************************************************************

        public Hand(Card[] cards = null)
        {
            if (cards != null)
            {
                foreach (Card card in cards)
                {
                    AddCard(card);
                }
            }
        }

        public Hand(List<Card> cards)
        {
            foreach (Card card in cards)
            {
                AddCard(card);
            }
        }

        public Hand(Hand hand)
            : this(hand.cards)
        { }

        //******************************************************************************************

        // There is no setter due to specific of the getter
        // Value should be change by changing the this.value variable directly
        public int Value
        {
            get
            {
                int toReturn = value;
                IsDoubleValue = false;

                if (HasAce())
                {
                    if (value <= 11)
                    {
                        toReturn += 10;
                        IsDoubleValue = true;
                    }
                }

                return toReturn;
            }
        }

        public bool CanDouble => CardsInHandAmount == 2 && Value != 21;
        public bool CanSplit => CardsInHandAmount == 2 && cards[0].Value == cards[1].Value;
        public bool CanSurrender => CardsInHandAmount == 2 && Value != 21;
        public bool IsBlackJack => CardsInHandAmount == 2 && Value == 21;

        //******************************************************************************************

        public override string ToString()
        {
            string toReturn = "";

            foreach (Card card in cards)
            {
                toReturn += card.ToString();
            }

            return toReturn;
        }

        //******************************************************************************************

        public void AddCard(Card card)
        {
            cards.Add(card);
            value += card.Value;
            ++CardsInHandAmount;
        }

        public void Clear()
        {
            cards.Clear();
            value = 0;
            CardsInHandAmount = 0;
        }

        public Hand Split()
        {
            if (CanSplit)
            {
                Hand toReturn = new Hand();

                toReturn += cards[1];
                cards.RemoveAt(1);
                --CardsInHandAmount;

                return toReturn;
            }

            return null;
        }

        //******************************************************************************************

        public static Hand operator +(Hand hand, Card card)
        {
            Hand toReturn = new Hand(hand);
            toReturn.AddCard(card);

            return toReturn;
        }

        public static Hand operator +(Hand hand1, Hand hand2)
        {
            Hand toReturn = new Hand(hand1);

            foreach (Card card in hand2.cards)
            {
                toReturn.AddCard(card);
            }

            return toReturn;
        }

        //******************************************************************************************

        bool HasAce()
        {
            foreach (Card card in cards)
            {
                if (card.Symbol == CardSymbol.Ace)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
