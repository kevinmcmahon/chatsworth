using System;
using System.IO;

namespace ChatsworthLib.DataAccess
{
    public class XmlPersistenceProvider : IPersistenceProvider
    {
        private readonly string _persistencePath;

        public XmlPersistenceProvider(string storagePath)
        {
            _persistencePath = storagePath;
        }

        public T Get<T>()
        {
            if (!Directory.Exists(Path.GetDirectoryName(_persistencePath)))
                Directory.CreateDirectory(Path.GetDirectoryName(_persistencePath));

            if (File.Exists(Path.GetFileName(_persistencePath)))
            {
                string stringContent = File.ReadAllText(_persistencePath);
                if (!String.IsNullOrEmpty(stringContent))
                    return ((T)SerializeHelper.Deserialize(typeof(T), stringContent));
            }
            return default(T);
        }

        public void Save<T>(T obj)
        {
            if (obj != null)
            {
                string serailzedObj = SerializeHelper.Serialize(obj);
                File.WriteAllText(_persistencePath, serailzedObj);
            }
        }
    }
}