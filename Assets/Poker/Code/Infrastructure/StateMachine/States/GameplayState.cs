using System;
using System.Threading.Tasks;
using Poker.Code.Core.Cards.CardStack;
using Poker.Code.Core.Cards.View;
using Poker.Code.Core.Combinations;
using Poker.Code.Core.GameplayLoop;
using Poker.Code.Core.Players;
using Poker.Code.Core.UI.Gameplay.TopPanel;
using Poker.Code.Data.Extensions;
using Poker.Code.Data.StaticData.Locations;
using Poker.Code.Services.CardMove;
using Poker.Code.Services.Factories.GameFactory;
using Poker.Code.Services.Factories.UIFactory;

namespace Poker.Code.Infrastructure.StateMachine.States
{
    public class GameplayState : IState
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly IUIFactory _uiFactory;
        private readonly IGameFactory _gameFactory;
        private readonly ICardMover _cardMover;

        private Player[] _players;
        private ICardStack _cardStack;
        private CardViewDeck _viewDeck;
        private PlayerStepReporter _stepReporter;
        private BlindConfigurator _blindConfigurator;
        private UserInfoTexts _userInfo;
        private ICombinationComparer _combinationComparer;

        private int _chipsPot;
        private int _smallBlindPlayerIndex;
        private Action _stackShowAction;

        public GameplayState(IGameStateMachine stateMachine, IUIFactory uiFactory, 
            IGameFactory gameFactory, ICardMover cardMover)
        {
            _stateMachine = stateMachine;
            _uiFactory = uiFactory;
            _gameFactory = gameFactory;
            _cardMover = cardMover;
        }
        
        public void Enter()
        {
            SetGameplayDependencies();
            ConfigurePlayerBlinds();

            _chipsPot = 0;
            _stackShowAction = SetFlop;
            _stepReporter.OnPlayerFolded += MovePlayerCardToFoldLocation;
            _stepReporter.OnStepCycleFinished += OnPlayerStepCycleFinished;
            _stepReporter.StartStepCycle(_players);
        }

        public void Exit()
        {
            _stepReporter.OnPlayerFolded -= MovePlayerCardToFoldLocation;
            _stepReporter.OnStepCycleFinished -= OnPlayerStepCycleFinished;
        }

        private void OnPlayerStepCycleFinished(int inGamePlayersCount)
        {
            CollectBetsFromPlayers();

            if (inGamePlayersCount == 1)
            {
                FinishGameplayCycle();
                return;
            }
            
            _stackShowAction.Invoke();
        }

        private void SetFlop()
        {
            _stackShowAction = SetTorn;
            StartStackStepCycle(3);
        }

        private void SetTorn()
        {
            _stackShowAction = SetRiver;
            StartStackStepCycle(1);
        }

        private void SetRiver()
        {
            _stackShowAction = FinishGameplayCycle; 
            StartStackStepCycle(1);
        }

        private async Task ShowCardsFromStack(int cardCount)
        {
            foreach (CardView cardView in _cardStack.GetCardForShow(cardCount))
                await _cardMover.ShowCard(cardView);
        }

        private async void StartStackStepCycle(int openCardsCount)
        {
            await ShowCardsFromStack(openCardsCount);
            _combinationComparer.EvaluatePlayerCombinations(_stepReporter.InGamePlayers);
            _stepReporter.StartStepCycle(_players);
        }

        private void FinishGameplayCycle() => _stateMachine.Enter<FinishGameplayState, int>(_chipsPot);

        private async void MovePlayerCardToFoldLocation(Player player)
        {
            Location foldLocation = _viewDeck.Location;
            foreach (CardView cardView in player.CardPair.CardViews)
            {
                _cardMover.RotateCard(cardView, foldLocation.Rotation.eulerAngles);
                await _cardMover.MoveCard(cardView, foldLocation.Position);
            }
            _viewDeck.ReturnCards(player.CardPair.CardViews);
        }

        private void CollectBetsFromPlayers()
        {
            foreach (Player player in _players)
            {
                _chipsPot += player.Bet;
                player.SetBet(0);
            }
            _userInfo.SetPot(_chipsPot);
        }

        private void SetGameplayDependencies()
        {
            _players = _gameFactory.AllPlayers;
            _cardStack = _gameFactory.CardStack;
            _viewDeck = _gameFactory.ViewCardDeck;
            _stepReporter = _gameFactory.StepReporter;
            _blindConfigurator = _gameFactory.BlindConfigurator;
            _userInfo = _uiFactory.TopPanelView.InfoTexts;
            _combinationComparer = _gameFactory.CombinationComparer;
        }

        private void ConfigurePlayerBlinds()
        {
            _smallBlindPlayerIndex = _blindConfigurator.SetBlindForEachPlayer(_players);
            _blindConfigurator.SetBlindBetForEachPlayer(_players);
            _players.SortArrayByIndex(_smallBlindPlayerIndex);
        }
    }
}