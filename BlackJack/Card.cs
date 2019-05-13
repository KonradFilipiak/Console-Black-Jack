using System;

namespace BlackJack
{
    public enum CardSymbol
    {
        Ace = 1, Two, Three, Four, Five, Six, Seven, Eight, Nine, Teen, Jack, Queen, King
    }

    public enum CardSuit
    {
        Hearts, Clubs, Diamonds, Spades
    }

    //******************************************************************************************

    public class Card
    {
        public CardSymbol Symbol { get; private set; }
        public CardSuit Suit { get; private set; }
        public int Value { get; private set; }

        string stringRepresentation;

        //******************************************************************************************

        public Card(CardSymbol symbol, CardSuit suit)
        {
            Symbol = symbol;
            Suit = suit;

            SetCardValue();
            SetCardStringRepresentation();
        }

        public Card(Card card)
            : this(card.Symbol, card.Suit)
        { }

        //******************************************************************************************

        public override string ToString() => stringRepresentation;

        //******************************************************************************************

        public static bool operator ==(Card card1, Card card2)
        {
            // Check if one of the cards is null
            if (ReferenceEquals(null, card2))
            {
                return ReferenceEquals(card1, null);
            }
            if (ReferenceEquals(card1, null))
            {
                return false;
            }
            // If both cards are not null, then compare them
            return (card1.Suit == card2.Suit && card1.Symbol == card2.Symbol);
        }

        public static bool operator !=(Card card1, Card card2)
        {
            // Check if one of the cards is null
            if (ReferenceEquals(null, card2))
            {
                return !(ReferenceEquals(card1, null));
            }
            if (object.ReferenceEquals(card1, null))
            {
                return true;
            }
            // If both cards are not null, then compare them
            return !(card1.Suit == card2.Suit && card1.Symbol == card2.Symbol);
        }

        //******************************************************************************************

        void SetCardValue()
        {
            // Value in BlackJack depends on card's symbol
            Value = ((int)Symbol > 10) ? 10 : (int)Symbol;
        }

        // Cards in string are represented by two letters,
        // one for card symbol and one for card suit
        // e.g Ac, Ts, 5h
        void SetCardStringRepresentation()
        {
            string symbol = SetSymbolStringRepresentation();
            Char suit = SetSuitStringRepresentation();

            stringRepresentation = symbol + suit;
        }

        string SetSymbolStringRepresentation()
        {
            string symbol = "";
            switch (Symbol)
            {
                case CardSymbol.Ace:
                    symbol = "A";
                    break;
                case CardSymbol.Teen:
                    symbol = "T";
                    break;
                case CardSymbol.Jack:
                    symbol = "J";
                    break;
                case CardSymbol.Queen:
                    symbol = "Q";
                    break;
                case CardSymbol.King:
                    symbol = "K";
                    break;
                default:
                    symbol = Convert.ToString((int)Symbol);
                    break;
            }

            return symbol;
        }

        Char SetSuitStringRepresentation()
        {
            Char suit = ' ';
            switch (Suit)
            {
                case CardSuit.Hearts:
                    suit = 'h';
                    break;
                case CardSuit.Clubs:
                    suit = 'c';
                    break;
                case CardSuit.Diamonds:
                    suit = 'd';
                    break;
                case CardSuit.Spades:
                    suit = 's';
                    break;
            }

            return suit;
        }
    }
}
