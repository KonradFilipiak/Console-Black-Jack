using Xunit;
using System.Collections.Generic;

namespace BlackJackTests
{
    public class HandTests
    {
        public class ValueTests
        {
            [Theory]
            [MemberData(nameof(CardsWithNoAce))]
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

            public static IEnumerable<object[]> CardsWithNoAce =>
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

        public class CanDoubleTests
        {
            [Fact]
            public void Can_Double_Is_True_If_There_Are_Two_Cards_And_Value_Is_Less_Then_21()
            {
                var cards = new List<BlackJack.Card>
                {
                    new BlackJack.Card(BlackJack.CardSymbol.King, BlackJack.CardSuit.Diamonds),
                    new BlackJack.Card(BlackJack.CardSymbol.Teen, BlackJack.CardSuit.Spades)
                };

                var hand = new BlackJack.Hand(cards);

                Assert.True(hand.CanDouble);
            }

            [Fact]
            public void Can_Double_Is_False_If_There_Is_More_Then_2_Cards()
            {
                var cards = new List<BlackJack.Card>
                {
                    new BlackJack.Card(BlackJack.CardSymbol.Two, BlackJack.CardSuit.Diamonds),
                    new BlackJack.Card(BlackJack.CardSymbol.Three, BlackJack.CardSuit.Spades),
                    new BlackJack.Card(BlackJack.CardSymbol.Four, BlackJack.CardSuit.Spades)
                };

                var hand = new BlackJack.Hand(cards);

                Assert.False(hand.CanDouble);
            }

            [Fact]
            public void Can_Double_Is_False_If_Value_Is_21()
            {
                var cards = new List<BlackJack.Card>
                {
                    new BlackJack.Card(BlackJack.CardSymbol.Teen, BlackJack.CardSuit.Diamonds),
                    new BlackJack.Card(BlackJack.CardSymbol.Ace, BlackJack.CardSuit.Spades)
                };

                var hand = new BlackJack.Hand(cards);

                Assert.False(hand.CanDouble);
            }
        }

        public class CanSplitTests
        {
            [Fact]
            public void Can_Split_Is_True_If_There_Are_Two_Cards_And_They_Have_The_Same_Value()
            {
                var cards = new List<BlackJack.Card>
                {
                    new BlackJack.Card(BlackJack.CardSymbol.King, BlackJack.CardSuit.Diamonds),
                    new BlackJack.Card(BlackJack.CardSymbol.Teen, BlackJack.CardSuit.Spades)
                };

                var hand = new BlackJack.Hand(cards);

                Assert.True(hand.CanSplit);
            }

            [Fact]
            public void Can_Split_Is_False_If_There_Is_More_Then_2_Cards()
            {
                var cards = new List<BlackJack.Card>
                {
                    new BlackJack.Card(BlackJack.CardSymbol.King, BlackJack.CardSuit.Diamonds),
                    new BlackJack.Card(BlackJack.CardSymbol.Teen, BlackJack.CardSuit.Spades),
                    new BlackJack.Card(BlackJack.CardSymbol.Teen, BlackJack.CardSuit.Spades)
                };

                var hand = new BlackJack.Hand(cards);

                Assert.False(hand.CanSplit);
            }

            [Fact]
            public void Can_Split_Is_False_If_Cards_Have_Different_Values()
            {
                var cards = new List<BlackJack.Card>
                {
                    new BlackJack.Card(BlackJack.CardSymbol.King, BlackJack.CardSuit.Diamonds),
                    new BlackJack.Card(BlackJack.CardSymbol.Two, BlackJack.CardSuit.Spades)
                };

                var hand = new BlackJack.Hand(cards);

                Assert.False(hand.CanSplit);
            }
        }

        public class CanSurrenderTests
        {
            [Fact]
            public void Can_Surrender_Is_True_If_There_Are_Two_Cards_And_Value_Is_Not_21()
            {
                var cards = new List<BlackJack.Card>
                {
                    new BlackJack.Card(BlackJack.CardSymbol.King, BlackJack.CardSuit.Diamonds),
                    new BlackJack.Card(BlackJack.CardSymbol.Teen, BlackJack.CardSuit.Spades)
                };

                var hand = new BlackJack.Hand(cards);

                Assert.True(hand.CanSurrender);
            }

