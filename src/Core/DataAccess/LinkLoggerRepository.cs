using System.Collections.Generic;
using Chatsworth.Core.Entity;
using NHibernate;
using NHibernate.Criterion;

namespace Chatsworth.Core.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    public class LinkLoggerRepository : NHibernateRepository<LinkLog>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionManager"></param>
        public LinkLoggerRepository(NHibernateSessionManager sessionManager) : base(sessionManager)
        {
        }

        public IEnumerable<LinkLog> GetAllLinks()
        {
            return GetAll();
        }

        public IEnumerable<LinkLog> GetLinks(int numberOfLinks)
        {
            using (ISession session = SessionManager.GetSession())
            {
                return session.CreateCriteria(typeof (LinkLog))
                    .AddOrder(Order.Desc("Id"))
                    .SetMaxResults(numberOfLinks)
                    .List<LinkLog>();
            }
        }
    }
}