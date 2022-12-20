using Poker.Code.Infrastructure.ServiceContainer;
using UnityEngine;

namespace Poker.Code.Services.Providers.AssetProvider
{
    public interface IAssetProvider : IService
    {
        GameObject GetMainMenuPrefab();
        GameObject GetSettingsPanelPrefab();
        GameObject GetCardViewPrefab();
        GameObject GetRootGameplayUIPrefab();
        GameObject GetChoicePanelPrefab();
        GameObject GetTopPanelPrefab();
        GameObject GetToMainMenuPopupPrefab();
        GameObject GetPlayerBetViewPrefab();
        GameObject GetChoiceIndicatorPrefab();
        GameObject GetResetBalancePopupPrefab();
        GameObject GetWinIndicatorPrefab();
    }
}