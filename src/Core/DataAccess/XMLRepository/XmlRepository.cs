using System;
using System.Collections.Generic;
using System.IO;

namespace Chatsworth.Core.DataAccess
{
    public class XmlRepository<T> : IRepository<T> where T : class
    {
        private readonly string _storagePath;

        public XmlRepository(string storagePath)
        {
            _storagePath = storagePath;
        }

        public IEnumerable<T> GetAll()
        {
            if (!Directory.Exists(Path.GetDirectoryName(_storagePath)))
                Directory.CreateDirectory(Path.GetDirectoryName(_storagePath));

            if (File.Exists(Path.GetFileName(_storagePath)))
            {
                string stringContent = File.ReadAllText(_storagePath);
                if (!String.IsNullOrEmpty(stringContent))
                    return ((T)SerializeHelper.Deserialize(typeof(T), stringContent)) as List<T>;
            }
            return default(List<T>);
        }

        public void Save(T instance)
        {
            if (instance == null)
                return;
            string serailzedObj = SerializeHelper.Serialize(instance);
            File.WriteAllText(_storagePath, serailzedObj);
        }

        public void Delete(T instance)
        {
            return;
        }
    }
}