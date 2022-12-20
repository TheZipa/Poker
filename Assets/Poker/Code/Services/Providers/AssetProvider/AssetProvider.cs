using UnityEngine;

namespace Poker.Code.Services.Providers.AssetProvider
{
    public class AssetProvider : IAssetProvider
    {
        private const string MainMenuPrefabPath = "Prefabs/UI/MainMenu";
        private const string SettingsPanelPrefabPath = "Prefabs/UI/SettingsPanel";
        private const string RootGameplayUIPrefab = "Prefabs/UI/RootGameplayUI";
        private const string ChoicePanelPrefabPath = "Prefabs/UI/ChoicePanel";
        private const string TopPanelPrefabPath = "Prefabs/UI/TopPanel";
        private const string ToMainMenuPopupPrefabPath = "Prefabs/UI/ToMainMenuPopup";
        private const string ResetBalancePopupPrefabPath = "Prefabs/UI/ResetBalancePopup";
        private const string CardViewPrefabPath = "Prefabs/Gameplay/CardView";
        private const string PlayerBetViewPrefabPath = "Prefabs/UI/PlayerBetView";
        private const string ChoiceIndicatorPrefabPath = "Prefabs/Gameplay/ChoiceIndicator";
        private const string WinIndicatorPrefabPath = "Prefabs/Gameplay/WinIndicator";

        public GameObject GetMainMenuPrefab() => Resources.Load<GameObject>(MainMenuPrefabPath);

        public GameObject GetSettingsPanelPrefab() => Resources.Load<GameObject>(SettingsPanelPrefabPath);

        public GameObject GetRootGameplayUIPrefab() => Resources.Load<GameObject>(RootGameplayUIPrefab);

        public GameObject GetChoicePanelPrefab() => Resources.Load<GameObject>(ChoicePanelPrefabPath);

        public GameObject GetTopPanelPrefab() => Resources.Load<GameObject>(TopPanelPrefabPath);
        
        public GameObject GetToMainMenuPopupPrefab() => Resources.Load<GameObject>(ToMainMenuPopupPrefabPath);

        public GameObject GetCardViewPrefab() => Resources.Load<GameObject>(CardViewPrefabPath);

        public GameObject GetPlayerBetViewPrefab() => Resources.Load<GameObject>(PlayerBetViewPrefabPath);
        
        public GameObject GetChoiceIndicatorPrefab() => Resources.Load<GameObject>(ChoiceIndicatorPrefabPath);
        
        public GameObject GetResetBalancePopupPrefab() => Resources.Load<GameObject>(ResetBalancePopupPrefabPath);

        public GameObject GetWinIndicatorPrefab() => Resources.Load<GameObject>(WinIndicatorPrefabPath);
    }
}