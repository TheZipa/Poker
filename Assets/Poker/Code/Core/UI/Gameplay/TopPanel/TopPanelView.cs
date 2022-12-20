using System;
using UnityEngine;
using UnityEngine.UI;

namespace Poker.Code.Core.UI.Gameplay.TopPanel
{
    public class TopPanelView : MonoBehaviour
    {
        public event Action OnHomeButtonClick;

        public PlayerStepTimerView PlayerTimerView;
        public UserInfoTexts InfoTexts;
        
        [SerializeField] private Button _homeButton;

        private void Awake() =>
            _homeButton.onClick.AddListener(SendHomeButtonClick);

        private void OnDestroy() => _homeButton.onClick.RemoveAllListeners();

        private void SendHomeButtonClick() => OnHomeButtonClick?.Invoke();
    }
}