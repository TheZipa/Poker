using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace Poker.Code.Core.UI.Gameplay.PlayerChoice
{
    public class BetSlider : MonoBehaviour
    {
        public event Action<int> OnBetUpdate;
        
        [SerializeField] private Slider _betSlider;
        [SerializeField] private Text _betCountText;

        private const int BetSliderStep = 5;
        private int _numberOfSteps;
        
        private void Awake() => _betSlider.onValueChanged.AddListener(UpdateBet);

        private void OnDestroy() => _betSlider.onValueChanged.RemoveAllListeners();

        public void Prepare(int maxBalance, int minRaise)
        {
            _betSlider.maxValue = maxBalance;
            _betSlider.minValue = minRaise;
            _numberOfSteps = (int)_betSlider.maxValue / BetSliderStep;
            UpdateBet(minRaise);
        }

        public void Show() => gameObject.SetActive(true);

        public void Hide() => gameObject.SetActive(false);

        private void UpdateBet(float newBet)
        {
            float range = (newBet / _betSlider.maxValue) * _numberOfSteps;
            int ceil = Mathf.CeilToInt(range);
            float bet = (ceil * BetSliderStep);
            float totalBet = bet == 0 ? _betSlider.minValue : bet;
            
            _betSlider.value = totalBet;
            UpdateBetText(totalBet);
            OnBetUpdate?.Invoke((int)totalBet);
        }

        private void UpdateBetText(float bet) => 
            _betCountText.text = bet.ToString(CultureInfo.InvariantCulture);
    }
}