using System;
using System.Diagnostics;

namespace BlackJack
{
    public class CardsShoe
    {
        Card[] cards;

        int cardsAtStartAmount;
        int decksAmount;

        //******************************************************************************************

        public CardsShoe(int decksAmount = 6)
        {
            Debug.Assert(decksAmount > 0, "Number of decks is <= 0!");

            this.decksAmount = decksAmount;

            CreateCards();
            ShuffleCards();
        }

        //******************************************************************************************

        public int CardsInShoe => cards.Length;

        // Returns true if there is less than 20% of cards left in the shoe (20% is standard in BlackJack games played in casinos)
        // It could be less but we want to avoid situation where there is no cards left when a hand is being played
        // It could be more but it is generally more fun to play if you can follow the cards which are gone (counting cards)
        public bool ShouldBeReshuffled => (cards.Length / (float)cardsAtStartAmount) < 0.2f;

        //******************************************************************************************

        public Card PopCard()
        {
            Card[] newShoe;
            Card toReturn;
            int cardsInShoeAmount = cards.Length;

            if (cardsInShoeAmount <= 0)
            {
                return null;
            }

            toReturn = cards[cardsInShoeAmount - 1];
            newShoe = new Card[cardsInShoeAmount - 1];

            for (int i = 0; i < cardsInShoeAmount - 1; ++i)
            {
                newShoe[i] = cards[i];
            }

            cards = newShoe;

            return toReturn;
        }

        //******************************************************************************************

        void CreateCards()
        {
            int cardSymbolsAmount = Enum.GetNames(typeof(CardSymbol)).Length;
            int cardSuitsAmount = Enum.GetNames(typeof(CardSuit)).Length;
            int cardsInDeckAmount = cardSymbolsAmount * cardSuitsAmount;

            cardsAtStartAmount = decksAmount * cardsInDeckAmount;

            cards = new Card[cardsAtStartAmount];

            for (int deck = 0; deck < decksAmount; ++deck)
            {
                for (int suit = 0; suit < cardSuitsAmount; ++suit)
                {
                    for (int symbol = 1; symbol <= cardSymbolsAmount; ++symbol)
                    {
                        cards[(deck * cardsInDeckAmount) + (suit * cardSymbolsAmount) + (symbol - 1)] = new Card((CardSymbol)symbol, (CardSuit)suit);
                    }
                }
            }
        }

        void ShuffleCards()
        {
            Random random = new Random();
            int cardsInShoe = cards.Length;

            for (int i = 0; i < cardsInShoe; ++i)
            {
                int randomPosition = random.Next(0, cardsInShoe);

                Card tmp = cards[i];
                cards[i] = cards[randomPosition];
                cards[randomPosition] = tmp;
            }
        }
    }
}
