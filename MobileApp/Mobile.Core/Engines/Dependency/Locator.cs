using System;

namespace Mobile.Core.Engines.Dependency
{
    public static class Locator
    {
        private static IServiceProvider provider;
        public static T GetInstance<T>()
        {
            return (T)provider.GetService(typeof(T));
        }

        public static object GetInstance(Type type)
        {
            return provider.GetService(type);
        }

        public static void Init(IServiceProvider serviceProvider)
        {
            provider = serviceProvider;
        }
    }
}
