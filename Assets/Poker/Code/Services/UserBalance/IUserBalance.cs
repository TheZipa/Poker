using Poker.Code.Core.Players;
using Poker.Code.Core.UI.Gameplay.TopPanel;
using Poker.Code.Infrastructure.ServiceContainer;

namespace Poker.Code.Services.UserBalance
{
    public interface IUserBalance : IService
    {
        void SetUser(Player user, UserInfoTexts infoTexts);
        void SaveBalance();
        void ResetUserBalance();
        void ClearGameplayDependencies();
        void AddBalance(int chips);
    }
}