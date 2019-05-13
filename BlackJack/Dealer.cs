using System.Collections.Generic;

namespace BlackJack
{
    public class Dealer : Box
    {
        public Dealer()
            : base("Dealer", 1)
        { }

        //******************************************************************************************

        public bool HasOnlyAce => ValueAsString == "1/11";

        //******************************************************************************************

        public override string ToString()
        {
            if (IsFinishedHitting && IsBlackJack)
            {
                return BoxName + " " + State + "\n" +
                        hand + "\n" +
                        "Value: " + ValueAsString;

            }

            return BoxName + " " + State + "\n" +
                hand + "\n" +
                "Value: " + Value;
        }

        //******************************************************************************************

        public bool ShouldStopHitting(List<Player> players)
        {
            bool isSomoneStillPlaying = IsSomeoneStillPlaying(ref players);
            bool isPlayerWithInsurance = IsPlayerWithInsurance(ref players);

            if (isPlayerWithInsurance && HasOnlyAce)
            {
                return false;
            }

            if (!isSomoneStillPlaying)
            {
                State = BoxState.Standing;
                return true;
            }

            if (Value > 16)
            {
                if (Value > 21)
                {
                    State = BoxState.TooMany;
                }
                else
                {
                    State = BoxState.Standing;
                }

                return true;
            }

            return false;
        }

        //******************************************************************************************

        bool IsSomeoneStillPlaying(ref List<Player> players)
        {
            foreach (var player in players)
            {
                if (player.HasBoxWithState(BoxState.Standing) || player.HasBoxWithState(BoxState.Double))
                {
                    return true;
                }
            }

            return false;
        }

        bool IsPlayerWithInsurance(ref List<Player> players)
        {
            foreach (var player in players)
            {
                if (player.Insurance > 0)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
