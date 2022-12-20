using Poker.Code.Core.UI;
using Poker.Code.Core.UI.Gameplay.PlayerChoice;
using Poker.Code.Core.UI.Gameplay.TopPanel;
using Poker.Code.Core.UI.MainMenu;
using Poker.Code.Infrastructure.ServiceContainer;
using UnityEngine;

namespace Poker.Code.Services.Factories.UIFactory
{
    public interface IUIFactory : IService
    {
        MainMenu CreateMainMenu();
        GameObject CreateRootCanvas();
        ChoicePanel CreateChoicePanel(Transform rootUI);
        TopPanelView TopPanelView { get; }
        ChoicePanel ChoicePanel { get; }
        string ActiveScene { get; set; }
        Popup ResetBalancePopup { get; }
        TopPanelView CreateTopPanel(Transform rootUI);
        Popup CreateResetBalancePopup(Transform rootUI);
    }
}