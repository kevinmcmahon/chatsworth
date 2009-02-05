using ChatsworthLib.Entity;
using NUnit.Framework;

namespace UnitTests
{
    [TestFixture]
    public class ChatMemberTests
    {
        [Test]
        public void should_be_able_to_find_member()
        {
            ChatMemberCollection member = new ChatMemberCollection
                                          {new ChatMember("foo@bar.com", "Foo"), new ChatMember("bar@bar.com", "Bar")};
            ChatMember bar = member.FindByJid("foo@bar.com");
            Assert.IsNotNull(bar);
            Assert.IsTrue(bar.Jid == "foo@bar.com");
        }

        [Test]
        public void not_in_list_returns_null()
        {
            ChatMemberCollection member = new ChatMemberCollection { new ChatMember("foo@bar.com", "Foo"), new ChatMember("bar@bar.com", "Bar") };
            ChatMember notThere = member.FindByJid("NOTINTHERE@bar.com");
            Assert.IsNull(notThere);
        }

        [Test]
        public void contruction_with_only_jid_has_jid_as_alias()
        {
            string testJid = "foo@bar.com";
            ChatMember foo = new ChatMember(testJid);
            Assert.IsTrue(testJid == foo.Alias);
        }
    }
}