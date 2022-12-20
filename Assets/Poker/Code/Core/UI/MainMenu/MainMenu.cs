using System;
using Poker.Code.Infrastructure.StateMachine;
using Poker.Code.Infrastructure.StateMachine.States;
using Poker.Code.Services.PersistentProgress;
using Poker.Code.Services.SaveLoad;
using Poker.Code.Services.Sound;
using Poker.Code.Services.UserBalance;

namespace Poker.Code.Core.UI.MainMenu
{
    public class MainMenu : IDisposable
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly MainMenuView _menuView;
        private readonly MenuSettingsView _settingsView;
        private readonly IPersistentProgress _playerProgress;
        private readonly ISaveLoad _saveLoadService;
        private readonly IUserBalance _userBalance;
        private readonly ISoundService _soundService;

        public MainMenu(IGameStateMachine stateMachine, MainMenuView menuView, MenuSettingsView settingsView,
            IPersistentProgress playerProgress, ISaveLoad saveLoadService, IUserBalance userBalance, ISoundService soundService)
        {
            _soundService = soundService;
            _saveLoadService = saveLoadService;
            _userBalance = userBalance;
            _playerProgress = playerProgress;
            _stateMachine = stateMachine;
            _settingsView = settingsView;
            _menuView = menuView;
            SubscribeMenuComponents();
        }

        public void Dispose()
        {
            _settingsView.OnBalanceReset -= ResetBalance;
            _settingsView.OnMusicVolumeUpdate -= UpdateMusicVolume;
            _settingsView.OnSoundVolumeUpdate -= UpdateSoundVolume;
            _menuView.OnPlayClick -= EnterGameplayState;
            _menuView.OnSettingsClick -= _settingsView.SwitchActive;
        }

        private void SubscribeMenuComponents()
        {
            _settingsView.OnBalanceReset += ResetBalance;
            _settingsView.OnPlayerCountUpdate += UpdateMaxPlayerCount;
            _settingsView.OnMusicVolumeUpdate += UpdateMusicVolume;
            _settingsView.OnSoundVolumeUpdate += UpdateSoundVolume;
            _menuView.OnPlayClick += EnterGameplayState;
            _menuView.OnSettingsClick += _settingsView.SwitchActive;
        }

        private void UpdateMaxPlayerCount(int count)
        {
            _playerProgress.Progress.Settings.AIPlayersCount = count;
            _saveLoadService.SaveProgress();
        }

        private void EnterGameplayState() => _stateMachine.Enter<LoadGameplayState>();
        

        private void UpdateSoundVolume(float newVolume)
        {
            _soundService.SetEffectsVolume(newVolume);
            _playerProgress.Progress.Settings.SoundVolume = newVolume;
            _saveLoadService.SaveProgress();
        }

        private void UpdateMusicVolume(float newVolume)
        {
            _soundService.SetBackgroundVolume(newVolume);
            _playerProgress.Progress.Settings.MusicVolume = newVolume;
            _saveLoadService.SaveProgress();
        }

        private void ResetBalance()
        {
            _userBalance.ResetUserBalance();
            _menuView.SetBalanceText(_playerProgress.Progress.Balance.Chips);
        }
    }
}