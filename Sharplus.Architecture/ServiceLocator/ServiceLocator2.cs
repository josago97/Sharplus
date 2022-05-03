using System;

namespace Sharplus.Architecture.ServiceLocator
{
    public class ServiceLocator2 : IServiceLocator
    {
        public bool CanResolve(Type type)
        {
            throw new NotImplementedException();
        }

        public bool CanResolve<T>() where T : class
        {
            throw new NotImplementedException();
        }

        public bool IsRegistered(Type type)
        {
            throw new NotImplementedException();
        }

        public bool IsRegistered<T>() where T : class
        {
            throw new NotImplementedException();
        }

        public void Register<TService>() where TService : class
        {
            throw new NotImplementedException();
        }

        public void Register(Type typeService)
        {
            throw new NotImplementedException();
        }

        public void Register<TService>(TService service) where TService : class
        {
            throw new NotImplementedException();
        }

        public void Register<TService, TImplementation>() where TImplementation : class, TService
        {
            throw new NotImplementedException();
        }

        public void Register<TImplementation>(TImplementation service, Type typeService) where TImplementation : class
        {
            throw new NotImplementedException();
        }

        public void Register<TService, TImplementation>(TImplementation service) where TImplementation : class, TService
        {
            throw new NotImplementedException();
        }

        public object Resolve(Type type)
        {
            throw new NotImplementedException();
        }

        public T Resolve<T>() where T : class
        {
            throw new NotImplementedException();
        }
    }
}
