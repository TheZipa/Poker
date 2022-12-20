using System;

namespace Poker.Code.Core.Timer
{
    public interface ITimerView
    {
        public event Action OnUpdate; 
        void SetTimerProgress(float progress);
    }
}