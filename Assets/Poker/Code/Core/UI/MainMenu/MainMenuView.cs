using System;
using Poker.Code.Data.Enums;
using Poker.Code.Services.Sound;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Poker.Code.Core.UI.MainMenu
{
    public class MainMenuView : MonoBehaviour
    {
        public event Action OnPlayClick;
        public event Action OnSettingsClick;

        [SerializeField] private Button _playButton;
        [SerializeField] private Button _settingsButton;
        [Space(10)]
        [SerializeField] private TextMeshProUGUI _balanceText;
        [SerializeField] private TextMeshProUGUI _recordText;
        
        private ISoundService _soundService;

        private void Awake()
        {
            _settingsButton.onClick.AddListener(OnSettingsButtonClick);
            _playButton.onClick.AddListener(OnPlayButtonClick);
        }

        public void Construct(ISoundService soundService, int balance, int record)
        {
            _soundService = soundService;
            SetBalanceText(balance);
            SetRecordText(record);
        }
        
        public void SetBalanceText(int balance) => _balanceText.text = balance.ToString();

        public void SetRecordText(int record) => _recordText.text = record.ToString();

        private void OnSettingsButtonClick()
        {
            _soundService.PlayEffectSound(SoundId.Click);
            OnSettingsClick?.Invoke();
        }

        private void OnPlayButtonClick()
        {
            _soundService.PlayEffectSound(SoundId.Click);
            OnPlayClick?.Invoke();
        }

        private void OnDestroy()
        {
            _settingsButton.onClick.RemoveAllListeners();
            _playButton.onClick.RemoveAllListeners();
        }
    }
}
