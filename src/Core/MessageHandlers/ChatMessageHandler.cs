using System.Collections.Generic;
using agsXMPP.protocol.client;
using Chatsworth.Core.Entity;

namespace Chatsworth.Core.MessageHandlers
{
    public class ChatMessageHandler : IMessageHandler
    {
        private readonly ICommunicator _communicator;
        private readonly IMemberDirectory _directory;

        public const string NOT_ACTIVE_IN_CHAT_MESSAGE = "You are currently inactive.  Message not sent.";

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

            if (from == null)
                return;

            if (from.ActiveInChat)
            {
                IEnumerable<ChatMember> recipients = _directory.GetToListForSubscriber(message.From.Bare);

                foreach (ChatMember member in recipients)
                {
                    if (member.ActiveInChat)
                    {
                        _communicator.SendMessage(member.Jid, string.Format("[\"{0}\"] {1}", from.Alias, message.Body));
                    }
                }
            }
            else
            {
                _communicator.SendMessage(from.Jid, NOT_ACTIVE_IN_CHAT_MESSAGE);
            }
        }

        public bool CanProcess(Message message)
        {
            return (message == null) ? false : true;
        }
    }
}