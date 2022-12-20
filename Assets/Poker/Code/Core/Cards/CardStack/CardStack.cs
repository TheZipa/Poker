using System.Linq;
using Poker.Code.Core.Cards.View;

namespace Poker.Code.Core.Cards.CardStack
{
    public class CardStack : ICardStack
    {
        public CardModel[] BoardModelCards { get; private set; }
        public CardView[] BoardViewCards { get; private set; }
        private int _showedCount;

        public void SetStack(CardModel[] models, CardView[] views)
        {
            _showedCount = 0;
            BoardModelCards = models;
            BoardViewCards = views;
        }

        public CardModel[] GetShowedCardModels() => BoardModelCards.Take(_showedCount).ToArray();

        public CardView[] GetCardForShow(int count)
        {
            CardView[] toShowViews = BoardViewCards
                .Skip(_showedCount)
                .Take(count)
                .ToArray();
            _showedCount += count;
            return toShowViews;
        }
    }
}