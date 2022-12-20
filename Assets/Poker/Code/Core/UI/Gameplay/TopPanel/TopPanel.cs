using System;
using Poker.Code.Infrastructure.StateMachine;
using Poker.Code.Infrastructure.StateMachine.States;

namespace Poker.Code.Core.UI.Gameplay.TopPanel
{
    public class TopPanel : IDisposable
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly TopPanelView _view;
        private readonly Popup _toHomePopup;

        public TopPanel(IGameStateMachine stateMachine, TopPanelView view, Popup toHomePopup)
        {
            _stateMachine = stateMachine;
            _toHomePopup = toHomePopup;
            _view = view;

            SubscribeComponents();
        }

        public void Dispose() => UnsubscribeComponents();

        private void SubscribeComponents()
        {
            _view.OnHomeButtonClick += _toHomePopup.Show;
            _toHomePopup.OnAccept += SetMainMenuState;
        }

        private void UnsubscribeComponents()
        {
            _view.OnHomeButtonClick -= _toHomePopup.Show;
            _toHomePopup.OnAccept -= SetMainMenuState;
        }

        private void SetMainMenuState() => _stateMachine.Enter<MainMenuState>();
    }
}