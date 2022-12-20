using Poker.Code.Data.StaticData;
using Poker.Code.Data.StaticData.Locations;
using Poker.Code.Data.StaticData.Sounds;
using Poker.Code.Services.Providers.StaticDataProvider;
using GameConfig = Poker.Code.Data.StaticData.GameConfig;

namespace Poker.Code.Services.StaticData
{
    public class StaticData : IStaticData
    {
        public GameConfig GameConfig { get; private set; }
        public LocationData LocationData { get; private set; }
        public UserInputText UserInputText { get; private set; }
        public SoundData SoundData { get; private set; }

        private readonly IStaticDataProvider _staticDataProvider;

        public StaticData(IStaticDataProvider staticDataProvider) =>
            _staticDataProvider = staticDataProvider;

        public void LoadStaticData()
        {
            GameConfig = _staticDataProvider.GetGameConfig();
            LocationData = _staticDataProvider.GetLocationData();
            UserInputText = _staticDataProvider.GetUserInputSprites();
            SoundData = _staticDataProvider.GetSoundData();
        }
    }
}