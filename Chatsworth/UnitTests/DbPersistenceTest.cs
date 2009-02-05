using System.Configuration;
using ChatsworthLib.DataAccess;
using ChatsworthLib.Entity;
using NUnit.Framework;

namespace UnitTests
{
    [TestFixture]
    public class DbPersistenceTest
    {
        [Test]
        public void foo()
        {
            var config = new NHibernateSessionConfig("ChatsworthLib","Data Source=chatsworthdb.s3db;Version=3");
            ChatsworthLib.DataAccess.NHibernateSessionManager manager = new NHibernateSessionManager(config);
            var chatsRepos = new ChatMemberRespository(manager);
            var foo = chatsRepos.GetAll();
            Assert.IsNotNull(foo);
        }
    }
}
