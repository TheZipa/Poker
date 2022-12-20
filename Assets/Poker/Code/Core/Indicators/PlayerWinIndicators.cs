using Poker.Code.Core.Players;

namespace Poker.Code.Core.Indicators
{
    public class PlayerWinIndicators
    {
        private readonly Indicator[] _indicators;

        public PlayerWinIndicators(Indicator[] indicators) => _indicators = indicators;

        public void ShowWinners(Player[] players)
        {
            for(int i = 0; i < players.Length; i++)
                _indicators[i].Show(players[i].IndicatorPosition);
        }

        public void HideAll()
        {
            foreach (Indicator indicator in _indicators)
                indicator.Hide();
        }
    }
}