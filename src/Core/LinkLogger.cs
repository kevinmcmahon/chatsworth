using System.Collections.Generic;
using Chatsworth.Core.DataAccess;
using Chatsworth.Core.Entity;
using log4net;

namespace Chatsworth.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class LinkLogger : ILinkLogger
    {
        private LinkLoggerRepository _repository;
        private ILog _log = new NullLogger();

        public ILog Log
        {
            get { return _log; }
            set { _log = value; }
        }

        public IEnumerable<LinkLog> GetAllLinkLogs()
        {
            return _repository.GetAllLinks();
        }

        public IEnumerable<LinkLog> GetLinks(int numberOfLinks)
        {
            return _repository.GetLinks(numberOfLinks);
        }

        public void SaveLog(LinkLog linkLog)
        {
            _repository.Save(linkLog);
        }

        public void AttachRepository(LinkLoggerRepository repository)
        {
            _repository = repository;
        }
    }
}