using System.Linq;
using Poker.Code.Core.Cards.CardStack;
using Poker.Code.Core.Players;
using Poker.Code.Core.Cards;
using Poker.Code.Data.Extensions;
using HoldemHand;

namespace Poker.Code.Core.Combinations
{
    public class HoldemHandComparer : ICombinationComparer
    {
        private readonly ICardStack _cardStack;

        public HoldemHandComparer(ICardStack cardStack) => _cardStack = cardStack;

        public Player[] ComparePlayerCombinations(Player[] players)
        {
            Hand maxCombinationHand = FindMaxCombinationHand(players);
            return players.Where(player => player.CardPair.Hand == maxCombinationHand).ToArray();
        }

        public void EvaluatePlayerCombinations(Player[] players)
        {
            string boardCards = _cardStack.GetShowedCardModels().ToCombinationString();

            foreach (Player player in players)
            {
                CardPair playerPair = player.CardPair;
                playerPair.Hand = new Hand(playerPair.CardModels.ToCombinationString(), boardCards);
            }
        }

        private Hand FindMaxCombinationHand(Player[] players)
        {
            Hand maxCombination = players[0].CardPair.Hand;
            for(int i = 1; i < players.Length; i++)
            {
                if (players[i].CardPair.Hand > maxCombination)
                    maxCombination = players[i].CardPair.Hand;
            }

            return maxCombination;
        }
    }
}