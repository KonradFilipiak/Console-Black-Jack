using System;
using System.Collections.Generic;

namespace BlackJack
{
    public enum Option
    {
        Hit,
        Stand,
        Double,
        Split,
        Insurance,
        EvenMoney,
        NoEvenMoney,
        Surrender
    }

    public class UIManager
    {
        public bool IsCollectingInsurance { get; set; }
        public bool IsPayingEvenMoney { get; set; }

        List<Option> options = new List<Option>();

        //******************************************************************************************

        public int OptionsAmount => options.Count;

        //******************************************************************************************

        public void DisplayAll(Dealer dealer, List<Player> players, Player activePlayer = null)
        {
            Console.Clear();

            DisplayPlayersMoney(ref players);
            DisplaySeparator();
            DisplayDealer(ref dealer);
            DisplaySeparator();
            DisplayPlayers(ref players);

            if (!ReferenceEquals(activePlayer, null))
            {
                DesignOptions(dealer, activePlayer);
            }
            DisplayOptions();
        }

        public Option? GetOption(int index)
        {
            if (index < 0 || index > options.Count)
            {
                return null;
            }

            return options[index];
        }

        //******************************************************************************************

        void DesignOptions(Dealer dealer, Player activePlayer)
        {
            options.Clear();

            if (IsCollectingInsurance)
            {
                int maxInsurance = activePlayer.MaxInsurance;
                options.Add(Option.Insurance);
            }
            else if (IsPayingEvenMoney)
            {
                options.Add(Option.EvenMoney);
                options.Add(Option.NoEvenMoney);
            }
            else
            {
                options.Add(Option.Hit);
                options.Add(Option.Stand);

                if (activePlayer.CanSurreder && !dealer.HasOnlyAce)
                {
                    options.Add(Option.Surrender);
                }
                if (activePlayer.CanDouble)
                {
                    options.Add(Option.Double);
                }
                if (activePlayer.CanSplit)
                {
                    options.Add(Option.Split);
                }
            }
        }

        void DisplayPlayersMoney(ref List<Player> players)
        {
            foreach (Player player in players)
            {
                Console.WriteLine("{0} money: {1}", player.PlayerName, player.Money);
            }
        }

        void DisplayDealer(ref Dealer dealer)
        {
            Console.WriteLine(dealer);
        }

        void DisplayPlayers(ref List<Player> players)
        {
            foreach (Player player in players)
            {
                Console.WriteLine(player);
                DisplaySeparator();
            }

        }

        void DisplaySeparator()
        {
            Console.WriteLine("----------------------------------------------------------------------");
        }

        void DisplayOptions()
        {
            int optionsAmount = 1;

            foreach (Option option in options)
            {
                Console.WriteLine(Convert.ToString(optionsAmount) + ". " + option);
                ++optionsAmount;
            }
        }
    }
}
