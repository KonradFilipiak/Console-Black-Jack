using System;
using System.Collections.Generic;
using System.Linq;

namespace BlackJack
{
    public class Player
    {
        public string PlayerName { get; set; }

        List<Box> boxes = new List<Box>();
        int money;
        int insurance;

        //******************************************************************************************

        public Player(string playerName, int money, int firstBet, params int[] bets)
        {
            PlayerName = playerName;
            Money = money;

            // Initializing first box
            AddBox(playerName + "1", firstBet);

            //Initializing rest of the boxes
            int boxesLenght = bets.Length;
            for (int i = 0; i < boxesLenght; ++i)
            {
                AddBox(PlayerName + " " + Convert.ToString(i + 2), bets[i]);
            }
        }

        public Player(string playerName, int money, int[] bets)
        {
            PlayerName = playerName;
            Money = money;

            int boxesLength = bets.Length;
            for (int i = 0; i < boxesLength; ++i)
            {
                AddBox(PlayerName + " " + Convert.ToString(i + 1), bets[i]);
            }
        }

        //******************************************************************************************

        public int Money
        {
            get { return money; }
            set
            {
                if (value >= 0)
                {
                    money = value;
                }
            }
        }

        public int Insurance
        {
            get { return insurance; }
            set
            {
                if (value >= 0 && money - value >= 0 && value <= MaxInsurance)
                {
                    insurance = value;
                }
            }
        }

        public int MaxInsurance
        {
            get
            {
                int maxInsurance = 0;

                foreach (Box box in boxes)
                {
                    maxInsurance += box.BetSize;
                }

                return (maxInsurance / 2 < Money) ? maxInsurance / 2 : Money;
            }
        }

        public BoxState State => ActiveBox.State;

        public bool CanDouble
        {
            get
            {
                Box activeBox = ActiveBox;

                if (Money >= activeBox.BetSize)
                {
                    return activeBox.CanDouble;
                }

                return false;
            }
        }

        public bool CanSplit
        {
            get
            {
                Box activeBox = ActiveBox;

                if (Money >= activeBox.BetSize)
                {
                    return activeBox.CanSplit;
                }

                return false;
            }
        }

        public bool CanSurreder
        {
            get { return ActiveBox.CanSurrender; }
        }

        public bool IsBlackJack
        {
            get { return ActiveBox.IsBlackJack; }
        }

        public bool HasActiveBox
        {
            get
            {
                return ActiveBox != null;
            }
        }

        public int BoxesCount
        {
            get { return boxes.Count; }
        }

        public Box ActiveBox
        {
            get
            {
                foreach (Box box in boxes)
                {
                    if (!box.IsFinishedHitting)
                    {
                        box.IsDeciding = true;
                        return box;
                    }
                }

                // If all boxes have finished playing
                return null;
            }
        }

        //******************************************************************************************

        public override string ToString()
        {
            string toReturn = "";

            foreach (Box box in boxes)
            {
                toReturn += box.ToString();
                if (!(boxes.Last() == box))
                {
                    toReturn += "\n\n";
                }
            }

            return toReturn;
        }

        //******************************************************************************************

        public void AddCard(Card card)
        {
            ActiveBox.AddCard(card);
        }

        public void AddCardToEveryBox(Card[] cards)
        {
            int boxNum = 0;
            foreach (Box box in boxes)
            {
                box.AddCard(cards[boxNum]);
                ++boxNum;
            }
        }

        public void Stand()
        {
            ActiveBox.Stand();
        }

        public void Double(Card card)
        {
            if (CanDouble)
            {
                Money -= ActiveBox.BetSize;
                ActiveBox.Double(card);
            }
        }

        public void Surrender()
        {
            if (CanSurreder)
            {
                int moneyBack = ActiveBox.Surrender();
                Money += moneyBack;
            }
        }

        public void Split()
        {
            if (CanSplit)
            {
                Box activeBox = ActiveBox;

                Money -= activeBox.BetSize;

                Box newBox = activeBox.Split();

                // Replacing the boxes list to make sure that the newBox is put in the right place (just behind the splited box)
                List<Box> newBoxesList = new List<Box>();
                foreach (Box box in boxes)
                {
                    newBoxesList.Add(box);
                    if (box == activeBox)
                    {
                        newBoxesList.Add(newBox);
                    }
                }
                boxes = newBoxesList;
            }
        }

        // Called after each hand
        public void Reset()
        {
            // Remvoes temporary boxes and sets nontemporary to initial state (without cards and double states)
            List<Box> newList = new List<Box>();
            foreach (Box box in boxes)
            {
                if (!box.IsTemporary)
                {
                    if (box.State == BoxState.Double)
                    {
                        newList.Add(new Box(box.BoxName, box.BetSize / 2));
                    }
                    else
                    {
                        newList.Add(new Box(box.BoxName, box.BetSize));
                    }
                }
            }

            // I add each individual box instead of just "this.boxes = newList;"
            // because this way I can control if the box can be actually added
            // (for example the box will not be added if the player has no money)
            boxes = new List<Box>();
            foreach (Box box in newList)
            {
                if (Money - box.BetSize > 0)
                {
                    Money -= box.BetSize;
                    boxes.Add(box);
                }
            }

            Insurance = 0;
        }

        public List<Box> GetBoxesWithState(BoxState state)
        {
            List<Box> boxesToReturn = new List<Box>();

            foreach (Box box in boxes)
            {
                if (box.State == state)
                {
                    boxesToReturn.Add(box);
                }
            }

            return boxesToReturn;
        }

        public bool HasBoxWithState(BoxState state)
        {
            foreach (Box box in boxes)
            {
                if (box.State == state)
                    return true;
            }

            return false;
        }

        //******************************************************************************************

        void AddBox(string boxName, int bet)
        {
            if (bet > 0 && Money - bet >= 0)
            {
                Money -= bet;
                boxes.Add(new Box(boxName, bet));
            }
        }

        //******************************************************************************************
    }
}
