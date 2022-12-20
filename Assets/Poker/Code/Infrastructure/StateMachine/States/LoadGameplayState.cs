using Poker.Code.Core.Players.ChoiceInput;
using Poker.Code.Core.Timer;
using Poker.Code.Services.Factories.GameFactory;
using Poker.Code.Services.Factories.UIFactory;
using Poker.Code.Services.SceneLoader;
using Poker.Code.Services.UserBalance;
using UnityEngine;

namespace Poker.Code.Infrastructure.StateMachine.States
{
    public class LoadGameplayState : IState
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly IGameFactory _gameFactory;
        private readonly IUIFactory _uiFactory;
        private readonly IUserBalance _userBalance;
        private readonly ISceneLoader _sceneLoader;

        private const string GameplayScene = "Gameplay";

        public LoadGameplayState(IGameStateMachine stateMachine, ISceneLoader sceneLoader,
            IGameFactory gameFactory, IUIFactory uiFactory, IUserBalance userBalance)
        {
            _gameFactory = gameFactory;
            _uiFactory = uiFactory;
            _userBalance = userBalance;
            _sceneLoader = sceneLoader;
            _stateMachine = stateMachine;
        }
        
        public void Enter() =>
            _sceneLoader.LoadScene(GameplayScene, LoadGame);

        public void Exit()
        {
        }

        private void CreateUI()
        {
            Transform rootUITransform = _uiFactory.CreateRootCanvas().transform;
            _uiFactory.ActiveScene = GameplayScene;
            _uiFactory.CreateChoicePanel(rootUITransform);
            _uiFactory.CreateTopPanel(rootUITransform);
            _uiFactory.CreateResetBalancePopup(rootUITransform);
        }

        private void CreateGameplayComponents()
        {
            _gameFactory.ActiveScene = GameplayScene;
            _gameFactory.CreateModelCardDeck();
            _gameFactory.CreateViewCardDeck();
            _gameFactory.CreateAllPlayers(new UserInput(_uiFactory.ChoicePanel));
            _gameFactory.CreateWinIndicators();
            _gameFactory.CreateCardStack();
            _gameFactory.CreateBlindConfigurator();
            _gameFactory.CreateCombinationComparer();
            ITimer timer = _gameFactory.CreateTimer(_uiFactory.TopPanelView.PlayerTimerView);
            _gameFactory.CreatePlayerStepReporter(timer);
        }

        private void LoadGame()
        {
            CreateUI();
            CreateGameplayComponents();
            _userBalance.SetUser(_gameFactory.User, _uiFactory.TopPanelView.InfoTexts);
            _stateMachine.Enter<PrepareUserBalanceState>();
        }
    }
}