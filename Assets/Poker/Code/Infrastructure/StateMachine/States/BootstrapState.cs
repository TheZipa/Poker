using Poker.Code.Services.CardMove;
using Poker.Code.Services.Dispose;
using Poker.Code.Services.Factories.GameFactory;
using Poker.Code.Services.Factories.UIFactory;
using Poker.Code.Services.PersistentProgress;
using Poker.Code.Services.Providers.AssetProvider;
using Poker.Code.Services.Providers.StaticDataProvider;
using Poker.Code.Services.SaveLoad;
using Poker.Code.Services.SceneLoader;
using Poker.Code.Services.Sound;
using Poker.Code.Services.StaticData;
using Poker.Code.Services.UserBalance;
using UnityEngine;

namespace Poker.Code.Infrastructure.StateMachine.States
{
    public class BootstrapState : IState
    {
        private readonly IGameStateMachine _gameStateMachine;

        public BootstrapState(IGameStateMachine gameStateMachine, ServiceContainer.ServiceContainer container,
            ISoundService soundService)
        {
            _gameStateMachine = gameStateMachine;

            RegisterServices(container, soundService);
        }

        public void Enter()
        {
            SetOrientation();
            _gameStateMachine.Enter<LoadProgressState>();
        }

        public void Exit()
        {
        }

        private void RegisterServices(ServiceContainer.ServiceContainer container, ISoundService soundService)
        {
            container.RegisterSingle<IGameStateMachine>(_gameStateMachine);
            container.RegisterSingle<IDisposeService>(new DisposeService());
            container.RegisterSingle<IPersistentProgress>(new PersistentPlayerProgress());
            container.RegisterSingle<IAssetProvider>(new AssetProvider());
            container.RegisterSingle<IStaticDataProvider>(new StaticDataProvider());
            container.RegisterSingle<ISceneLoader>(new SceneLoader(
                container.Single<IDisposeService>()));
            RegisterStaticData(container);
            container.RegisterSingle<ISaveLoad>(new PrefsSaveLoad(
                container.Single<IPersistentProgress>()));
            RegisterSoundService(container, soundService);
            RegisterCardMover(container);
            RegisterUserBalance(container);
            RegisterUIFactory(container);
            RegisterGameFactory(container);
        }

        private void RegisterCardMover(ServiceContainer.ServiceContainer container)
        {
            container.RegisterSingle<ICardMover>(new CardMover(
                container.Single<ISoundService>(),
                container.Single<IStaticData>().GameConfig.AnimationSpeed));
        }

        private void RegisterGameFactory(ServiceContainer.ServiceContainer container)
        {
            container.RegisterSingle<IGameFactory>(new GameFactory(
                container.Single<IDisposeService>(),
                container.Single<IAssetProvider>(),
                container.Single<IStaticData>(),
                container.Single<IPersistentProgress>(),
                container.Single<ISoundService>()));
        }

        private void RegisterUIFactory(ServiceContainer.ServiceContainer container)
        {
            container.RegisterSingle<IUIFactory>(new UIFactory(
                _gameStateMachine,
                container.Single<IAssetProvider>(),
                container.Single<IStaticData>(),
                container.Single<IPersistentProgress>(),
                container.Single<ISaveLoad>(),
                container.Single<IDisposeService>(),
                container.Single<IUserBalance>(),
                container.Single<ISoundService>()));
        }

        private void RegisterUserBalance(ServiceContainer.ServiceContainer container)
        {
            container.RegisterSingle<IUserBalance>(new UserBalance(
                container.Single<IPersistentProgress>(),
                container.Single<ISaveLoad>(), container.Single<IStaticData>().GameConfig.StartCoins));
        }

        private void RegisterStaticData(ServiceContainer.ServiceContainer container)
        {
            IStaticData staticData = new StaticData(container.Single<IStaticDataProvider>());
            staticData.LoadStaticData();
            container.RegisterSingle<IStaticData>(staticData);
        }

        private void RegisterSoundService(ServiceContainer.ServiceContainer container, ISoundService soundService)
        {
            soundService.Construct(container.Single<IStaticData>().SoundData);
            container.RegisterSingle<ISoundService>(soundService);
        }

        private void SetOrientation()
        {
            Screen.orientation = ScreenOrientation.Landscape;
            Screen.orientation = ScreenOrientation.AutoRotation;
        
            Screen.autorotateToPortrait = Screen.autorotateToPortraitUpsideDown = false;
            Screen.autorotateToLandscapeLeft = Screen.autorotateToLandscapeRight = true;
        }
    }
}