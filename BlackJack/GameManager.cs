using System;
using System.Collections.Generic;

namespace BlackJack
{
    public class GameManager
    {
        CardsShoe shoe = new CardsShoe();
        Dealer dealer = new Dealer();
        UIManager uiManager = new UIManager();
        List<Player> players = new List<Player>();

        //******************************************************************************************

        public GameManager(Player firstPlayer, params Player[] players)
        {
            this.players.Add(firstPlayer);

            foreach (Player player in players)
            {
                this.players.Add(player);
            }
        }

        public GameManager(Player[] players)
        {
            foreach (Player player in players)
            {
                this.players.Add(player);
            }
        }

        public GameManager(List<Player> players)
        {
            foreach (Player player in players)
            {
                this.players.Add(player);
            }
        }

        //******************************************************************************************

        public void PlaySingleHand()
        {
            StartHand();

            if (dealer.HasOnlyAce)
            {
                CollectInsurance();
                DecideOnEvenMoney();
            }

            // Players
            foreach (Player player in players)
            {
                while (player.HasActiveBox)
                {
                    if (player.ActiveBox.CardsAmount == 1)
                    {
                        player.ActiveBox.AddCard(PopCardFromShoe());
                    }

                    uiManager.DisplayAll(dealer, players, player);
                    int optionsAmount = uiManager.OptionsAmount;

                    // Making decision
                    bool makingDecision = true;
                    int decision = 0;
                    while (makingDecision)
                    {
                        try
                        {
                            decision = Convert.ToInt32(Console.ReadLine());

                            if (decision < 1 || decision > optionsAmount)
                            {
                                Console.WriteLine("Wrong number! Try again.");
                            }
                            else
                            {
                                makingDecision = false;
                            }
                        }
                        catch
                        {
                            Console.WriteLine("It needs to be a number!");
                        }
                    }

                    Option? choosenOption = uiManager.GetOption(decision - 1);

                    // Taking action based on decision
                    switch (choosenOption)
                    {
                        case Option.Hit:
                            player.AddCard(PopCardFromShoe());
                            break;
                        case Option.Stand:
                            player.Stand();
                            break;
                        case Option.Surrender:
                            player.Surrender();
                            break;
                        case Option.Double:
                            player.Double(PopCardFromShoe());
                            break;
                        case Option.Split:
                            player.Split();
                            break;
                    }
                }
            }

            // Dealer
            while (!dealer.ShouldStopHitting(players))
            {
                dealer.AddCard(PopCardFromShoe());
            }

            // Playing is over, summarying the hand
            uiManager.DisplayAll(dealer, players);

            PayoutWinnings();

            Console.WriteLine("Press Enter to play another hand...");
            Console.ReadLine();

            Reset();
        }

        //******************************************************************************************

        void StartHand()
        {
            ReshuffleCards();
            DealCards();
        }

        void ReshuffleCards()
        {
            if (shoe.ShouldBeReshuffled)
            {
                shoe = new CardsShoe();
            }
        }

        void DealCards()
        {
            DealACardToEachPlayer();
            DealACardToDealer();
            DealACardToEachPlayer();
        }

        void DealACardToEachPlayer()
        {
            foreach (Player player in players)
            {
                AddCardToEveryBox(player);
            }
        }

        void DealACardToDealer()
        {
            dealer.AddCard(shoe.PopCard());
        }

        void PayoutWinnings()
        {
            PayoutInsurance();
            PayoutBoxes();
        }

        void PayoutInsurance()
        {
            if (dealer.IsBlackJack)
            {
                foreach (Player player in players)
                {
                    player.Money += player.Insurance * 3;
                }
            }
        }

        void PayoutBoxes()
        {
            foreach (Player player in players)
            {
                PayoutEvenMoney(player);
                PayoutBoxesInPlay(player);
            }
        }

        void PayoutEvenMoney(Player player)
        {
            List<Box> evenMoneyBoxes = player.GetBoxesWithState(BoxState.EvenMoney);

            foreach (Box box in evenMoneyBoxes)
            {
                player.Money += box.BetSize * 2;
            }
        }

        void PayoutBoxesInPlay(Player player)
        {
            if (dealer.State == BoxState.TooMany)
            {
                PayoutOnDealerTooMany(player);
            }
            else if (dealer.State == BoxState.BlackJack)
            {
                PayoutOnDealerBlackJack(player);
            }
            else if (dealer.State == BoxState.Standing)
            {
                PayoutOnDealerStanding(player, dealer.Value);
            }
        }

        void PayoutOnDealerTooMany(Player player)
        {
            List<Box> standing = player.GetBoxesWithState(BoxState.Standing);
            List<Box> doubling = player.GetBoxesWithState(BoxState.Double);
            List<Box> blackJacks = player.GetBoxesWithState(BoxState.BlackJack);

            foreach (Box box in standing)
            {
                PayoutStandingBox(player, box);
            }

            foreach (Box box in doubling)
            {
                PayoutDoublingBox(player, box);
            }

            foreach (Box box in blackJacks)
            {
                PayoutBlackJackBox(player, box);
            }
        }

