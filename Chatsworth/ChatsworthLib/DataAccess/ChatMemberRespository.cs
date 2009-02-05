using System.Collections.Generic;
using ChatsworthLib.Entity;

namespace ChatsworthLib.DataAccess
{
    public class ChatMemberRespository : NHibernateRepository<ChatMember>
    {
        public ChatMemberRespository(NHibernateSessionManager sessionManager) : base(sessionManager) {}

        public ChatMemberCollection GetChatMembers()
        {
            List<ChatMember> list = (List<ChatMember>) this.GetAll();
            return new ChatMemberCollection(list);
        }
    }
}