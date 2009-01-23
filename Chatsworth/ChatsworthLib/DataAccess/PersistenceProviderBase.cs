using System;
using System.Collections.Specialized;
using System.Configuration.Provider;

namespace ChatsworthLib.DataAccess
{
    public class PersistenceProviderBase : ProviderBase
    {
        public virtual void Save<T>()
        {
            throw new NotImplementedException();
        }

        public virtual T Get<T>()
        {
            throw new NotImplementedException();
        }

        protected virtual void Setup(string name, string description, NameValueCollection config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config", "Config parameter can not be null. Please check the config file");
            }

            if (!string.IsNullOrEmpty(config["description"]))
                return;
            
            config.Remove("description");
            config.Add("description", description);
        }
    }
}