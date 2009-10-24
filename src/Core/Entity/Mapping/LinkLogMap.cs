using FluentNHibernate.Mapping;

namespace Chatsworth.Core.Entity.Mapping
{
    public class LinkLogMap : ClassMap<LinkLog>
    {
        public LinkLogMap()
        {
            Table("LinkLog");

            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.DateLogged)
                .Not.Nullable();
            Map(x => x.Url)
                .Not.Nullable();

            References(x => x.Sender)
                .Column("ChatMemberId")
                .Cascade.All()
                .Fetch.Join();
        }
    }
}