using System.Text.RegularExpressions;
using agsXMPP.protocol.client;
using ChatsworthLib.Entity;

namespace ChatsworthLib.Commands
{
    public class WhisperCommand : ICommand
    {
        private readonly ICommunicator _communicator;
        private readonly IMemberDirectory _directory;
        private readonly Regex regex = new Regex(
       "/(?<Command>\\w+)\\s(?<Alias>\\w+)\\s(?<Message>.*)",
         RegexOptions.IgnoreCase
         | RegexOptions.CultureInvariant
         | RegexOptions.IgnorePatternWhitespace
         | RegexOptions.Compiled
         );

        public WhisperCommand(ICommunicator communicator, IMemberDirectory directory)
        {
            _communicator = communicator;
            _directory = directory;
        }

        public void Execute(Message message)
        {
            WhisperMessageData data = ProcessWhisper(message);
            _communicator.SendMessage(data.ToJid, data.Message);
        }

        private string GetAlias(string jid)
        {
            ChatMember member = _directory.ChatSubscribers.Find(jid);
            return member.Alias;
        }

        private WhisperMessageData ProcessWhisper(Message message)
        {
            string fromAlias = GetAlias(message.From.Bare);
            
            Match match = regex.Match(message.Body);

            string toJid = GetRecipient(match.Groups["Alias"].ToString());
            string rawMessage = match.Groups["Message"].ToString();

            var data = new WhisperMessageData
                           {
                               ToJid = toJid,
                               Message = FormatMessage(fromAlias,rawMessage)
                           };
            return data;
        }

        private string GetRecipient(string alias)
        {
            ChatMember member = _directory.ChatSubscribers.FindByAlias(alias);
            return member.Jid;
        }

        private string FormatMessage(string from, string msg)
        {
            return string.Format("[\"{0}\" to just you] {1}", from, msg.Trim());
        }

        public string CommandName
        {
            get { return "whisper"; }
        }
    }

    public class WhisperMessageData
    {
        public WhisperMessageData() {}

        public WhisperMessageData(string toJid, string message)
        {
            ToJid = toJid;
            Message = message;
        }

        public string ToJid { get; set; }
        public string Message { get; set; }
    }
}