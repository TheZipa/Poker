using System;
using Poker.Code.Data.Enums;
using Poker.Code.Services.Sound;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Poker.Code.Core.UI.MainMenu
{
    public class MenuSettingsView : MonoBehaviour
    {
        public event Action<int> OnPlayerCountUpdate;
        public event Action<float> OnSoundVolumeUpdate;
        public event Action<float> OnMusicVolumeUpdate;
        public event Action OnBalanceReset;

        [SerializeField] private Slider _playerCountSlider;
        [SerializeField] private Slider _soundSlider;
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private TextMeshProUGUI _playerCountText;
        [SerializeField] private Button _resetBalanceButton;
        [SerializeField] private Popup _resetBalancePopup;
        private ISoundService _soundService;

        private void Awake()
        {
            _playerCountSlider.onValueChanged.AddListener(UpdatePlayerCountText);
            _playerCountSlider.onValueChanged.AddListener(SendPlayerCountUpdate);
            _soundSlider.onValueChanged.AddListener(SendSoundVolumeUpdate);
            _musicSlider.onValueChanged.AddListener(SendMusicVolumeUpdate);
            _resetBalanceButton.onClick.AddListener(OnBalanceResetClick);
            _resetBalancePopup.OnAccept += SendBalanceReset;
        }

        public void Construct(ISoundService soundService, int playerCount, int maxPlayerCount,
            float soundVolume, float musicVolume)
        {
            _soundService = soundService;
            _playerCountText.text = playerCount.ToString();
            _playerCountSlider.maxValue = maxPlayerCount;
            _playerCountSlider.value = playerCount;
            _soundSlider.value = soundVolume;
            _musicSlider.value = musicVolume;
            _resetBalancePopup.Construct(_soundService);
        }

        public void SwitchActive() => gameObject.SetActive(!gameObject.activeSelf);

        private void UpdatePlayerCountText(float count) =>
            _playerCountText.text = count.ToString();
        
        private void SendPlayerCountUpdate(float count) =>
            OnPlayerCountUpdate?.Invoke(Convert.ToInt32(count));

        private void SendSoundVolumeUpdate(float volume) =>
            OnSoundVolumeUpdate?.Invoke(volume);

        private void SendMusicVolumeUpdate(float volume) =>
            OnMusicVolumeUpdate?.Invoke(volume);

        private void OnBalanceResetClick()
        {
            _resetBalancePopup.Show();
            _soundService.PlayEffectSound(SoundId.Click);
        }
        
        private void SendBalanceReset() => OnBalanceReset?.Invoke();

        private void OnDestroy()
        {
            _soundSlider.onValueChanged.RemoveAllListeners();
            _musicSlider.onValueChanged.RemoveAllListeners();
            _resetBalanceButton.onClick.RemoveAllListeners();
            _resetBalancePopup.OnAccept -= SendBalanceReset;
        }
    }
}