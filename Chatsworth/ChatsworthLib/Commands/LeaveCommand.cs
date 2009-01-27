using agsXMPP.protocol.client;

namespace ChatsworthLib.Commands
{
    public class LeaveCommand : ICommand
    {
        private readonly ICommunicator _communicator;
        private readonly IMemberDirectory _directory;

        public LeaveCommand(ICommunicator communicator, IMemberDirectory directory)
        {
            _communicator = communicator;
            _directory = directory;
        }

        public void Execute(Message message)
        {
            int count = _directory.ChatSubscribers.RemoveAll(x => x.Jid == message.From.Bare);
            if (count > 0)
            {
                _communicator.SendMessage(message.From.Bare,"You have left the chat.");
            }
        }

        public string CommandName
        {
            get { return "leave"; }
        }
    }
}