using System.Collections.Generic;
using System.Text;
using agsXMPP.protocol.client;
using Chatsworth.Core.Entity;

namespace Chatsworth.Core.Commands
{
    /// <summary>
    /// Handles links 
    /// </summary>
    public class LinksCommand : ICommand
    {
        private readonly ICommunicator _communicator;
        private readonly IMemberDirectory _directory;
        private readonly ILinkLogger _linkLogger;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="communicator"></param>
        /// <param name="directory"></param>
        /// <param name="linkLogger"></param>
        public LinksCommand(ICommunicator communicator, IMemberDirectory directory, ILinkLogger linkLogger)
        {
            _communicator = communicator;
            _directory = directory;
            _linkLogger = linkLogger;
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="message"></param>
        public void Execute(Message message)
        {
            LinksCriteria criteria = BuildLinkCommandParameters(message);

            IEnumerable<LinkLog> linkLogs = RetrieveLinks(criteria);

            var sb = new StringBuilder();

            sb.AppendLine("Links Meeting Criteria: ");

            foreach (LinkLog linkLog in linkLogs)
            {
                sb.AppendLine(string.Format("{0} {1} {2}", linkLog.Url, linkLog.Sender.Alias, linkLog.DateLogged));
            }

            ChatMember member = _directory.LookUp(message.From.Bare);

            _communicator.SendMessage(member.Jid, sb.ToString());
        }

        public string CommandName
        {
            get { return "links"; }
        }

        private IEnumerable<LinkLog> RetrieveLinks(LinksCriteria criteria)
        {
            return _linkLogger.GetAllLinkLogs();
        }

        private LinksCriteria BuildLinkCommandParameters(Message message)
        {
            return new LinksCriteria();
        }
    }

    public class LinksCriteria
    {
        public int NumberOfLinksRequested { get; set; }
    }
}