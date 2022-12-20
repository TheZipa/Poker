using TMPro;
using UnityEngine;

namespace Poker.Code.Core.Players
{
    public class PlayerBetView : MonoBehaviour
    {
        [SerializeField] private TextMeshPro _betText;
        private const string BetText = "Bet: ";

        public void SetBet(int bet) => _betText.text = BetText + bet;
    }
}