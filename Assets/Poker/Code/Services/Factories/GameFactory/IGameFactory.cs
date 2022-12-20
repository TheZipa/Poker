using Poker.Code.Core.Cards.CardStack;
using Poker.Code.Core.Cards.ModelCardDeck;
using Poker.Code.Core.Cards.View;
using Poker.Code.Core.Combinations;
using Poker.Code.Core.GameplayLoop;
using Poker.Code.Core.Indicators;
using Poker.Code.Core.Players;
using Poker.Code.Core.Players.ChoiceInput;
using Poker.Code.Core.Timer;
using Poker.Code.Infrastructure.ServiceContainer;

namespace Poker.Code.Services.Factories.GameFactory
{
    public interface IGameFactory : IService
    {
        IModelCardDeck ModelCardDeck { get; }
        CardViewDeck ViewCardDeck { get; }
        Player[] AllPlayers { get; }
        Player User { get; }
        ICardStack CardStack { get; }
        PlayerStepReporter StepReporter { get; }
        BlindConfigurator BlindConfigurator { get; }
        string ActiveScene { get; set; }
        ICombinationComparer CombinationComparer { get; }
        PlayerWinIndicators WinIndicators { get; }
        IModelCardDeck CreateModelCardDeck();
        CardViewDeck CreateViewCardDeck();
        Player[] CreateAllPlayers(UserInput userInput);
        ICardStack CreateCardStack();
        ITimer CreateTimer(ITimerView timerView);
        PlayerStepReporter CreatePlayerStepReporter(ITimer timer);
        BlindConfigurator CreateBlindConfigurator();
        ICombinationComparer CreateCombinationComparer();
        PlayerWinIndicators CreateWinIndicators();
    }
}