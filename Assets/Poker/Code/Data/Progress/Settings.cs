using System;

namespace Poker.Code.Data.Progress
{
    [Serializable]
    public class Settings
    {
        public float SoundVolume;
        public float MusicVolume;
        public int AIPlayersCount;

        public Settings()
        {
            SoundVolume = MusicVolume = 0.5f;
            AIPlayersCount = 4;
        }
    }
}