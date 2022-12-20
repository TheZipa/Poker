using Poker.Code.Data.StaticData;
using Poker.Code.Data.StaticData.Locations;
using Poker.Code.Data.StaticData.Sounds;
using UnityEngine;
using GameConfig = Poker.Code.Data.StaticData.GameConfig;

namespace Poker.Code.Services.Providers.StaticDataProvider
{
    public class StaticDataProvider : IStaticDataProvider
    {
        private const string GameConfigPath = "StaticData/Game Config";
        private const string LocationDataPath = "StaticData/Location Data";
        private const string UserInputSpritesPath = "StaticData/User Input Sprites";
        private const string SoundDataPath = "StaticData/Sound Data";

        public GameConfig GetGameConfig() => Resources.Load<GameConfig>(GameConfigPath);

        public LocationData GetLocationData() => Resources.Load<LocationData>(LocationDataPath);

        public UserInputText GetUserInputSprites() => Resources.Load<UserInputText>(UserInputSpritesPath);

        public SoundData GetSoundData() => Resources.Load<SoundData>(SoundDataPath);
    }
}