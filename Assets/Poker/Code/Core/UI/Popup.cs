using System;
using Poker.Code.Data.Enums;
using Poker.Code.Services.Sound;
using UnityEngine;
using UnityEngine.UI;

namespace Poker.Code.Core.UI
{
    public class Popup : MonoBehaviour
    {
        public event Action OnAccept;
        public event Action OnCancel;
        
        [SerializeField] private Button _acceptButton;
        [SerializeField] private Button _cancelButton;
        private ISoundService _soundService;

        private void Awake()
        {
            _acceptButton.onClick.AddListener(OnAcceptClick);
            _acceptButton.onClick.AddListener(Hide);
            _cancelButton.onClick.AddListener(OnCancelClick);
            _cancelButton.onClick.AddListener(Hide);
        }

        private void OnDestroy()
        {
            _acceptButton.onClick.RemoveAllListeners();
            _cancelButton.onClick.RemoveAllListeners();
        }

        public void Construct(ISoundService soundService) =>
            _soundService = soundService;

        public void Show() => gameObject.SetActive(true);

        private void Hide() => gameObject.SetActive(false);

        private void OnAcceptClick()
        {
            _soundService.PlayEffectSound(SoundId.Click);
            OnAccept?.Invoke();
        }

        private void OnCancelClick()
        {
            _soundService.PlayEffectSound(SoundId.Click);
            OnCancel?.Invoke();
        }
    }
}