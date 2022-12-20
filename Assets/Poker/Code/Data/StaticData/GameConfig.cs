using UnityEngine;

namespace Poker.Code.Data.StaticData
{
    [CreateAssetMenu(fileName = "Game Config", menuName = "Static Data/Game Config")]
    public class GameConfig : ScriptableObject
    {
        public int MaxAICount;
        public int StartCoins;
        public int SmallBlind;
        public float PlayerStepTime;
        public float AnimationSpeed;
        public float CardStackOffset;
    }
}