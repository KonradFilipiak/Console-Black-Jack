using System;
using System.Collections.Generic;
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
                var expectedSymbol = BlackJack.CardSymbol.Ace;

                var card = new BlackJack.Card(BlackJack.CardSymbol.Ace, BlackJack.CardSuit.Hearts);
                var actualSymbol = card.Symbol;

                Assert.Equal(expectedSymbol, actualSymbol);
            }

            [Fact]
            public void Card_Suit_Should_Be_Assigned()
            {
                var expectedSuit = BlackJack.CardSuit.Hearts;

                var card = new BlackJack.Card(BlackJack.CardSymbol.Ace, BlackJack.CardSuit.Hearts);
                var actualSuit = card.Suit;

                Assert.Equal(expectedSuit, actualSuit);
            }

            [Theory]
            [MemberData(nameof(CardsWithValue))]
            public void Card_Value_Should_Be_Assinged(BlackJack.CardSymbol symbol, int expectedValue)
            {
                var card = new BlackJack.Card(symbol, BlackJack.CardSuit.Hearts);
                var actualValue = card.Value;

                Assert.Equal(expectedValue, actualValue);
            }

            [Theory]
            [MemberData(nameof(CardsWithStringRepresentation))]
            public void Card_String_Representation_Should_Be_Assigned(
                BlackJack.CardSymbol symbol, BlackJack.CardSuit suit, String expectedString)
            {
                var card = new BlackJack.Card(symbol, suit);

                var actualString = card.ToString();

                Assert.Equal(expectedString, actualString);
            }

            public static IEnumerable<object[]> CardsWithValue =>
                new List<object[]>
                {
                    new object[] {BlackJack.CardSymbol.Ace, 1},
                    new object[] {BlackJack.CardSymbol.Two, 2},
                    new object[] {BlackJack.CardSymbol.Nine, 9},
                    new object[] {BlackJack.CardSymbol.Teen, 10},
                    new object[] {BlackJack.CardSymbol.King, 10}
                };

            public static IEnumerable<object[]> CardsWithStringRepresentation =>
                new List<object[]>
                {
                    new object[] {BlackJack.CardSymbol.Ace, BlackJack.CardSuit.Hearts, "Ah"},
                    new object[] {BlackJack.CardSymbol.Two, BlackJack.CardSuit.Clubs, "2c"},
                    new object[] {BlackJack.CardSymbol.Nine, BlackJack.CardSuit.Diamonds, "9d"},
                    new object[] {BlackJack.CardSymbol.Teen, BlackJack.CardSuit.Spades, "Ts"},
                    new object[] {BlackJack.CardSymbol.King, BlackJack.CardSuit.Clubs, "Kc"},
                };
        }
    }
}
