using System.Collections.Generic;
using System.IO;
using Chatsworth.Core.DataAccess;
using Chatsworth.Core.Entity;
using Machine.Specifications;
using NUnit.Framework;

namespace Chatsworth.UnitTests
{
    public abstract class with_xml_repos_settings_and_chat_member_list : with_chat_member_list
    {
        protected static string filePath;

        Establish context = () =>
            {
                filePath = ".\\test_entity.xml";

                chatMemberList = new List<ChatMember>
                                     {new ChatMember("foo@bar.com", "Foo"), new ChatMember("bar@bar.com", "Bar")};
            };
    }

    [Subject(typeof (XmlRepository<ChatMember>), "XmlRepository")]
    public class when_saving_chat_member_list : with_xml_repos_settings_and_chat_member_list
    {
        Because of = () =>
            {
                var p = new XmlRepository<List<ChatMember>>(filePath);
                p.Save(chatMemberList);
            };

        It should_created_the_storage_file = () => Assert.IsTrue(File.Exists(filePath));
    }

    [Subject(typeof (XmlRepository<ChatMember>), "XmlRepository")]
    public class when_attempting_to_retrieve_from_xml_that_does_not_exist : with_xml_repos_settings_and_chat_member_list
    {
        static List<ChatMember> testList;

        Because of = () =>
            {
                var repository = new XmlRepository<List<ChatMember>>(".\\test_empty_file.xml"); 
                testList = (List<ChatMember>) repository.GetAll();
            };

        It should_return_null =()=> Assert.IsNull(testList);
    }
}