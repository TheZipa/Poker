using Poker.Code.Services.Factories.GameFactory;
using Poker.Code.Services.Factories.UIFactory;
using Poker.Code.Services.UserBalance;

namespace Poker.Code.Infrastructure.StateMachine.States
{
    public class PrepareUserBalanceState : IState
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly IUIFactory _uiFactory;
        private readonly IGameFactory _gameFactory;
        private readonly IUserBalance _userBalance;

        public PrepareUserBalanceState(IGameStateMachine stateMachine, IUIFactory uiFactory,
            IGameFactory gameFactory, IUserBalance userBalance)
        {
            _stateMachine = stateMachine;
            _uiFactory = uiFactory;
            _gameFactory = gameFactory;
            _userBalance = userBalance;
        }

        public void Enter()
        {
            if (_gameFactory.User.Balance <= 0)
                ResetPlayerBalance();

            _stateMachine.Enter<CardDispenseState>();
        }

        public void Exit()
        {
        }

        private void ResetPlayerBalance()
        {
            _userBalance.ResetUserBalance();
            _uiFactory.ResetBalancePopup.Show();
        }
    }
}