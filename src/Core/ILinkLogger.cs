using System.Collections.Generic;
using Chatsworth.Core.DataAccess;
using Chatsworth.Core.Entity;
using log4net;

namespace Chatsworth.Core
{
    public interface ILinkLogger
    {
        IEnumerable<LinkLog> GetAllLinkLogs();
        void SaveLog(LinkLog log);
        void AttachRepository(LinkLoggerRepository repository);
        ILog Log { get; set; }
    }
}