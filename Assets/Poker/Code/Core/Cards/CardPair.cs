using HoldemHand;
using Poker.Code.Core.Cards.View;
using Poker.Code.Data.StaticData.Locations;

namespace Poker.Code.Core.Cards
{
    public class CardPair
    {
        public Location LeftCardLocation { get; }
        public Location RightCardLocation { get; }
        public Hand Hand { get; set; }
        
        public CardModel[] CardModels;
        public CardView[] CardViews;

        public CardPair(Location leftCardLocation, Location rightCardLocation)
        {
            LeftCardLocation = leftCardLocation;
            RightCardLocation = rightCardLocation;
            Hand = new Hand();
        }

        public void SetCards(CardModel[] models, CardView[] views)
        {
            CardModels = models;
            CardViews = views;
        }
    }
}