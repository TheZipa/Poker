using System;
using System.Threading.Tasks;
using HoldemHand;

namespace Poker.Code.Core.Players.ChoiceInput
{
    public class AiInput : IPlayerInput
    {
        public event Action<int> OnBetUpdate;
        public event Action<int> OnRaise;
        public event Action OnFold;
        public event Action OnCheckCall;
        public event Action OnAllIn;

        private readonly Random _random = new Random();
        
        public async void Enable(Player player, int toCall)
        {
            await Task.Delay(_random.Next(700, 3150));

            if (toCall > 0)
            {
                if(toCall <= 50 || player.CardPair.Hand.HandTypeValue >= Hand.HandTypes.Pair)
                    InvokeCheckCall(toCall);
                else
                    OnFold?.Invoke(); 
                return;
            }

            TryToRaise(player.CardPair.Hand.HandTypeValue, toCall);
        }

        public void Disable()
        {
        }

        private void TryToRaise(Hand.HandTypes combination, int toCall)
        {
            if (combination >= Hand.HandTypes.TwoPair)
                InvokeRaise(_random.Next(4, 35) * 5);
            else
                InvokeCheckCall(toCall);
        }

        private void InvokeCheckCall(int toCall)
        {
            OnBetUpdate?.Invoke(toCall);
            OnCheckCall?.Invoke();
        }

        private void InvokeRaise(int raise)
        {
            OnBetUpdate?.Invoke(raise);
            OnRaise?.Invoke(raise);
        }

        private void InvokeAllIn() => OnAllIn?.Invoke();
    }
}