namespace Poker.Code.Infrastructure.ServiceContainer
{
    public class ServiceContainer
    {
        private static ServiceContainer _instance;

        public static ServiceContainer Container => _instance ?? new ServiceContainer();

        public void RegisterSingle<TService>(TService implementation) where TService : IService
        {
            Implementation<TService>.ImplementationInstance = implementation;
        }

        public TService Single<TService>() where TService : IService
        {
            return Implementation<TService>.ImplementationInstance;
        }

        private class Implementation<TService> where TService : IService
        {
            public static TService ImplementationInstance;
        }
    }
}