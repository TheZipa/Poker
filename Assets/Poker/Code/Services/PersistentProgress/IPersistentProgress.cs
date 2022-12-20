using Poker.Code.Data.Progress;
using Poker.Code.Infrastructure.ServiceContainer;

namespace Poker.Code.Services.PersistentProgress
{
    public interface IPersistentProgress : IService
    {
        PlayerProgress Progress { get; set; }
    }
}