using UnityEngine;
using UnityEngine.UI;

namespace Poker.Code.Core.UI
{
    public class Background : MonoBehaviour
    {
        [SerializeField] private Sprite _iPhoneBackground;
        [SerializeField] private Sprite _iPadBackground;
        [SerializeField] private Image _backgroundImage;

        private const int iPadHeight = 1500;

        private void Start() => SetQuality();

        private void SetQuality() =>
            _backgroundImage.sprite = Screen.height >= iPadHeight ? _iPadBackground : _iPhoneBackground;
    }
}