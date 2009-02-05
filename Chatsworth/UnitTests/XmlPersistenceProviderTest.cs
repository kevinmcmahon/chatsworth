using ChatsworthLib.DataAccess;
using ChatsworthLib.Entity;
using NUnit.Framework;

namespace UnitTests
{
    [TestFixture]
    public class XmlPersistenceProviderTest
    {
        private ChatMember fooMember;
        private ChatMember booMember;
        private ChatMemberCollection col;

        [SetUp]
        public void SetUp()
        {
            fooMember = new ChatMember("foo@bar.com", "Foo");
            booMember = new ChatMember("boo@bar.com", "Boo");
            col = new ChatMemberCollection {fooMember, booMember};
        }

        //[Test]
        //public void should_write_single_entity_to_xml_file()
        //{
        //    var p = new XmlPersistenceProvider(".\\test_entity.xml");
        //    p.Save(fooMember);
        //}

        //[Test]
        //public void should_write_collection_entity_to_xml_file()
        //{
        //    var p = new XmlPersistenceProvider(".\\test_collection.xml");
        //    p.Save(col);
        //}

        //[Test]
        //public void rehydrate_single_entity()
        //{
        //    var p = new XmlPersistenceProvider(".\\test_entity.xml");
        //    var test = p.Get<ChatMember>();
        //    Assert.IsTrue(test.Alias == fooMember.Alias);
        //    Assert.IsTrue(test.Jid == fooMember.Jid);
        //}
        
        //[Test]
        //public void rehydrate_entity_collection()
        //{
        //    var p = new XmlPersistenceProvider(".\\test_collection.xml");
        //    var test = p.Get<ChatMemberCollection>();
        //    Assert.IsTrue(test[0].Alias == fooMember.Alias);
        //    Assert.IsTrue(test[0].Jid == fooMember.Jid);
        //    Assert.IsTrue(test[1].Alias == booMember.Alias);
        //    Assert.IsTrue(test[1].Jid == booMember.Jid);
        //}

        //[Test]
        //public void should_not_fail_when_retrieving_empty_collection()
        //{
        //    var p = new XmlPersistenceProvider(".\\test_empty_file.xml");
        //    var test = p.Get<ChatMemberCollection>();
        //    Assert.IsNull(test);
        //}
    }
}