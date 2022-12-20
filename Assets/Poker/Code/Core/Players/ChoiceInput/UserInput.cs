using System;
using Poker.Code.Core.UI.Gameplay.PlayerChoice;

namespace Poker.Code.Core.Players.ChoiceInput
{
    public class UserInput : IPlayerInput, IDisposable
    {
        public event Action<int> OnRaise;
        public event Action OnFold;
        public event Action OnCheckCall;
        public event Action OnAllIn;

        private readonly BetSlider _betSlider;
        private readonly PlayerChoiceButtons _choiceButtons;

        private int _raiseBet;

        public UserInput(ChoicePanel choicePanel)
        {
            _betSlider = choicePanel.BetSlider;
            _choiceButtons = choicePanel.ChoiceButtons;
        }

        public void Enable(Player player, int toCall)
        {
            int minRaise = toCall * 2;
            bool minRaiseLessThenBalance = minRaise < player.Balance;
            _raiseBet = minRaise;

            PrepareChoiceButtons(player.Bet, toCall);
            
            if (minRaiseLessThenBalance)
            {
                _betSlider.Prepare(player.Balance, minRaise);
                _betSlider.Show();
            }
            _choiceButtons.SetButtonsActive(minRaiseLessThenBalance, true, true, minRaiseLessThenBalance);
            SubscribeUserInterface();
        }

        public void Disable()
        {
            _betSlider.Hide();
            _choiceButtons.SetButtonsActive(false, false, false, false);
            UnsubscribeUserInterface();
        }

        public void Dispose()
        {
            if(OnFold != null) UnsubscribeUserInterface();
        }

        private void PrepareChoiceButtons(int playerBet, int toCall)
        {
            if (toCall == playerBet) _choiceButtons.SetCheckView();
            else _choiceButtons.SetCallView();
        }

        private void SendFold()
        {
            Disable();
            OnFold?.Invoke();
        }

        private void SendCheckCall()
        {
            Disable();
            OnCheckCall?.Invoke();
        }

        private void SendRaise()
        {
            Disable();
            OnRaise?.Invoke(_raiseBet);
        }

        private void SendAllIn()
        {
            Disable();
            OnAllIn?.Invoke();
        }

        private void SetRaiseBet(int newBet) => _raiseBet = newBet;

        private void SubscribeUserInterface()
        {
            _choiceButtons.OnFoldClick += SendFold;
            _choiceButtons.OnCheckCallClick += SendCheckCall;
            _choiceButtons.OnRaiseClick += SendRaise;
            _choiceButtons.OnAllInClick += SendAllIn;
            _betSlider.OnBetUpdate += SetRaiseBet;
        }

        private void UnsubscribeUserInterface()
        {
            _choiceButtons.OnFoldClick -= SendFold;
            _choiceButtons.OnCheckCallClick -= SendCheckCall;
            _choiceButtons.OnRaiseClick -= SendRaise;
            _choiceButtons.OnAllInClick -= SendAllIn;
            _betSlider.OnBetUpdate -= SetRaiseBet;
        }
    }
}