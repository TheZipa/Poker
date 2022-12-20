using System;
using Poker.Code.Infrastructure.ServiceContainer;

namespace Poker.Code.Services.SceneLoader
{
    public interface ISceneLoader : IService
    {
        void LoadScene(string sceneName, Action onLoaded = null);
    }
}