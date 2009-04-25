using System.Xml;
using FluentNHibernate;
using FluentNHibernate.Mapping;

namespace Chatsworth.Core.Entity
{
    public class ChatMemberMap : ClassMap<ChatMember>, IMapGenerator
    {
        public ChatMemberMap()
        {
            WithTable("ChatMember");

            Id(x => x.Id).GeneratedBy.Identity();

            Map(x => x.Jid)
                .CanNotBeNull();

            Map(x => x.Alias)
                .CanNotBeNull();

            Map(x => x.ActiveInChat)
                .CanNotBeNull();
        }

        public XmlDocument Generate()
        {
            return CreateMapping(new MappingVisitor());
        }
    }
}
