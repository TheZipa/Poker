using System;
using System.Collections.Generic;

namespace Poker.Code.Services.Dispose
{
    public class DisposeService : IDisposeService
    {
        private readonly Dictionary<string, List<IDisposable>> _disposables = 
            new Dictionary<string, List<IDisposable>>(2);

        public void RegisterDisposable(IDisposable disposable, string scene)
        {
            bool isSceneExists = _disposables.TryGetValue(scene, out List<IDisposable> registered);

            if (isSceneExists)
            {
                registered.Add(disposable);
            }
            else
            {
                List<IDisposable> newRegistered = _disposables[scene] = new List<IDisposable>(10);
                newRegistered.Add(disposable);
            }
        }

        public void DisposeAll(string scene)
        {
            if (_disposables.TryGetValue(scene, out List<IDisposable> registered))
            {
                foreach (IDisposable disposable in registered)
                    disposable.Dispose();

                registered.Clear();
            }
        }
    }
}