using System.Collections.Generic;

namespace Chatsworth.Core.DataAccess
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        void Save(T instance);
        void Delete(T instance);
        void Update(T instance);
    }
}