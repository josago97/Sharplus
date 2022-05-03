using System;
using System.Collections.Generic;

namespace Sharplus.Architecture.ServiceLocator
{
    public static class ServiceLocator
    {
        private static Dictionary<Type, object> instances = new Dictionary<Type, object>();

        public static void Set<T>(T instance)
        {
            instances[typeof(T)] = instance;
        }

        public static T Get<T>()
        {
            T result;
            TryGet(out result);

            return result;
        }

        public static bool TryGet<T>(out T result)
        {
            result = default;
            bool exists = false;

            if (instances.TryGetValue(typeof(T), out object aux))
            {
                result = (T)aux;
                exists = true;
            }

            return exists;
        }

        public static void Clear()
        {
            instances.Clear();
        }
    }
}
