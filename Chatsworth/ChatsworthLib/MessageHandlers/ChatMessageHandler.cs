using System.Collections.Generic;
using agsXMPP.protocol.client;
using ChatsworthLib.Entity;

namespace ChatsworthLib.MessageHandlers
{
    public class ChatMessageHandler : IMessageHandler
    {
        private readonly ICommunicator _communicator;
        private readonly MemberDirectory _directory;

        public ChatMessageHandler(ICommunicator communicator, MemberDirectory directory)
        {
            _communicator = communicator;
            _directory = directory;
        }

        public void ProcessMessage(Message message)
        {
            if (!CanProcess(message))
                return;

            ChatMember from = _directory.ChatSubscribers.Find(message.From.Bare);

            List<ChatMember> recipients = _directory.ChatSubscribers.FindAll(x => x.Jid != from.Jid);

            foreach (ChatMember member in recipients)
            {
                _communicator.SendMessage(member.Jid, string.Format("[\"{0}\"] {1}", from.Alias, message.Body));
            }
        }

        public bool CanProcess(Message message)
        {
            return true;
        }
    }
}