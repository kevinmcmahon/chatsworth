using System.Collections.Generic;
using Chatsworth.Core.DataAccess;
using Chatsworth.Core.Entity;
using log4net;

namespace Chatsworth.Core
{
    public interface IMemberDirectory
    {
        bool AddSubscriber(string jid);
        bool AddSubscriber(string jid, string alias);
        bool RemoveSubscriber(string jid);
        List<ChatMember> GetToListForSubscriber(string jid);
        ChatMember LookUp(string jid);
        List<ChatMember> GetAllSubscribers();
        ChatMember LookUpByAlias(string alias);
        void AttachRepository(ChatMemberRespository respository);
        ILog Log { get; set; }
    }
}