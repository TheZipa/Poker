using System;
using System.Collections.Generic;
using Poker.Code.Infrastructure.StateMachine.States;
using Poker.Code.Services.CardMove;
using Poker.Code.Services.Factories.GameFactory;
using Poker.Code.Services.Factories.UIFactory;
using Poker.Code.Services.PersistentProgress;
using Poker.Code.Services.SaveLoad;
using Poker.Code.Services.SceneLoader;
using Poker.Code.Services.Sound;
using Poker.Code.Services.StaticData;
using Poker.Code.Services.UserBalance;

namespace Poker.Code.Infrastructure.StateMachine
{
    public class GameStateMachine : IGameStateMachine
    {
        private readonly Dictionary<Type, IExitableState> _states;
        private IExitableState _activeState;

        public GameStateMachine(ServiceContainer.ServiceContainer container, ISoundService soundService,
            ICoroutineRunner coroutineRunner)
        {
            _states = new Dictionary<Type, IExitableState>()
            {
                [typeof(BootstrapState)] = new BootstrapState(this, container, soundService),
                [typeof(LoadProgressState)] = new LoadProgressState(this, 
                    container.Single<IPersistentProgress>(),
                        container.Single<ISaveLoad>(),
                    container.Single<IStaticData>(), soundService),
                [typeof(MainMenuState)] = new MainMenuState(this, 
                    container.Single<ISceneLoader>(), container.Single<IUIFactory>(),
                    container.Single<IUserBalance>()),
                [typeof(LoadGameplayState)] = new LoadGameplayState(this, container.Single<ISceneLoader>(),
                        container.Single<IGameFactory>(), container.Single<IUIFactory>(),
                        container.Single<IUserBalance>()),
                [typeof(PrepareUserBalanceState)] = new PrepareUserBalanceState(this, container.Single<IUIFactory>(),
                        container.Single<IGameFactory>(), container.Single<IUserBalance>()),
                [typeof(CardDispenseState)] = new CardDispenseState(this, coroutineRunner,
                    container.Single<IGameFactory>(),
                    container.Single<ICardMover>(),
                    container.Single<IStaticData>()),
                [typeof(GameplayState)] = new GameplayState(this,
                    container.Single<IUIFactory>(),
                    container.Single<IGameFactory>(),
                    container.Single<ICardMover>()),
                [typeof(FinishGameplayState)] = new FinishGameplayState(this, 
                    container.Single<IGameFactory>(), container.Single<IUIFactory>(),
                    container.Single<IUserBalance>(), container.Single<ICardMover>(), coroutineRunner)
            };
        }

        public void Enter<TState>() where TState : class, IState =>
            ChangeState<TState>().Enter();

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload> =>
            ChangeState<TState>().Enter(payload);

        private TState GetState<TState>() where TState : class, IExitableState
            => _states[typeof(TState)] as TState;

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            _activeState?.Exit();
            TState state = GetState<TState>();
            _activeState = state;
            return state;
        }

        ~GameStateMachine() => _activeState.Exit();
    }
}