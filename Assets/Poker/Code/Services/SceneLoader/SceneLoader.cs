using System;
using Poker.Code.Services.Dispose;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Poker.Code.Services.SceneLoader
{
    public class SceneLoader : ISceneLoader
    {
        private readonly IDisposeService _disposeService;

        public SceneLoader(IDisposeService disposeService) =>
            _disposeService = disposeService;

        public void LoadScene(string sceneName, Action onLoaded = null)
        {
            _disposeService.DisposeAll(SceneManager.GetActiveScene().name);
            AsyncOperation loadSceneAsyncOperation = SceneManager.LoadSceneAsync(sceneName);
            loadSceneAsyncOperation.completed += operation => onLoaded?.Invoke();
        }
    }
}