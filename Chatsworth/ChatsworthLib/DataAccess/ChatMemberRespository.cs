using System.Collections.Generic;
using ChatsworthLib.Entity;

namespace ChatsworthLib.DataAccess
{
    public class ChatMemberRespository : NHibernateRepository<ChatMember>
    {
        public ChatMemberRespository(NHibernateSessionManager sessionManager) : base(sessionManager) {}

        public IEnumerable<ChatMember> GetChatMembers()
        {
            return this.GetAll();
        }
    }
}