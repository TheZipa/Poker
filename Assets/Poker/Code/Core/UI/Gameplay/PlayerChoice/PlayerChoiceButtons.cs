using System;
using Poker.Code.Data.Enums;
using Poker.Code.Data.StaticData;
using Poker.Code.Services.Sound;
using TMPro;
using UnityEngine;

namespace Poker.Code.Core.UI.Gameplay.PlayerChoice
{
    public class PlayerChoiceButtons : MonoBehaviour
    {
        public event Action OnRaiseClick;
        public event Action OnCheckCallClick;
        public event Action OnFoldClick;
        public event Action OnAllInClick;

        [SerializeField] private AnimatedInteractButton _raiseButton;
        [SerializeField] private AnimatedInteractButton _checkCallButton;
        [SerializeField] private AnimatedInteractButton _foldButton;
        [SerializeField] private AnimatedInteractButton _allInButton;
        [SerializeField] private TextMeshProUGUI _checkCallText;

        private UserInputText _userInputText;
        private ISoundService _soundService;

        private void Awake()
        {
            _raiseButton.onClick.AddListener(OnRaiseButtonClick);
            _checkCallButton.onClick.AddListener(OnCheckCallButtonClick);
            _foldButton.onClick.AddListener(OnFoldButtonClick);
            _allInButton.onClick.AddListener(OnAllInButtonClick);
        }

        private void OnDestroy()
        {
            _raiseButton.onClick.RemoveAllListeners();
            _checkCallButton.onClick.RemoveAllListeners();
            _foldButton.onClick.RemoveAllListeners();
            _allInButton.onClick.RemoveAllListeners();
        }

        public void Construct(UserInputText userInputText, ISoundService soundService)
        {
            _soundService = soundService;
            _userInputText = userInputText;
        }

        public void SetButtonsActive(bool raise, bool checkCall, bool fold, bool allIn)
        {
            _raiseButton.SetInteractable(raise);
            _checkCallButton.SetInteractable(checkCall);
            _foldButton.SetInteractable(fold);
            _allInButton.SetInteractable(allIn);
        }

        public void SetCallView() => _checkCallText.text = _userInputText.CallText;

        public void SetCheckView() => _checkCallText.text = _userInputText.CheckText;

        private void OnRaiseButtonClick()
        {
            _soundService.PlayEffectSound(SoundId.Click);
            OnRaiseClick?.Invoke();
        }

        private void OnCheckCallButtonClick()
        {
            _soundService.PlayEffectSound(SoundId.Click);
            OnCheckCallClick?.Invoke();
        }

        private void OnFoldButtonClick()
        {
            _soundService.PlayEffectSound(SoundId.Click);
            OnFoldClick?.Invoke();
        }

        private void OnAllInButtonClick()
        {
            _soundService.PlayEffectSound(SoundId.Click);
            OnAllInClick?.Invoke();
        }
    }
}