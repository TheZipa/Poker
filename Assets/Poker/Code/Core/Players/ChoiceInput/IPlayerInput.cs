using System;

namespace Poker.Code.Core.Players.ChoiceInput
{
    public interface IPlayerInput
    {
        public event Action<int> OnRaise;
        public event Action OnFold;
        public event Action OnCheckCall;
        public event Action OnAllIn;
        public void Enable(Player player, int toCall);
        public void Disable();
    }
}