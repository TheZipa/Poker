using System.Collections.Generic;
using Poker.Code.Data.Extensions;

namespace Poker.Code.Core.Cards.ModelCardDeck
{
    public class ModelCardDeck : IModelCardDeck
    {
        private readonly Stack<CardModel> _cards;
    
        public ModelCardDeck(Stack<CardModel> cardModels) => _cards = cardModels;

        public void Shuffle() => _cards.Shuffle();

        public CardModel GetCard() => _cards.Pop();

        public void ReturnCards(CardModel[] cards)
        {
            foreach (CardModel card in cards) 
                _cards.Push(card);
        }
    }
}