using System.Xml;
using FluentNHibernate;
using FluentNHibernate.Mapping;

namespace ChatsworthLib.Entity
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
        }

        public XmlDocument Generate()
        {
            return CreateMapping(new MappingVisitor());
        }
    }
}
