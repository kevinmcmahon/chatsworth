using System.Collections.Generic;
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
            var member = new List<ChatMember>
                                          { new ChatMember("foo@bar.com", "Foo"), new ChatMember("bar@bar.com", "Bar")};
            ChatMember bar = member.FindByJid("foo@bar.com");
            Assert.IsNotNull(bar);
            Assert.IsTrue(bar.Jid == "foo@bar.com");
        }

        [Test]
        public void not_in_list_returns_null()
        {
            List<ChatMember> member = new List<ChatMember> { new ChatMember("foo@bar.com", "Foo"), new ChatMember("bar@bar.com", "Bar") };
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

        [Test]
        public void should_be_able_to_find_member_via_extension_method()
        {
            List<ChatMember> members = new List<ChatMember> { new ChatMember("foo@bar.com", "Foo"), new ChatMember("bar@bar.com", "Bar") };
            ChatMember bar = members.FindByJid("foo@bar.com");
            Assert.IsNotNull(bar);
            Assert.IsTrue(bar.Jid == "foo@bar.com");
        }
    }
}