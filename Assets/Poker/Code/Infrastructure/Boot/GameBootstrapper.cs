using Poker.Code.Infrastructure.StateMachine;
using Poker.Code.Infrastructure.StateMachine.States;
using Poker.Code.Services.Sound;
using UnityEngine;

namespace Poker.Code.Infrastructure.Boot
{
    public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
    {
        [SerializeField] private SoundService _soundService;
        private Game _game;

        private void Awake()
        {
            _game = new Game(new GameStateMachine(ServiceContainer.ServiceContainer.Container, _soundService, this));
            
            _game.GameStateMachine.Enter<BootstrapState>();

            DontDestroyOnLoad(this);
        }
    }
}