using ChatsworthLib.DataAccess;
using ChatsworthLib.Entity;

namespace ChatsworthLib
{
    public interface IMemberDirectory
    {
        bool AddSubscriber(string jid);
        bool AddSubscriber(string jid, string alias);
        bool RemoveSubscriber(string jid);
        ChatMemberCollection GetToListForSubscriber(string jid);
        ChatMember LookUp(string jid);
        ChatMemberCollection GetAllSubscribers();
        ChatMember LookUpByAlias(string alias);
        void AttachRepository(ChatMemberRespository respository);
    }
}