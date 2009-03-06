using System.Collections.Generic;
using ChatsworthLib.DataAccess;
using ChatsworthLib.Entity;
using log4net;

namespace ChatsworthLib
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