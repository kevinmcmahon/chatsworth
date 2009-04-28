using System.Collections.Generic;

namespace Chatsworth.Core.DataAccess
{
    public class NHibernateRepository<T> : IRepository<T> where T : class
    {
        private readonly NHibernateSessionManager _sessionManager;

        public NHibernateRepository(NHibernateSessionManager sessionManager)
        {
            _sessionManager = sessionManager;
        }

        public IEnumerable<T> GetAll()
        {
            using (var session = _sessionManager.GetSession())
            {
                session.Flush();
                return session.CreateCriteria(typeof (T)).List<T>();
            }
        }

        public void Save(T instance)
        {
            using (var session = _sessionManager.GetSession())
            using (var trans = session.BeginTransaction())
            {
                session.Save(instance);
                trans.Commit();
            }
        }

        public void Update(T instance)
        {
            using (var session = _sessionManager.GetSession())
            using (var trans = session.BeginTransaction())
            {
                session.Update(instance);
                trans.Commit();
            }
        }

        public void Delete(T instance)
        {
            using (var session = _sessionManager.GetSession())
            using (var trans = session.BeginTransaction())
            {
                session.Delete(instance);
                trans.Commit();
            }
        }
    }
}