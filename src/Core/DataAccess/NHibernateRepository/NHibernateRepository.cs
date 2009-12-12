using System.Collections.Generic;

namespace Chatsworth.Core.DataAccess
{
    /// <summary>
    /// NHibernate repository implementation.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class NHibernateRepository<T> : IRepository<T> where T : class
    {
        private readonly NHibernateSessionManager _sessionManager;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="sessionManager"></param>
        public NHibernateRepository(NHibernateSessionManager sessionManager)
        {
            _sessionManager = sessionManager;
        }

        public NHibernateSessionManager SessionManager
        {
            get { return _sessionManager; }
        }

        /// <summary>
        /// Fetch all of type T
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> GetAll()
        {
            using (var session = SessionManager.GetSession())
            {
                session.Flush();
                return session.CreateCriteria(typeof (T)).List<T>();
            }
        }

        /// <summary>
        /// Saves instance
        /// </summary>
        /// <param name="instance"></param>
        public void Save(T instance)
        {
            using (var session = SessionManager.GetSession())
            using (var trans = session.BeginTransaction())
            {
                session.Save(instance);
                trans.Commit();
            }
        }

        /// <summary>
        /// Updates instance.
        /// </summary>
        /// <param name="instance"></param>
        public void Update(T instance)
        {
            using (var session = SessionManager.GetSession())
            using (var trans = session.BeginTransaction())
            {
                session.Update(instance);
                trans.Commit();
            }
        }

        /// <summary>
        /// Deletes instance.
        /// </summary>
        /// <param name="instance"></param>
        public void Delete(T instance)
        {
            using (var session = SessionManager.GetSession())
            using (var trans = session.BeginTransaction())
            {
                session.Delete(instance);
                trans.Commit();
            }
        }
    }
}