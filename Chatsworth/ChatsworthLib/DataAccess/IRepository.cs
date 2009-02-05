using System.Collections.Generic;

namespace ChatsworthLib.DataAccess
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        void Save(T instance);
        void Delete(T instance);
    }
}