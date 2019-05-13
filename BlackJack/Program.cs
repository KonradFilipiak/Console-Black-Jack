using System;
using System.Collections.Generic;

namespace BlackJack
{
    class Program
    {
        const int maxBoxes = 7;
        static List<Player> players = new List<Player>();
        static int playersAmount;

        static void Main()
        {
            playersAmount = SetPlayersAmount();
            CreatePlayers();

            GameManager gm = new GameManager(players);
            while (true) // Just playing game in an infinite loop
            {
                gm.PlaySingleHand();
            }
        }

        //******************************************************************************************

        static int SetPlayersAmount()
        {
            int newPlayersAmount = 0;
            bool makingDecision = true;

            while (makingDecision)
            {
                try
                {
                    Console.Write("Number of players? (1-{0}): ", maxBoxes);
                    newPlayersAmount = Convert.ToInt32(Console.ReadLine());

                    if (newPlayersAmount < 1 || newPlayersAmount > maxBoxes)
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

            return newPlayersAmount;
        }

        static void CreatePlayers()
        {
            int boxesLeft = maxBoxes;
            for (int i = 0; i < playersAmount; ++i)
            {
                string playerName = SetPlayerName(i);
                int playerMoney = SetPlayerMoney(playerName);
                int[] playerBets = SetPlayerBets(playerName, playerMoney, ref boxesLeft);

                players.Add(new Player(playerName, playerMoney, playerBets));
            }
        }


        static string SetPlayerName(int playerNumber)
        {
            Console.Write("Player{0} name: ", playerNumber + 1);
            return Console.ReadLine();
        }

        static int SetPlayerMoney(string playerName)
        {
            int newPlayerMoney = 0;
            bool makingDecision = true;

            while (makingDecision)
            {
                try
                {
                    Console.Write("Money for {0} (integer, more than 10, less then 10 000): ", playerName);
                    newPlayerMoney = Convert.ToInt32(Console.ReadLine());

                    if (newPlayerMoney < 10 || newPlayerMoney > 10000)
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

            return newPlayerMoney;
        }

        static int[] SetPlayerBets(string playerName, int playerMoney, ref int boxesLeft)
        {
            int playerBoxesAmount = SetPlayerBoxesAmount(playerName, boxesLeft);
            boxesLeft -= playersAmount - 1;

            int[] playerBets = new int[playerBoxesAmount];
            for (int i = 0; i < playerBoxesAmount; i++)
            {
                int bet = 0;
                bool makingDecision = true;

                while (makingDecision)
                {
                    try
                    {
                        Console.Write("Bet amount on Box {0} (integer, remember you need to place bets on all boxes): ", i + 1);
                        bet = Convert.ToInt32(Console.ReadLine());

                        // Making sure that player can afford bets on all boxes that are left
                        if (bet > playerMoney - playerBoxesAmount * 10 || bet < 0)
                        {
                            Console.WriteLine("Wrong number! Try again.");
                        }
                        else
                        {
                            playerBets[i] = bet;
                            makingDecision = false;
                        }
                    }
                    catch
                    {
                        Console.WriteLine("It needs to be a number!");
                    }
                }
            }

            return playerBets;
        }

        static int SetPlayerBoxesAmount(string playerName, int boxesLeft)
        {
            int playerBoxesAmount = 0;
            bool makingDecision = true;

            while (makingDecision)
            {
                try
                {
                    Console.Write("Number of boxes for {0} (max {1}): ", playerName, boxesLeft);
                    playerBoxesAmount = Convert.ToInt32(Console.ReadLine());

                    if (playerBoxesAmount < 1 || playerBoxesAmount > boxesLeft)
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

            return playerBoxesAmount;
        }
    }
}