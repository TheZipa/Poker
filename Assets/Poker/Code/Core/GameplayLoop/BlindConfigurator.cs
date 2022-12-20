using Poker.Code.Core.Players;
using Poker.Code.Data.Enums;
using UnityEngine;

namespace Poker.Code.Core.GameplayLoop
{
    public class BlindConfigurator
    {
        private readonly int _smallBlind;
        private int _startIndex;

        public BlindConfigurator(int smallBlind) => _smallBlind = smallBlind;

        public int SetBlindForEachPlayer(Player[] players)
        {
            int playersCount = players.Length;
            int bigBlindIndex = GetBigBlindIndex(playersCount - 1);
            int smallBlindIndex = 0;
            for (int i = 0; i < playersCount; i++)
            {
                if (i == _startIndex)
                {
                    players[i].Blind = Blind.SmallBlind;
                    smallBlindIndex = i;
                }
                else if (i == bigBlindIndex)
                {
                    players[i].Blind = Blind.BigBlind;
                }
                else
                {
                    players[i].Blind = Blind.Dealer;
                }
            }

            UpdateStartIndex(playersCount);
            return smallBlindIndex;
        }

        public void SetBlindBetForEachPlayer(Player[] players)
        {
            foreach (Player player in players)
            {
                player.SetBet(_smallBlind * (int) player.Blind);
                player.MakeBet(player.Bet);
            }
        }

        private void UpdateStartIndex(int maxPlayers)
        {
            if (++_startIndex == maxPlayers) 
                _startIndex = 0;
        }

        private int GetBigBlindIndex(int lastPlayerIndex)
        {
            int previousIndex = _startIndex - 1;
            if (previousIndex < 0) previousIndex = lastPlayerIndex;
            return previousIndex;
        }
    }
}