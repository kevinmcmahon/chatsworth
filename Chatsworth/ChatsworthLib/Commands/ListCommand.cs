using System.Text;
using agsXMPP.protocol.client;
using ChatsworthLib.Entity;

namespace ChatsworthLib.Commands
{
    public class ListCommand : ICommand
    {
        private readonly ICommunicator _communicator;
        private readonly MemberDirectory _directory;

        public ListCommand(ICommunicator communicator, MemberDirectory directory)
        {
            _directory = directory;
            _communicator = communicator;
        }

        public void Execute(Message message)
        {
            ChatMember member = _directory.ChatSubscribers.Find(message.From.Bare);
            if (member == null)
                return;
            var sb = new StringBuilder();
            foreach (var chatMember in _directory.ChatSubscribers)
            {
                sb.AppendLine(string.Format("{0} {1}", chatMember.Jid,
                                            chatMember.HasAlias ? "("+chatMember.Alias+")" : string.Empty));
            }
            _communicator.SendMessage(member.Jid,sb.ToString());
        }

        public string CommandName
        {
            get { return "list"; }
        }
    }
}