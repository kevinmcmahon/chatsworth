using FluentNHibernate.Mapping;

namespace Chatsworth.Core.Entity
{
    public class ChatMemberMap : ClassMap<ChatMember>
    {
        public ChatMemberMap()
        {
            Table("ChatMember");

            Id(x => x.Id).GeneratedBy.Identity();

            Map(x => x.Jid)
                .Not.Nullable();

            Map(x => x.Alias)
                .Not.Nullable();

            Map(x => x.ActiveInChat)
                .Not.Nullable();
        }
    }
}