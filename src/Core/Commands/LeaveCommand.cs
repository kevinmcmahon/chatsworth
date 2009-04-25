using agsXMPP.protocol.client;

namespace Chatsworth.Core.Commands
{
    public class LeaveCommand : ICommand
    {
        private readonly ICommunicator _communicator;
        private readonly IMemberDirectory _directory;
        public const string REMOVED_CONFIRMATION_MESSAGE = "You have left the chat.";

        public LeaveCommand(ICommunicator communicator, IMemberDirectory directory)
        {
            _communicator = communicator;
            _directory = directory;
        }

        public void Execute(Message message)
        {
            if(_directory.RemoveSubscriber(message.From.Bare))
            {
                _communicator.SendMessage(message.From.Bare,REMOVED_CONFIRMATION_MESSAGE);
            }
        }

        public string CommandName
        {
            get { return "leave"; }
        }
    }
}