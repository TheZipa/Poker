using System;
using Poker.Code.Infrastructure.ServiceContainer;

namespace Poker.Code.Services.Dispose
{
    public interface IDisposeService : IService
    {
        void RegisterDisposable(IDisposable disposable, string scene);
        void DisposeAll(string scene);
    }
}