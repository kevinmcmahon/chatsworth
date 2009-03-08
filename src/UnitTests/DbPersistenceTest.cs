using Chatsworth.Core.DataAccess;
using NUnit.Framework;

namespace Chatsworth.UnitTests
{
    [TestFixture]
    public class DbPersistenceTest
    {
        [Test]
        public void foo()
        {
            var config = new NHibernateSessionConfig("Chatsworth.Core", "Data Source=chatsworthdb.s3db;Version=3");
            var manager = new NHibernateSessionManager(config);
            var chatsRepos = new ChatMemberRespository(manager);
            var foo = chatsRepos.GetAll();
            Assert.IsNotNull(foo);
        }
    }
}