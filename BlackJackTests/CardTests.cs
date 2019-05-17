using System;
using Xunit;

namespace BlackJackTests
{
    public class CardTests
    {

        public class ConstructorTests
        {
            [Fact]
            public void Card_Symbol_Should_Be_Assigned()
            {
                var expected = BlackJack.CardSymbol.Ace;

                var card = new BlackJack.Card(BlackJack.CardSymbol.Ace, BlackJack.CardSuit.Hearts);
                var actual = card.Symbol;

                Assert.Equal(expected, actual);
            }

            [Fact]
            public void Card_Suit_Should_Be_Assigned()
            {
                var expected = BlackJack.CardSuit.Hearts;

                var card = new BlackJack.Card(BlackJack.CardSymbol.Ace, BlackJack.CardSuit.Hearts);
                var actual = card.Suit;

                Assert.Equal(expected, actual);
            }

            [Theory]
            [InlineData(BlackJack.CardSymbol.Ace, 1)]
            [InlineData(BlackJack.CardSymbol.Two, 2)]
            [InlineData(BlackJack.CardSymbol.Nine, 9)]
            [InlineData(BlackJack.CardSymbol.Teen, 10)]
            [InlineData(BlackJack.CardSymbol.King, 10)]
            public void Card_Value_Should_Be_Assinged(BlackJack.CardSymbol symbol, int expected)
            {
                var card = new BlackJack.Card(symbol, BlackJack.CardSuit.Hearts);
                var actual = card.Value;

                Assert.Equal(expected, actual);
            }

            [Theory]
            [InlineData(BlackJack.CardSymbol.Ace, BlackJack.CardSuit.Hearts, "Ah")]
            [InlineData(BlackJack.CardSymbol.Two, BlackJack.CardSuit.Clubs, "2c")]
            [InlineData(BlackJack.CardSymbol.Nine, BlackJack.CardSuit.Diamonds, "9d")]
            [InlineData(BlackJack.CardSymbol.Teen, BlackJack.CardSuit.Spades, "Ts")]
            [InlineData(BlackJack.CardSymbol.King, BlackJack.CardSuit.Clubs, "Kc")]
            public void Card_String_Representation_Should_Be_Assigned(
                BlackJack.CardSymbol symbol, BlackJack.CardSuit suit, String expected)
            {
                var card = new BlackJack.Card(symbol, suit);

                var actual = card.ToString();

                Assert.Equal(expected, actual);
            }
        }
    }
}
