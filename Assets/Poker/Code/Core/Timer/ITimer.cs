using System;

namespace Poker.Code.Core.Timer
{
    public interface ITimer
    {
        event Action OnElapsed;
        void Start();
        void Stop();
    }
}