using System.Collections.Generic;
using agsXMPP.protocol.client;
using Chatsworth.Core.Entity;

namespace Chatsworth.Core.MessageHandlers
{
    public class ChatMessageHandler : IMessageHandler
    {
        private readonly ICommunicator _communicator;
        private readonly IMemberDirectory _directory;

        public ChatMessageHandler(ICommunicator communicator, IMemberDirectory directory)
        {
            _communicator = communicator;
            _directory = directory;
        }

        public void ProcessMessage(Message message)
        {
            if (!CanProcess(message))
                return;

            ChatMember from = _directory.LookUp(message.From.Bare);

            IEnumerable<ChatMember> recipients = _directory.GetToListForSubscriber(message.From.Bare);

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