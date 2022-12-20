using TMPro;
using UnityEngine;

namespace Poker.Code.Core.UI.Gameplay.TopPanel
{
    public class UserInfoTexts : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _balanceText;
        [SerializeField] private TextMeshProUGUI _potText;

        public void SetBalance(int balance) =>
            _balanceText.text = balance.ToString();

        public void SetPot(int pot) =>
            _potText.text = pot.ToString();
    }
}