using Poker.Code.Data.StaticData;
using Poker.Code.Data.StaticData.Locations;
using Poker.Code.Data.StaticData.Sounds;
using Poker.Code.Infrastructure.ServiceContainer;
using GameConfig = Poker.Code.Data.StaticData.GameConfig;

namespace Poker.Code.Services.StaticData
{
    public interface IStaticData : IService
    {
        GameConfig GameConfig { get; }
        LocationData LocationData { get; }
        UserInputText UserInputText { get; }
        SoundData SoundData { get; }
        void LoadStaticData();
    }
}