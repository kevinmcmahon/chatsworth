using System.Collections.Generic;

namespace Chatsworth.Core.Entity
{
    public class ChatMember
    {
        public ChatMember(){}
        public ChatMember(string jid) : this(jid, "") {}

        public ChatMember(string jid, string alias)
        {
            Jid = jid;
            Alias = string.IsNullOrEmpty(alias) ? jid : alias;
            ActiveInChat = true;
        }

        public virtual int Id { get; set; }
        public virtual string Jid { get; set; }
        public virtual string Alias { get; set; }

        public virtual bool HasAlias { get { return Jid != Alias; } }

        public virtual bool ActiveInChat { get; set; }
    }

    public static class Extensions
    {
        public static ChatMember FindByJid(this List<ChatMember> source, string jid)
        {
            return source.Find(x => x.Jid == jid);
        }

        public static ChatMember FindByAlias(this List<ChatMember> source, string alias)
        {
            return source.Find(x => x.Alias == alias);
        }
    }
}