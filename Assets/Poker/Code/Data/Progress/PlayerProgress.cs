using System;

namespace Poker.Code.Data.Progress
{
    [Serializable]
    public class PlayerProgress
    {
        public Balance Balance;
        public Settings Settings;

        public PlayerProgress(int startCoins)
        {
            Balance = new Balance(startCoins);
            Settings = new Settings();
        }
    }
}