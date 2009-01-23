using agsXMPP.protocol.client;
using ChatsworthLib.Entity;

namespace ChatsworthLib.Commands
{
    public class JoinCommand : ICommand
    {
        private readonly ICommunicator _communicator;
        private readonly MemberDirectory _directory;

        public JoinCommand(ICommunicator communicator, MemberDirectory directory)
        {
            _communicator = communicator;
            _directory = directory;
        }

        public void Execute(Message message)
        {
            string response = "";
            if (!_directory.ChatSubscribers.Exists(x => x.Jid == message.From.Bare))
            {
                _directory.ChatSubscribers.Add(new ChatMember(message.From.Bare));
                response = "You've joined the chat.";
            }
            else
            {
                response = "You've already joined the chat.";
            }
            _communicator.SendMessage(message.From.Bare,response);
        }

        public string CommandName
        {
            get { return "join"; }
        }
    }
}