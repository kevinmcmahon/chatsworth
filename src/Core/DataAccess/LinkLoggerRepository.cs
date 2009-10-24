using System.Collections.Generic;
using Chatsworth.Core.Entity;

namespace Chatsworth.Core.DataAccess
{
    public class LinkLoggerRepository : NHibernateRepository<LinkLog>
    {
        public LinkLoggerRepository(NHibernateSessionManager sessionManager) : base(sessionManager)
        {
        }

        public IEnumerable<LinkLog> GetAllLinks()
        {
            return GetAll();
        }
    }
}