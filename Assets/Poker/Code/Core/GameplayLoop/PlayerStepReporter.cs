using System;
using System.Linq;
using Poker.Code.Core.Indicators;
using Poker.Code.Core.Players;
using Poker.Code.Core.Timer;

namespace Poker.Code.Core.GameplayLoop
{
    public class PlayerStepReporter : IDisposable
    {
        public event Action<Player> OnPlayerFolded;
        public event Action<int> OnStepCycleFinished;
        
        public Player[] InGamePlayers { get; private set; }
        
        private readonly ITimer _stepTimer;
        private readonly Indicator _choiceIndicator;

        private Player[] _players;
        private int _currentPlayerStepIndex;
        private int _maxBet;

        public PlayerStepReporter(ITimer stepTimer, Indicator choiceIndicator)
        {
            _stepTimer = stepTimer;
            _choiceIndicator = choiceIndicator;

            _stepTimer.OnElapsed += FoldPlayerByTimeElapsed;
        }

        public void StartStepCycle(Player[] players)
        {
            _players = players;
            _currentPlayerStepIndex = 0;

            if (IsOnePlayerWithBalance()) FinishPlayerStep();

            FindMaxBet();
            StartPlayerStep();
        }

        public void Dispose() => _stepTimer.OnElapsed -= FoldPlayerByTimeElapsed;

        private void SubscribePlayerChoice(Player player)
        {
            player.OnFold += OnPlayerFold;
            player.OnCheckCall += OnPlayerCheckCall;
            player.OnRaise += OnPlayerRaise;
        }

        private void UnsubscribePlayerChoice(Player player)
        {
            player.OnFold -= OnPlayerFold;
            player.OnCheckCall -= OnPlayerCheckCall;
            player.OnRaise -= OnPlayerRaise;
        }

        private void OnPlayerFold()
        {
            OnPlayerFolded?.Invoke(GetCurrentStepPlayer());
            FinishPlayerStep();
        }

        private void OnPlayerCheckCall() => FinishPlayerStep();

        private void OnPlayerRaise()
        {
            _maxBet = GetCurrentStepPlayer().Bet;
            FinishPlayerStep();
        }

        private void FoldPlayerByTimeElapsed()
        {
            Player currentStepPlayer = GetCurrentStepPlayer();
            UnsubscribePlayerChoice(currentStepPlayer);
            currentStepPlayer.Fold();
            OnPlayerFolded?.Invoke(currentStepPlayer);
            _stepTimer.Stop();
            SwitchStepToNextPlayer();
        }

        private void StartPlayerStep()
        {
            Player currentStepPlayer = GetCurrentStepPlayer();
            
            if (currentStepPlayer.IsFold || currentStepPlayer.Balance == 0)
            {
                SwitchStepToNextPlayer();
                return;
            }

            SubscribePlayerChoice(currentStepPlayer);
            currentStepPlayer.StartChoice(_maxBet);
            _choiceIndicator.Show(currentStepPlayer.IndicatorPosition);
            _stepTimer.Start();
        }

        private void FinishPlayerStep()
        {
            UnsubscribePlayerChoice(GetCurrentStepPlayer());
            _stepTimer.Stop();
            SwitchStepToNextPlayer();
        }

        private Player GetCurrentStepPlayer() => _players[_currentPlayerStepIndex];

        private void SwitchStepToNextPlayer()
        {
            CollectInGamePlayers();

            if (InGamePlayers.Length == 1)
            {
                FinishStepCycle();
            }
            else if (++_currentPlayerStepIndex == _players.Length)
            {
                if (IsAllPlayersReady() == false)
                {
                    StartStepCycle(_players);
                    return;
                }

                FinishStepCycle();
            }
            else
            {
                StartPlayerStep();
            }
        }

        private void FindMaxBet()
        {
            _maxBet = 0;
            foreach (Player player in _players)
            {
                if (player.Bet > _maxBet)
                    _maxBet = player.Bet;
            }
        }

        private bool IsOnePlayerWithBalance()
        {
            int counter = _players.Count(player => player.IsFold == false && player.Balance != 0);
            return counter == 1;
        }

        private bool IsAllPlayersReady()
        {
            int readyPlayersCount = _players.Count(player =>
                (player.IsFold || player.Balance == 0) || (player.Bet == _maxBet && player.IsFold == false));
            return readyPlayersCount == _players.Length;
        }
        
        private void CollectInGamePlayers() => 
            InGamePlayers = _players.Where(player => player.IsFold == false).ToArray();

        private void FinishStepCycle()
        {
            //CollectInGamePlayers();
            _choiceIndicator.Hide();
            OnStepCycleFinished?.Invoke(InGamePlayers.Length);
        }
    }
}