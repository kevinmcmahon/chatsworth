using System.Collections.Generic;
using Chatsworth.Core.Entity;

namespace Chatsworth.Core.DataAccess
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