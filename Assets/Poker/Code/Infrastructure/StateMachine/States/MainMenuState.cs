using Poker.Code.Services.Factories.UIFactory;
using Poker.Code.Services.SceneLoader;
using Poker.Code.Services.UserBalance;

namespace Poker.Code.Infrastructure.StateMachine.States
{
    public class MainMenuState : IState
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly ISceneLoader _sceneLoader;
        private readonly IUIFactory _uiFactory;
        private readonly IUserBalance _userBalance;
        
        private const string MainMenuSceneName = "MainMenu";

        public MainMenuState(IGameStateMachine stateMachine, ISceneLoader sceneLoader, 
            IUIFactory uiFactory, IUserBalance userBalance)
        {
            _uiFactory = uiFactory;
            _userBalance = userBalance;
            _sceneLoader = sceneLoader;
            _stateMachine = stateMachine;
        }
        
        public void Enter()
        {
            _userBalance.ClearGameplayDependencies();
            _sceneLoader.LoadScene(MainMenuSceneName, CreateMenuUI);
        }

        public void Exit()
        {
        }

        private void CreateMenuUI()
        {
            _uiFactory.ActiveScene = MainMenuSceneName;
            _uiFactory.CreateMainMenu();
        }
    }
}