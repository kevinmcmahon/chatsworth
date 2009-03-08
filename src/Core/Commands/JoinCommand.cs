using agsXMPP.protocol.client;
using Chatsworth.Core.Entity;

namespace Chatsworth.Core.Commands
{
    public class JoinCommand : ICommand
    {
        private readonly ICommunicator _communicator;
        private readonly IMemberDirectory _directory;

        public JoinCommand(ICommunicator communicator, IMemberDirectory directory)
        {
            _communicator = communicator;
            _directory = directory;
        }

        public void Execute(Message message)
        {
            string response = "";
            
            ChatMember from = _directory.LookUp(message.From.Bare);

            if (from == null)
            {
                _directory.AddSubscriber(message.From.Bare);
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