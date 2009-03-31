using System.Collections.Generic;
using Chatsworth.Core.Entity;
using Machine.Specifications;
using NUnit.Framework;

namespace Chatsworth.UnitTests
{
    public abstract class with_chat_member_list
    {
        protected static List<ChatMember> chatMemberList;

        Establish context = () =>
            {
                chatMemberList = new List<ChatMember>
                                     {new ChatMember("foo@bar.com", "Foo"), new ChatMember("bar@bar.com", "Bar")};
            };
    }

    [Subject(typeof(ChatMember), "For a given ChatMember list")]
    public class when_provided_a_valid_jid : with_chat_member_list
    {
        static string jidToSearchBy = "foo@bar.com";
        static ChatMember bar;

        Because of = () => { bar = chatMemberList.FindByJid(jidToSearchBy); };

        It should_return_a_chatmember_object_that_is_not_null =()=> Assert.IsNotNull(bar);
        It should_return_a_chat_member_whose_jid_matches_the_search_by_string = () => Assert.IsTrue(string.Equals(bar.Jid, jidToSearchBy));
    }

    [Subject(typeof(ChatMember))]
    public class when_jid_to_search_by_is_not_in_list : with_chat_member_list
    {
        static ChatMember notThere;
        
        Because of = () => { notThere = chatMemberList.FindByJid("NOTINTHERE@bar.com"); };
        
        It should_return_a_null_chatmember =()=> Assert.IsNull(notThere);
    }

    [Subject(typeof(ChatMember))]
    public class creation_of_chatmember_with_only_a_jid
    {
        static string testJid = "foo@bar.com";

        static ChatMember foo;

        Because of = () => { foo = new ChatMember(testJid); };

        It should_create_a_chat_member_with_jid_set_to_jid = () => Assert.IsTrue(string.Equals(foo.Jid, testJid));

        It should_create_a_chat_member_with_alias_set_to_jid = () => Assert.IsTrue(testJid == foo.Alias);
    }
    
    [Subject(typeof(ChatMember))]
    public class creation_of_chatmember_with_jid_and_alias
    {
        static string testJid = "foo@bar.com";
        static string testAlias = "foo alias";

        static ChatMember foo;

        Because of = () => { foo = new ChatMember(testJid, testAlias); };
        
        It should_create_a_chat_member_with_jid_set_to_jid = () => Assert.IsTrue(string.Equals(foo.Jid, testJid));

        It should_create_a_chat_member_with_alias_set_to_alias = () => Assert.IsTrue(string.Equals(foo.Alias, testAlias));
    }
}