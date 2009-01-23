using System.Collections.Generic;

namespace ChatsworthLib.Entity
{
    public class ChatMember
    {
        public ChatMember(){}
        public ChatMember(string jid) : this(jid, "") {}

        public ChatMember(string jid, string alias)
        {
            Jid = jid;
            Alias = string.IsNullOrEmpty(alias) ? jid : alias;
        }

        public string Jid { get; set; }
        public string Alias { get; set; }
        public bool HasAlias { get { return Jid != Alias;} }
    }

    public class ChatMemberCollection : List<ChatMember>
    {
        public ChatMember Find(string jid)
        {
            return this.Find(x => x.Jid == jid);
        }
    }
}