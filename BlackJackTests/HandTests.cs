using Xunit;
using System.Collections.Generic;

namespace BlackJackTests
{
    public class HandTests
    {
        public class ConstructorTests
        {
        }

        public class ValueTests
        {
            [Theory]
            [MemberData(nameof(CardsWithNoAceList))]
            public void Value_Should_Be_A_Sum_Of_Card_Values_If_There_Is_No_Ace
                (int expectedValue, params BlackJack.Card[] cards)
            {
                var hand = new BlackJack.Hand(cards);

                var actualValue = hand.Value;

                Assert.Equal(expectedValue, actualValue);
            }

            [Theory]
            [MemberData(nameof(CardsWithAceWithValueOfEleven))]
            public void Ace_Should_Add_Eleven_To_Value_If_It_Is_Less_Or_Equal_21
                (int expectedValue, params BlackJack.Card[] cards)
            {
                var hand = new BlackJack.Hand(cards);

                var actualValue = hand.Value;

                Assert.Equal(expectedValue, actualValue);
            }

            [Theory]
            [MemberData(nameof(CardsWithAceWithValueOfOne))]
            public void Ace_Should_Add_One_To_Value_If_Adding_Eleven_Makes_It_More_Then_21
                (int expectedValue, params BlackJack.Card[] cards)
            {
                var hand = new BlackJack.Hand(cards);

                var actualValue = hand.Value;

                Assert.Equal(expectedValue, actualValue);
            }

            public static IEnumerable<object[]> CardsWithNoAceList =>
                new List<object[]>
            {
                new object[]
                {
                    "2",
                    new BlackJack.Card(BlackJack.CardSymbol.Two, BlackJack.CardSuit.Clubs)
                },
                new object[]
                {
                    "20",
                    new BlackJack.Card(BlackJack.CardSymbol.Teen, BlackJack.CardSuit.Clubs),
                    new BlackJack.Card(BlackJack.CardSymbol.King, BlackJack.CardSuit.Clubs)
                },
                new object[]
                {
                    "21",
                    new BlackJack.Card(BlackJack.CardSymbol.Two, BlackJack.CardSuit.Clubs),
                    new BlackJack.Card(BlackJack.CardSymbol.King, BlackJack.CardSuit.Clubs),
                    new BlackJack.Card(BlackJack.CardSymbol.Nine, BlackJack.CardSuit.Clubs)
                },
                new object[]
                {
                    "22",
                    new BlackJack.Card(BlackJack.CardSymbol.Two, BlackJack.CardSuit.Clubs),
                    new BlackJack.Card(BlackJack.CardSymbol.King, BlackJack.CardSuit.Clubs),
                    new BlackJack.Card(BlackJack.CardSymbol.Teen, BlackJack.CardSuit.Clubs)
                },

            };

            public static IEnumerable<object[]> CardsWithAceWithValueOfEleven =>
                new List<object[]>
                {
                    new object[]
                    {
                        "11",
                        new BlackJack.Card(BlackJack.CardSymbol.Ace, BlackJack.CardSuit.Clubs)
                    },
                    new object[]
                    {
                        "12",
                        new BlackJack.Card(BlackJack.CardSymbol.Ace, BlackJack.CardSuit.Clubs),
                        new BlackJack.Card(BlackJack.CardSymbol.Ace, BlackJack.CardSuit.Diamonds)
                    },
                    new object[]
                    {
                        "21",
                        new BlackJack.Card(BlackJack.CardSymbol.Ace, BlackJack.CardSuit.Clubs),
                        new BlackJack.Card(BlackJack.CardSymbol.King, BlackJack.CardSuit.Diamonds)
                    },
                    new object[]
                    {
                        "16",
                        new BlackJack.Card(BlackJack.CardSymbol.Ace, BlackJack.CardSuit.Clubs),
                        new BlackJack.Card(BlackJack.CardSymbol.Two, BlackJack.CardSuit.Diamonds),
                        new BlackJack.Card(BlackJack.CardSymbol.Three, BlackJack.CardSuit.Diamonds)
                    },
                };
            public static IEnumerable<object[]> CardsWithAceWithValueOfOne =>
                new List<object[]>
                {
                    new object[]    // 12 Aces
                    {
                        "12",
                        new BlackJack.Card(BlackJack.CardSymbol.Ace, BlackJack.CardSuit.Clubs),
                        new BlackJack.Card(BlackJack.CardSymbol.Ace, BlackJack.CardSuit.Diamonds),
                        new BlackJack.Card(BlackJack.CardSymbol.Ace, BlackJack.CardSuit.Hearts),
                        new BlackJack.Card(BlackJack.CardSymbol.Ace, BlackJack.CardSuit.Spades),
                        new BlackJack.Card(BlackJack.CardSymbol.Ace, BlackJack.CardSuit.Clubs),
                        new BlackJack.Card(BlackJack.CardSymbol.Ace, BlackJack.CardSuit.Diamonds),
                        new BlackJack.Card(BlackJack.CardSymbol.Ace, BlackJack.CardSuit.Hearts),
                        new BlackJack.Card(BlackJack.CardSymbol.Ace, BlackJack.CardSuit.Spades),
                        new BlackJack.Card(BlackJack.CardSymbol.Ace, BlackJack.CardSuit.Clubs),
                        new BlackJack.Card(BlackJack.CardSymbol.Ace, BlackJack.CardSuit.Diamonds),
                        new BlackJack.Card(BlackJack.CardSymbol.Ace, BlackJack.CardSuit.Hearts),
                        new BlackJack.Card(BlackJack.CardSymbol.Ace, BlackJack.CardSuit.Spades)
                    },
                    new object[]
                    {
                        "21",
                        new BlackJack.Card(BlackJack.CardSymbol.Ace, BlackJack.CardSuit.Clubs),
                        new BlackJack.Card(BlackJack.CardSymbol.Queen, BlackJack.CardSuit.Diamonds),
                        new BlackJack.Card(BlackJack.CardSymbol.King, BlackJack.CardSuit.Diamonds)
                    }
                };
        }
    }
}