            [Fact]
            public void Can_Surrender_Is_False_If_There_Is_More_Then_2_Cards()
            {
                var cards = new List<BlackJack.Card>
                {
                    new BlackJack.Card(BlackJack.CardSymbol.King, BlackJack.CardSuit.Diamonds),
                    new BlackJack.Card(BlackJack.CardSymbol.Two, BlackJack.CardSuit.Spades),
                    new BlackJack.Card(BlackJack.CardSymbol.Three, BlackJack.CardSuit.Spades)
                };

                var hand = new BlackJack.Hand(cards);

                Assert.False(hand.CanSurrender);
            }

            [Fact]
            public void Can_Surrender_Is_False_If_Values_Equals_21()
            {
                var cards = new List<BlackJack.Card>
                {
                    new BlackJack.Card(BlackJack.CardSymbol.King, BlackJack.CardSuit.Diamonds),
                    new BlackJack.Card(BlackJack.CardSymbol.Ace, BlackJack.CardSuit.Spades)
                };

                var hand = new BlackJack.Hand(cards);

                Assert.False(hand.CanSurrender);
            }
        }

        public class IsBlackJackTests
        {
            [Fact]
            public void Is_BlackJack_Is_True_If_There_Are_2_Cards_And_Value_Equals_21()
            {
                var cards = new List<BlackJack.Card>
                {
                    new BlackJack.Card(BlackJack.CardSymbol.King, BlackJack.CardSuit.Diamonds),
                    new BlackJack.Card(BlackJack.CardSymbol.Ace, BlackJack.CardSuit.Spades)
                };

                var hand = new BlackJack.Hand(cards);

                Assert.True(hand.IsBlackJack);
            }

            [Fact]
            public void Is_BlackJack_Is_True_If_There_Is_More_Then_2_Cards()
            {
                var cards = new List<BlackJack.Card>
                {
                    new BlackJack.Card(BlackJack.CardSymbol.Eight, BlackJack.CardSuit.Diamonds),
                    new BlackJack.Card(BlackJack.CardSymbol.Two, BlackJack.CardSuit.Diamonds),
                    new BlackJack.Card(BlackJack.CardSymbol.Ace, BlackJack.CardSuit.Spades)
                };

                var hand = new BlackJack.Hand(cards);

                Assert.False(hand.IsBlackJack);
            }

            [Fact]
            public void Is_BlackJack_Is_True_If_Value_Is_Not_21()
            {
                var cards = new List<BlackJack.Card>
                {
                    new BlackJack.Card(BlackJack.CardSymbol.Eight, BlackJack.CardSuit.Diamonds),
                    new BlackJack.Card(BlackJack.CardSymbol.Two, BlackJack.CardSuit.Diamonds),
                    new BlackJack.Card(BlackJack.CardSymbol.Ace, BlackJack.CardSuit.Spades)
                };

                var hand = new BlackJack.Hand(cards);

                Assert.False(hand.IsBlackJack);
            }
        }

        public class SplitTests
        {
            List<BlackJack.Card> cards;
            BlackJack.Hand hand;
            List<BlackJack.Card> expectedCards = new List<BlackJack.Card>();

            public SplitTests()
            {
                cards = new List<BlackJack.Card>
                {
                    new BlackJack.Card(BlackJack.CardSymbol.Teen, BlackJack.CardSuit.Diamonds),
                    new BlackJack.Card(BlackJack.CardSymbol.King, BlackJack.CardSuit.Clubs)
                };

                hand = new BlackJack.Hand(cards);
            }

            [Fact]
            public void Current_Hand_Has_Only_The_First_Card_After_Split()
            {
                expectedCards.Add(hand.GetCards()[0]);

                hand.Split();
                var actualCards = hand.GetCards();

                Assert.Equal(expectedCards, actualCards);
            }

            [Fact]
            public void New_Hand_Has_Only_The_Second_Card_From_Existing_Hand()
            {
                expectedCards.Add(hand.GetCards()[1]);

                var newHand = hand.Split();
                var actualCards = newHand.GetCards();

                Assert.Equal(expectedCards, actualCards);
            }
        }
    }
}
