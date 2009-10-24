using System;
using System.Text.RegularExpressions;
using agsXMPP.protocol.client;
using Chatsworth.Core.Entity;

namespace Chatsworth.Core.MessageHandlers
{
    public class LinkLoggerHandler : IMessageHandler
    {
        private readonly Regex _regEx;
        private const string URL_PATTERN = @"((www\.|(http|https|ftp|news|file)+\:\/\/)[&#95;.a-z0-9-]+\.[a-z0-9\/&#95;:@=.+?,##%&~-]*[^.|\'|\# |!|\(|?|,| |>|<|;|\)])";
        private readonly IMemberDirectory _directory;
        private ILinkLogger _linkLogger;

        public LinkLoggerHandler(IMemberDirectory directory, ILinkLogger linkLogger)
        {
            _linkLogger = linkLogger;
            _directory = directory;
            _regEx = new Regex(URL_PATTERN, RegexOptions.IgnoreCase);
        }

        public void ProcessMessage(Message message)
        {
            MatchCollection matchCollection = _regEx.Matches(message.Body);

            ChatMember member = _directory.LookUp(message.From.Bare);   
         
            foreach (var urlMatch in matchCollection)
            {
                LinkLog linkLog = new LinkLog {DateLogged = DateTime.Now, Url = urlMatch.ToString(), Sender = member};
                _linkLogger.SaveLog(linkLog);
            }
        }

        public bool CanProcess(Message message)
        {
            return _regEx.Matches(message.Body).Count > 0;
        }
    }
}
