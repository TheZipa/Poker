using Poker.Code.Core.UI;
using Poker.Code.Core.UI.Gameplay.PlayerChoice;
using Poker.Code.Core.UI.Gameplay.TopPanel;
using Poker.Code.Core.UI.MainMenu;
using Poker.Code.Data.Progress;
using Poker.Code.Infrastructure.StateMachine;
using Poker.Code.Services.Dispose;
using Poker.Code.Services.PersistentProgress;
using Poker.Code.Services.Providers.AssetProvider;
using Poker.Code.Services.SaveLoad;
using Poker.Code.Services.Sound;
using Poker.Code.Services.StaticData;
using Poker.Code.Services.UserBalance;
using UnityEngine;

namespace Poker.Code.Services.Factories.UIFactory
{
    public class UIFactory : IUIFactory
    {
        public string ActiveScene { get; set; }
        public TopPanelView TopPanelView { get; private set; }
        public ChoicePanel ChoicePanel { get; private set; }
        public Popup ResetBalancePopup { get; private set; }
        
        private readonly IAssetProvider _assetProvider;
        private readonly IGameStateMachine _stateMachine;
        private readonly IPersistentProgress _playerProgress;
        private readonly IStaticData _staticData;
        private readonly ISaveLoad _saveLoadService;
        private readonly IDisposeService _disposeService;
        private readonly IUserBalance _userBalance;
        private readonly ISoundService _soundService;

        public UIFactory(IGameStateMachine stateMachine, IAssetProvider assetProvider, IStaticData staticData, 
            IPersistentProgress playerProgress, ISaveLoad saveLoadService, IDisposeService disposeService, 
            IUserBalance userBalance, ISoundService soundService)
        {
            _soundService = soundService;
            _saveLoadService = saveLoadService;
            _disposeService = disposeService;
            _userBalance = userBalance;
            _playerProgress = playerProgress;
            _staticData = staticData;
            _stateMachine = stateMachine;
            _assetProvider = assetProvider;
        }

        public MainMenu CreateMainMenu()
        {
            GameObject mainMenuObject = Object.Instantiate(_assetProvider.GetMainMenuPrefab());
            MenuSettingsView menuSettingsView = CreateMenuSettings(mainMenuObject.transform);
            MainMenuView mainMenuView = CreateMainMenuView(mainMenuObject);
            MainMenu mainMenu = new MainMenu(_stateMachine, mainMenuView, menuSettingsView,
                _playerProgress, _saveLoadService, _userBalance, _soundService);
            _disposeService.RegisterDisposable(mainMenu, ActiveScene);
            return mainMenu;
        }

        public GameObject CreateRootCanvas() =>
            Object.Instantiate(_assetProvider.GetRootGameplayUIPrefab());

        public TopPanelView CreateTopPanel(Transform rootUI)
        {
            GameObject topPanelObject = Object.Instantiate(_assetProvider.GetTopPanelPrefab(), rootUI);
            TopPanelView = topPanelObject.GetComponent<TopPanelView>();
            TopPanelView.InfoTexts.SetBalance(_playerProgress.Progress.Balance.Chips);
            Popup toMainMenuPopup = CreateToMainMenuPopup(rootUI);
            _disposeService.RegisterDisposable(new TopPanel(_stateMachine, TopPanelView, toMainMenuPopup), ActiveScene); 
            return TopPanelView;
        }

        private Popup CreateToMainMenuPopup(Transform rootUI)
        {
            Popup toMainMenuPopup = Object
                .Instantiate(_assetProvider.GetToMainMenuPopupPrefab(), rootUI)
                .GetComponent<Popup>();
            toMainMenuPopup.Construct(_soundService);
            return toMainMenuPopup;
        }

        public ChoicePanel CreateChoicePanel(Transform rootUI)
        {
            GameObject choicePanelObject = Object
                .Instantiate(_assetProvider.GetChoicePanelPrefab(), rootUI);
            
            ChoicePanel = choicePanelObject.GetComponent<ChoicePanel>();
            ChoicePanel.BetSlider.Hide();
            ChoicePanel.ChoiceButtons.Construct(_staticData.UserInputText, _soundService);

            return ChoicePanel;
        }

        public Popup CreateResetBalancePopup(Transform rootUI)
        {
            ResetBalancePopup = Object
                .Instantiate(_assetProvider.GetResetBalancePopupPrefab(), rootUI)
                .GetComponent<Popup>();
            ResetBalancePopup.Construct(_soundService);
            return ResetBalancePopup;
        }

        private MainMenuView CreateMainMenuView(GameObject mainMenuObject)
        {
            MainMenuView mainMenuView = mainMenuObject.GetComponent<MainMenuView>();
            Balance playerBalance = _playerProgress.Progress.Balance;
            mainMenuView.Construct(_soundService, playerBalance.Chips, playerBalance.Record);
            return mainMenuView;
        }

        private MenuSettingsView CreateMenuSettings(Transform mainMenuTransform)
        {
            GameObject settingsPanel = Object.Instantiate(_assetProvider.GetSettingsPanelPrefab(), mainMenuTransform);
            MenuSettingsView menuSettingsView = settingsPanel.GetComponent<MenuSettingsView>();
            Settings playerSettings = _playerProgress.Progress.Settings;
            menuSettingsView.Construct(_soundService, playerSettings.AIPlayersCount,
                _staticData.GameConfig.MaxAICount, playerSettings.SoundVolume,
                playerSettings.MusicVolume);
            return menuSettingsView;
        }
    }
}