using System;

namespace Sharplus.Architecture.ServiceLocator
{
    public interface IServiceLocator
    {
        bool CanResolve(Type type);
        bool CanResolve<T>() where T : class;

        bool IsRegistered(Type type);
        bool IsRegistered<T>() where T : class;

        void Register<TService>() where TService : class;
        void Register(Type typeService);
        void Register<TService>(TService service) where TService : class;
        void Register<TService, TImplementation>() where TImplementation : class, TService;
        void Register<TImplementation>(TImplementation service, Type typeService) where TImplementation : class;
        void Register<TService, TImplementation>(TImplementation service) where TImplementation : class, TService;

        object Resolve(Type type);
        T Resolve<T>() where T : class;
    }
}
