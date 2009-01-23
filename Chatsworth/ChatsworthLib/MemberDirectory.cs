using System;
using ChatsworthLib.DataAccess;
using ChatsworthLib.Entity;

namespace ChatsworthLib
{
    public class MemberDirectory : IMemberDirectory, IDisposable
    {
        private readonly IPersistenceProvider _provider;
        private ChatMemberCollection _chatMembers;

        public MemberDirectory(IPersistenceProvider provider)
        {
            _provider = provider;
        }

        public ChatMemberCollection ChatSubscribers
        {
            get
            {
                if (_chatMembers == null)
                {
                    _chatMembers = _provider.Get<ChatMemberCollection>() ?? new ChatMemberCollection();
                }
                return _chatMembers;
            }
            set { _chatMembers = value; }
        }

        public void Dispose()
        {
            _provider.Save(ChatSubscribers);
        }
    }
}