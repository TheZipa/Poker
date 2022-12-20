using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using Poker.Code.Core.Cards;
using Poker.Code.Core.Cards.CardStack;
using Poker.Code.Core.Cards.ModelCardDeck;
using Poker.Code.Core.Cards.View;
using Poker.Code.Core.Combinations;
using Poker.Code.Core.Indicators;
using Poker.Code.Core.Players;
using Poker.Code.Services.CardMove;
using Poker.Code.Services.Factories.GameFactory;
using Poker.Code.Services.Factories.UIFactory;
using Poker.Code.Services.UserBalance;
using UnityEngine;

namespace Poker.Code.Infrastructure.StateMachine.States
{
    public class FinishGameplayState : IPayloadedState<int>
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly IGameFactory _gameFactory;
        private readonly IUIFactory _uiFactory;
        private readonly IUserBalance _userBalance;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly ICardMover _cardMover;

        private const float WinDelayTime = 6.5f;

        private Player[] _inGamePlayers;
        private CardViewDeck _viewDeck;
        private ICardStack _cardStack;
        private IModelCardDeck _modelDeck;
        private ICombinationComparer _combinationComparer;
        private PlayerWinIndicators _winIndicators;

        public FinishGameplayState(IGameStateMachine stateMachine, IGameFactory gameFactory,
            IUIFactory uiFactory, IUserBalance userBalance, ICardMover cardMover, ICoroutineRunner coroutineRunner)
        {
            _userBalance = userBalance;
            _gameFactory = gameFactory;
            _uiFactory = uiFactory;
            _stateMachine = stateMachine;
            _coroutineRunner = coroutineRunner;
            _cardMover = cardMover;
        }
        
        public async void Enter(int chipsPot)
        {
            SetupDependencies();
            await ShowAllHands();
            CalculateWinners(chipsPot);
            _userBalance.SaveBalance();
            _uiFactory.TopPanelView.InfoTexts.SetPot(0);
            _coroutineRunner.StartCoroutine(ShowWinners());
        }

        public void Exit()
        {
        }

        private void SetupDependencies()
        {
            _inGamePlayers = _gameFactory.StepReporter.InGamePlayers;
            _viewDeck = _gameFactory.ViewCardDeck;
            _cardStack = _gameFactory.CardStack;
            _modelDeck = _gameFactory.ModelCardDeck;
            _combinationComparer = _gameFactory.CombinationComparer;
            _winIndicators = _gameFactory.WinIndicators;
        }

        private void EvaluatePlayerCombinations()
        {
            _combinationComparer.EvaluatePlayerCombinations(_inGamePlayers);
            _inGamePlayers = _combinationComparer.ComparePlayerCombinations(_inGamePlayers);
            
            Debug.Log("Winners count = " + _inGamePlayers.Length);
            Debug.Log("Combination - " + _inGamePlayers[0].CardPair.Hand.HandTypeValue);
        }

        private async Task ShowAllHands()
        {
            foreach (Player player in _inGamePlayers)
            {
                if(player == _gameFactory.User) continue;
                
                foreach (CardView cardView in player.CardPair.CardViews)
                    await _cardMover.ShowCard(cardView);
            }
        }

        private void CalculateWinners(int chipsPot)
        {
            if(TryPayChipsToOneWinner(chipsPot)) return;
            
            EvaluatePlayerCombinations();

            if(TryPayChipsToOneWinner(chipsPot)) return;
            
            PayChipsForWinners(_inGamePlayers, chipsPot);
        }

        private void PayChipsForWinners(Player[] players, int chips)
        {
            int chipsForPlayer = chips / players.Length;
            foreach (Player player in players)
                player.Balance += chipsForPlayer;
        }

        private bool TryPayChipsToOneWinner(int chips)
        {
            if (_inGamePlayers.Length != 1) return false;
            
            _inGamePlayers[0].Balance += chips;
            return true;
        }

        private IEnumerator ShowWinners()
        {
            _winIndicators.ShowWinners(_inGamePlayers);
            yield return new WaitForSeconds(WinDelayTime);
            _winIndicators.HideAll();
            StartNewGameplayCycle();
        }

        private void ReturnAllCards()
        {
            foreach (CardPair cardPair in _gameFactory.AllPlayers.Select(player => player.CardPair))
            {
                _modelDeck.ReturnCards(cardPair.CardModels);
                _viewDeck.ReturnCards(cardPair.CardViews);
            }
            
            _modelDeck.ReturnCards(_cardStack.BoardModelCards);
            _viewDeck.ReturnCards(_cardStack.BoardViewCards);
        }

        private void ResetPlayers()
        {
            foreach (Player player in _gameFactory.AllPlayers)
                player.IsFold = false;
        }

        private void StartNewGameplayCycle()
        {
            ResetPlayers();
            ReturnAllCards();
            _stateMachine.Enter<PrepareUserBalanceState>();
        }
    }
}