using Poker.Code.Core.Cards.View;

namespace Poker.Code.Core.Cards.CardStack
{
    public interface ICardStack
    {
        CardModel[] BoardModelCards { get; }
        CardView[] BoardViewCards { get; }
        void SetStack(CardModel[] models, CardView[] views);
        CardModel[] GetShowedCardModels();
        CardView[] GetCardForShow(int count);
    }
}