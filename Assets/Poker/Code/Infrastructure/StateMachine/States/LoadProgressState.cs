using Poker.Code.Data.Progress;
using Poker.Code.Services.PersistentProgress;
using Poker.Code.Services.SaveLoad;
using Poker.Code.Services.Sound;
using Poker.Code.Services.StaticData;

namespace Poker.Code.Infrastructure.StateMachine.States
{
    public class LoadProgressState : IState
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly IPersistentProgress _playerProgress;
        private readonly ISaveLoad _saveLoadService;
        private readonly IStaticData _staticDataService;
        private readonly ISoundService _soundService;

        public LoadProgressState(IGameStateMachine stateMachine, IPersistentProgress playerProgress,
            ISaveLoad saveLoadService, IStaticData staticDataService, ISoundService soundService)
        {
            _soundService = soundService;
            _staticDataService = staticDataService;
            _saveLoadService = saveLoadService;
            _playerProgress = playerProgress;
            _stateMachine = stateMachine;
        }
        
        public void Enter()
        {
            LoadProgressOrInitNew();
            InitializeSoundVolume();
            _stateMachine.Enter<MainMenuState>();
        }

        public void Exit()
        {
        }
        
        private void LoadProgressOrInitNew() =>
            _playerProgress.Progress = _saveLoadService.LoadProgress() ?? CreateNewProgress();

        private PlayerProgress CreateNewProgress() => new PlayerProgress(_staticDataService.GameConfig.StartCoins);

        private void InitializeSoundVolume()
        {
            PlayerProgress progress = _playerProgress.Progress;
            _soundService.SetBackgroundVolume(progress.Settings.MusicVolume);
            _soundService.SetEffectsVolume(progress.Settings.SoundVolume);
            _soundService.EnableBackgroundMusic();
        }
    }
}