        void PayoutOnDealerBlackJack(Player player)
        {
            List<Box> blackJacks = player.GetBoxesWithState(BoxState.BlackJack);

            foreach (Box box in blackJacks)
            {
                PayoutDrawingBox(player, box);
            }
        }

        void PayoutOnDealerStanding(Player player, int dealerValue)
        {
            List<Box> standing = player.GetBoxesWithState(BoxState.Standing);
            List<Box> doubling = player.GetBoxesWithState(BoxState.Double);
            List<Box> blackJacks = player.GetBoxesWithState(BoxState.BlackJack);

            foreach (Box box in standing)
            {
                if (box.Value == dealerValue)
                {
                    PayoutDrawingBox(player, box);
                }
                else if (box.Value > dealerValue)
                {
                    PayoutStandingBox(player, box);
                }
            }

            foreach (Box box in doubling)
            {
                if (box.Value == dealerValue)
                {
                    PayoutDrawingBox(player, box);
                }
                else if (box.Value > dealerValue)
                {
                    PayoutStandingBox(player, box);
                }
            }

            foreach (Box box in blackJacks)
            {
                PayoutBlackJackBox(player, box);
            }
        }

        void PayoutStandingBox(Player player, Box box)
        {
            player.Money += box.BetSize * 2;
        }

        void PayoutDoublingBox(Player player, Box box)
        {
            player.Money += (int)(box.BetSize * 2.5);
        }

        void PayoutBlackJackBox(Player player, Box box)
        {
            player.Money += (int)(box.BetSize * 2.5);
        }

        void PayoutDrawingBox(Player player, Box box)
        {
            player.Money += box.BetSize;
        }

        void CollectInsurance()
        {
            uiManager.IsCollectingInsurance = true;

            foreach (Player player in players)
            {
                uiManager.DisplayAll(dealer, players, player);

                // Making decision
                bool makingDecision = true;
                int insuranceAmount = 0;
                int maxInsurance = player.MaxInsurance;
                if (maxInsurance < 5)  // If player doesn't have enough money, then don't even ask
                {
                    break;
                }
                while (makingDecision)
                {
                    try
                    {
                        Console.WriteLine("{0}: Min insurance: 0, Max insurance: {1}", player.PlayerName, maxInsurance);
                        insuranceAmount = Convert.ToInt32(Console.ReadLine());

                        if (insuranceAmount < 0 || insuranceAmount > maxInsurance)
                        {
                            Console.WriteLine("Wrong number! Try again.");
                        }
                        else
                        {
                            makingDecision = false;
                        }
                    }
                    catch
                    {
                        Console.WriteLine("It needs to be a number!");
                    }
                }

                player.Insurance = insuranceAmount;
                player.Money -= insuranceAmount;
            }

            uiManager.IsCollectingInsurance = false;
        }

        void DecideOnEvenMoney()
        {
            uiManager.IsPayingEvenMoney = true;

            foreach (Player player in players)
            {
                if (player.HasBoxWithState(BoxState.BlackJack))
                {
                    uiManager.DisplayAll(dealer, players, player);
                    int optionsAmount = uiManager.OptionsAmount;

                    // Making decision
                    bool makingDecision = true;
                    int decision = 0;
                    while (makingDecision)
                    {
                        try
                        {
                            Console.WriteLine("{0}: do you want to take even money on your BlackJacks?");
                            decision = Convert.ToInt32(Console.ReadLine());

                            if (decision < 1 || decision > optionsAmount)
                            {
                                Console.WriteLine("Wrong number! Try again.");
                            }
                            else
                            {
                                makingDecision = false;
                            }
                        }
                        catch
                        {
                            Console.WriteLine("It needs to be a number!");
                        }
                    }

                    Option? optionChosen = uiManager.GetOption(decision - 1);

                    // Taking action based on decision
                    switch (optionChosen)
                    {
                        case Option.EvenMoney:
                            List<Box> blackJacks = player.GetBoxesWithState(BoxState.BlackJack);
                            foreach (Box box in blackJacks)
                            {
                                box.TakeEvenMoney();
                            }
                            break;
                        case Option.NoEvenMoney:
                            // Doing nothing in this case
                            break;
                    }
                }
            }

            uiManager.IsPayingEvenMoney = false;
        }

        void Reset()
        {
            dealer = new Dealer();

            if (shoe.ShouldBeReshuffled)
            {
                shoe = new CardsShoe();
            }

            foreach (Player player in players)
            {
                player.Reset();
            }
        }

        Card PopCardFromShoe()
        {
            Card card = shoe.PopCard();
            if (card == null)
            {
                shoe = new CardsShoe();
                card = shoe.PopCard();
            }

            return card;
        }

        void AddCardToEveryBox(Player player)
        {
            int boxesCount = player.BoxesCount;
            Card[] cards = new Card[boxesCount];

            for (int i = 0; i < boxesCount; ++i)
            {
                cards[i] = PopCardFromShoe();
            }

            player.AddCardToEveryBox(cards);
        }
    }
}
