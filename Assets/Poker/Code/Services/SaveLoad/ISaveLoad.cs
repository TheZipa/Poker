using Poker.Code.Data.Progress;
using Poker.Code.Infrastructure.ServiceContainer;

namespace Poker.Code.Services.SaveLoad
{
    public interface ISaveLoad : IService
    {
        void SaveProgress();
        PlayerProgress LoadProgress();
    }
}