using System;
using Poker.Code.Core.Cards;
using Poker.Code.Core.Players.ChoiceInput;
using Poker.Code.Data.Enums;
using Poker.Code.Services.Sound;
using UnityEngine;

namespace Poker.Code.Core.Players
{
    public class Player : IDisposable
    {
        public event Action OnBalanceChanged;
        public event Action OnFold;
        public event Action OnRaise;
        public event Action OnCheckCall;

        public bool IsFold { get; set; }

        public int Balance
        {
            get => _balance;
            set
            {
                _balance = value;
                OnBalanceChanged?.Invoke();
            }
        }

        public Blind Blind { get; set; }
        public int Bet { get; private set; }
        public CardPair CardPair { get; }
        public Vector3 IndicatorPosition { get; }

        private readonly PlayerBetView _betView;
        private readonly IPlayerInput _input;
        private readonly ISoundService _soundService;

        private int _balance;
        private int _toCall;

        public Player(IPlayerInput input, PlayerBetView betView, CardPair cardPair,
            ISoundService soundService ,int startBalance, Vector3 indicatorPosition)
        {
            _soundService = soundService;
            _betView = betView;
            CardPair = cardPair;
            _input = input;
            Balance = startBalance;
            IndicatorPosition = indicatorPosition;

            SubscribeInput();
        }

        public void StartChoice(int maxBet)
        {
            _toCall = maxBet - Bet;
            _input.Enable(this, maxBet);
        }

        public void Dispose() => UnsubscribeInput();

        public void SetBet(int bet)
        {
            Bet = bet;
            _betView.SetBet(Bet);
        }

        public void Fold()
        {
            IsFold = true;
            OnFold?.Invoke();
            _input.Disable();
        }

        public void Raise(int raiseBet)
        {
            SetBet(raiseBet);
            MakeBet(Bet);
            OnRaise?.Invoke();
            _soundService.PlayEffectSound(SoundId.Chips);
            _input.Disable();
        }

        public void CheckCall()
        {
            _soundService.PlayEffectSound(_toCall == 0 ? SoundId.Check : SoundId.Chips);
            MakeBet(_toCall);
            SetBet(Bet + _toCall);
            OnCheckCall?.Invoke();
            _input.Disable();
        }

        public void MakeBet(int bet) => Balance -= bet;

        private void AllIn() => Raise(Balance - Bet);

        private void SubscribeInput()
        {
            _input.OnFold += Fold;
            _input.OnCheckCall += CheckCall;
            _input.OnRaise += Raise;
            _input.OnAllIn += AllIn;
        }

        private void UnsubscribeInput()
        {
            _input.OnFold -= Fold;
            _input.OnCheckCall -= CheckCall;
            _input.OnRaise -= Raise;
            _input.OnAllIn -= AllIn;
        }
    }
}