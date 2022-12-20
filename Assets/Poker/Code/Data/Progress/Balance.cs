using System;

namespace Poker.Code.Data.Progress
{
    [Serializable]
    public class Balance
    {
        public int Chips;
        public int Record;

        public Balance(int coins) => Chips = Record = coins;
    }
}