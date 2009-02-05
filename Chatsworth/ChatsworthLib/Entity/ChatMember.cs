using System.Collections;
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

        public int Id { get; set; }
        public string Jid { get; set; }
        public string Alias { get; set; }

        public bool HasAlias { get { return Jid != Alias;} }
    }

    public class ChatMemberCollection : List<ChatMember>
    {
        public ChatMemberCollection(IEnumerable<ChatMember> list)
        {
            base.Clear();
            base.AddRange(list);
        }

        public ChatMemberCollection()
        {
            
        }

        public ChatMember FindByJid(string jid)
        {
            return Find(x => x.Jid == jid);
        }

        public ChatMember FindByAlias(string alias)
        {
            return this.Find(x => x.Alias == alias);
        }
    }
}