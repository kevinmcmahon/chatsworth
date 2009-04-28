using agsXMPP.protocol.client;
using Chatsworth.Core.Entity;

namespace Chatsworth.Core.Commands
{
    public class MuteCommand : ICommand
    {
        ICommunicator _communicator;
        IMemberDirectory _directory;

        public MuteCommand(ICommunicator communicator, IMemberDirectory directory)
        {
            _communicator = communicator;
            _directory = directory;
        }

        public void Execute(Message message)
        {
            ChatMember member = _directory.LookUp(message.From.Bare);

            if (member == null)
                return;

            member.ActiveInChat = !member.ActiveInChat;

            _directory.UpdateSubscriber(member);

            string chatMessage = member.ActiveInChat ? "Chat is active" : "Chat has been muted.";

            _communicator.SendMessage(member.Jid, chatMessage);
        }

        public string CommandName
        {
            get { return "mute"; }
        }
    }
}
