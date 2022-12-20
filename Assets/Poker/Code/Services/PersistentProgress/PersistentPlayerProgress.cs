using Poker.Code.Data.Progress;

namespace Poker.Code.Services.PersistentProgress
{
    public class PersistentPlayerProgress : IPersistentProgress
    {
        public PlayerProgress Progress { get; set; }
    }
}