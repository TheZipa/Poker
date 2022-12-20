using Poker.Code.Core.Players;

namespace Poker.Code.Core.Combinations
{
    public interface ICombinationComparer
    {
        Player[] ComparePlayerCombinations(Player[] players);
        void EvaluatePlayerCombinations(Player[] players);
    }
}