using System;
using Poker.Code.Core.Timer;
using UnityEngine;
using UnityEngine.UI;

namespace Poker.Code.Core.UI.Gameplay.TopPanel
{
    public class PlayerStepTimerView : MonoBehaviour, ITimerView
    {
        public event Action OnUpdate;
        [SerializeField] private Image _progressBar;

        public void SetTimerProgress(float progress) =>
            _progressBar.fillAmount = progress;

        private void Update() => OnUpdate?.Invoke();
    }
}