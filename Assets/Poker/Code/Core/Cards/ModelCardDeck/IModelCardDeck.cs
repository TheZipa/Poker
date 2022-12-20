namespace Poker.Code.Core.Cards.ModelCardDeck
{
    public interface IModelCardDeck
    {
        void Shuffle();
        CardModel GetCard();
        void ReturnCards(CardModel[] cards);
    }
}