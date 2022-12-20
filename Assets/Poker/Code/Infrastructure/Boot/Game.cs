using Poker.Code.Infrastructure.StateMachine;

namespace Poker.Code.Infrastructure.Boot
{
    public class Game
    {
        public readonly IGameStateMachine GameStateMachine;

        public Game(IGameStateMachine gameStateMachine)
        {
            GameStateMachine = gameStateMachine;
        }
    }
}