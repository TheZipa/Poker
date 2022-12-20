using System;
using UnityEngine;

namespace Poker.Code.Core.Timer
{
    public class Timer : ITimer, IDisposable
    {
        public event Action OnElapsed;
        
        private readonly ITimerView _view;
        private readonly float _timeLap;

        private float _currentTime;
        private bool _isActive = false;

        public Timer(ITimerView view, float timeLap)
        {
            _view = view;
            _currentTime = _timeLap = timeLap;
            view.OnUpdate += OnUpdate;
        }

        public void Dispose() => _view.OnUpdate -= OnUpdate;

        public void Start()
        {
            _isActive = true;
            _currentTime = _timeLap;
        }

        public void Stop()
        {
            _isActive = false;
            _view.SetTimerProgress(1f);
        }

        private void OnUpdate()
        {
            if (_isActive == false) return;

            if (_currentTime > 0)
            {
                _currentTime -= Time.deltaTime;
                _view.SetTimerProgress(_currentTime / _timeLap);
            }
            else
            {
                Stop();
                OnElapsed?.Invoke();
            }
        }
    }
}