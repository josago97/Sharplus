using System;
using System.Collections.Generic;
using System.Text;

namespace Sharplus.Architecture.DependencyInjection
{
    public class Container
    {
        private Dictionary<Type, object> _instances;

        public Container()
        {
            _instances = new Dictionary<Type, object>();
        }

        public void Bind<T>(T instance)
        {
            _instances.Add(typeof(T), instance);
        }

        public void Resolve<T>(T instance)
        {

        }

    }
}
