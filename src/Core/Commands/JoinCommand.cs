using agsXMPP.protocol.client;
using Chatsworth.Core.Entity;

namespace Chatsworth.Core.Commands
{
    public class JoinCommand : ICommand
    {
        public string JOINED_MESSAGE = "You've joined the chat.";
        public string ALREADY_JOINED_MESSAGE = "You've already joined the chat.";
        
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
                response = JOINED_MESSAGE;
            }
            else
            {
                response = ALREADY_JOINED_MESSAGE;
            }
            _communicator.SendMessage(message.From.Bare,response);
        }

        public string CommandName
        {
            get { return "join"; }
        }
    }
}