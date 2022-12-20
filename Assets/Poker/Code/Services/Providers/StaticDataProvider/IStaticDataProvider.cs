using Poker.Code.Data.StaticData;
using Poker.Code.Data.StaticData.Locations;
using Poker.Code.Data.StaticData.Sounds;
using Poker.Code.Infrastructure.ServiceContainer;
using GameConfig = Poker.Code.Data.StaticData.GameConfig;

namespace Poker.Code.Services.Providers.StaticDataProvider
{
    public interface IStaticDataProvider : IService
    {
        GameConfig GetGameConfig();
        LocationData GetLocationData();
        UserInputText GetUserInputSprites();
        SoundData GetSoundData();
    }
}