using System.Configuration;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

namespace Chatsworth
{
    public static class ServiceLocator
    {
        private static readonly IUnityContainer _unityContainer;

        static ServiceLocator()
        {
            _unityContainer = new UnityContainer();

            var section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
            section.Containers.Default.Configure(_unityContainer);
        }

        public static T Retrieve<T>()
        {
            return _unityContainer.Resolve<T>();
        }

        public static T Retrieve<T>(string key)
        {
            return _unityContainer.Resolve<T>(key);
        }

        public static void RegisterInstance<T>(T instance)
        {
            _unityContainer.RegisterInstance(instance);
        }
    }
}