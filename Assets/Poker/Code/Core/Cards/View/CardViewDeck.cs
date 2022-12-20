using System.Collections.Generic;
using Poker.Code.Data.Enums;
using Poker.Code.Data.StaticData.Locations;

namespace Poker.Code.Core.Cards.View
{
    public class CardViewDeck
    {
        public Location Location { get; }
        private readonly CardMaterialProvider _materialProvider;
        private readonly Stack<CardView> _cards;

        public CardViewDeck(Stack<CardView> cardViews, CardMaterialProvider materialProvider, Location location)
        {
            _cards = cardViews;
            _materialProvider = materialProvider;
            Location = location;
        }

        public CardView GetCard(CardValue value, CardSuit suit)
        {
            CardView cardView = _cards.Pop();
            cardView.SetValueMaterial(_materialProvider.GetMaterial(value, suit));
            cardView.gameObject.SetActive(true);
            return cardView;
        }

        public void ReturnCards(CardView[] cards)
        {
            foreach (CardView card in cards)
            {
                card.gameObject.SetActive(false);
                card.transform.position = Location.Position;
                card.transform.rotation = Location.Rotation;
                _cards.Push(card);
            }
        }
    }
}