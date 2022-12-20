using System.Collections.Generic;
using Poker.Code.Core.Cards;
using Poker.Code.Core.Cards.CardStack;
using Poker.Code.Core.Cards.ModelCardDeck;
using Poker.Code.Core.Cards.View;
using Poker.Code.Core.Combinations;
using Poker.Code.Core.GameplayLoop;
using Poker.Code.Core.Indicators;
using Poker.Code.Core.Players;
using Poker.Code.Core.Players.ChoiceInput;
using Poker.Code.Core.Timer;
using Poker.Code.Data.Enums;
using Poker.Code.Data.StaticData.Locations;
using Poker.Code.Services.Dispose;
using Poker.Code.Services.PersistentProgress;
using Poker.Code.Services.Providers.AssetProvider;
using Poker.Code.Services.Sound;
using Poker.Code.Services.StaticData;
using UnityEngine;

namespace Poker.Code.Services.Factories.GameFactory
{
    public class GameFactory : IGameFactory
    {
        public IModelCardDeck ModelCardDeck { get; private set; }
        public CardViewDeck ViewCardDeck { get; private set; }
        public ICardStack CardStack { get; private set; }
        public Player User { get; private set; }
        public PlayerStepReporter StepReporter { get; private set; }
        public BlindConfigurator BlindConfigurator { get; private set; }
        public ICombinationComparer CombinationComparer { get; private set; }
        public PlayerWinIndicators WinIndicators { get; private set; }
        public Player[] AllPlayers { get; private set; }
        public string ActiveScene { get; set; }

        private readonly IDisposeService _disposeService;
        private readonly IAssetProvider _assetProvider;
        private readonly IStaticData _staticData;
        private readonly IPersistentProgress _playerProgress;
        private readonly ISoundService _soundService;

        public GameFactory(IDisposeService disposeService, IAssetProvider assetProvider, 
            IStaticData staticData, IPersistentProgress playerProgress, ISoundService soundService)
        {
            _soundService = soundService;
            _playerProgress = playerProgress;
            _staticData = staticData;
            _assetProvider = assetProvider;
            _disposeService = disposeService;
        }

        public IModelCardDeck CreateModelCardDeck()
        {
            Stack<CardModel> cardModels = new Stack<CardModel>(54);

            for (int i = 2; i < 15; i++)
            {
                CardValue cardValue = (CardValue) i;
                for (int j = 1; j < 5; j++)
                {
                    CardSuit cardSuit = (CardSuit) j;
                    cardModels.Push(new CardModel(cardValue, cardSuit));
                }
            }

            return ModelCardDeck = new ModelCardDeck(cardModels);
        }

        public Player[] CreateAllPlayers(UserInput userInput)
        {
            int aiCount = _playerProgress.Progress.Settings.AIPlayersCount;
            GameObject betViewPrefab = _assetProvider.GetPlayerBetViewPrefab();
            AllPlayers = new Player[aiCount + 1];
            
            CreateAIPlayers(aiCount, betViewPrefab);
            CreateUserPlayer(betViewPrefab, userInput);

            return AllPlayers;
        }

        public ICardStack CreateCardStack() => CardStack = new CardStack();

        public ICombinationComparer CreateCombinationComparer() =>
            CombinationComparer = new HoldemHandComparer(CardStack);

        public BlindConfigurator CreateBlindConfigurator() =>
            BlindConfigurator = new BlindConfigurator(_staticData.GameConfig.SmallBlind);

        public PlayerWinIndicators CreateWinIndicators()
        {
            Indicator[] winIndicators = new Indicator[_playerProgress.Progress.Settings.AIPlayersCount + 1];
            GameObject indicatorPrefab = _assetProvider.GetWinIndicatorPrefab();

            for (int i = 0; i < winIndicators.Length; i++)
            {
                Indicator winIndicator = winIndicators[i] = Object.Instantiate(indicatorPrefab).GetComponent<Indicator>();
                winIndicator.Hide();
            }
            
            return WinIndicators = new PlayerWinIndicators(winIndicators);
        }

        public ITimer CreateTimer(ITimerView timerView) =>
            new Timer(timerView, _staticData.GameConfig.PlayerStepTime);

        public PlayerStepReporter CreatePlayerStepReporter(ITimer timer)
        {
            Indicator choiceIndicator = Object
                .Instantiate(_assetProvider.GetChoiceIndicatorPrefab())
                .GetComponent<Indicator>();
            choiceIndicator.Hide();
            StepReporter = new PlayerStepReporter(timer, choiceIndicator);
            _disposeService.RegisterDisposable(StepReporter, ActiveScene);
            return StepReporter;
        }

        public CardViewDeck CreateViewCardDeck()
        {
            Location cardViewLocation = _staticData.LocationData.CardViewLocation;
            CardMaterialProvider materialProvider = new CardMaterialProvider();
            materialProvider.LoadCardMaterials();
            Stack<CardView> cardViews = CreateCardViews(_playerProgress.Progress.Settings.AIPlayersCount + 1,
                _assetProvider.GetCardViewPrefab(), cardViewLocation);
            return ViewCardDeck = new CardViewDeck(cardViews, materialProvider, cardViewLocation);
        }

        private void CreateAIPlayers(int aiCount, GameObject betViewPrefab)
        {
            PlayerLocation[] aiLocations = _staticData.LocationData.AILocations;
            Player[] aiPlayers = new Player[aiCount];
            for (int i = 0; i < aiCount; i++)
            {
                PlayerBetView betView = CreatePlayerBetView(betViewPrefab, aiLocations[i].PlayerTextLocation);
                aiPlayers[i] = new Player(new AiInput(), betView, new CardPair(
                    aiLocations[i].LeftCardLocation, aiLocations[i].RightCardLocation), 
                    _soundService,999999999, aiLocations[i].TurnMarkerLocation.Position);
                _disposeService.RegisterDisposable(aiPlayers[i], ActiveScene);
            }

            aiPlayers.CopyTo(AllPlayers, 0);
        }

        private void CreateUserPlayer(GameObject betViewPrefab, UserInput userInput)
        {
            PlayerLocation userLocation = _staticData.LocationData.UserLocation;
            PlayerBetView userBetView = CreatePlayerBetView(betViewPrefab, userLocation.PlayerTextLocation);
            User = new Player(userInput, userBetView,
                new CardPair(userLocation.LeftCardLocation, userLocation.RightCardLocation),
                _soundService, _playerProgress.Progress.Balance.Chips, userLocation.TurnMarkerLocation.Position);
            _disposeService.RegisterDisposable(User, ActiveScene);
            AllPlayers[AllPlayers.Length - 1] = User;
        }
        
        private Stack<CardView> CreateCardViews(int playerCount, GameObject cardPrefab, Location location)
        {
            int cardsCount = GetMaxCardViewCount(playerCount);
            Stack<CardView> cardViews = new Stack<CardView>(cardsCount);

            for (int i = 0; i < cardsCount; i++)
            {
                GameObject cardObject = Object.Instantiate(cardPrefab, location.Position, location.Rotation);
                cardObject.SetActive(false);
                cardViews.Push(cardObject.GetComponent<CardView>());
            }

            return cardViews;
        }

        private PlayerBetView CreatePlayerBetView(GameObject prefab, Location location) =>
            Object.Instantiate(prefab, location.Position, Quaternion.Euler(new Vector3(90, -90, 0)))
                .GetComponent<PlayerBetView>();

        private int GetMaxCardViewCount(int playerCount) => playerCount * 2 + 5;
    }
}