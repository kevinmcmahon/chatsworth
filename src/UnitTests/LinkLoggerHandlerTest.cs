using agsXMPP.protocol.client;
using Chatsworth.Core;
using Chatsworth.Core.MessageHandlers;
using NUnit.Framework;

namespace Chatsworth.UnitTests
{
    [TestFixture]
    public class LinkLoggerHandlerTest
    {
        [Test]
        public void Foo()
        {
            LinkLoggerHandler handler = new LinkLoggerHandler(new MemberDirectory(), new LinkLogger());

            Assert.IsTrue(handler.CanProcess(new Message("foo@jid.com", "the is a www.google.com match")));
            Assert.IsTrue(handler.CanProcess(new Message("foo@jid.com", "the is a http://google.com match")));
            Message multiUrlMessage = new Message("foo@jid.com",
                                                  "the is a http://google.com www.cnn.com https://nyt.com match");
            Assert.IsTrue(handler.CanProcess(multiUrlMessage));
            Assert.IsFalse(handler.CanProcess(new Message("foo@jid.com", "this is not a processable message"))); 

            handler.ProcessMessage(multiUrlMessage);
        }
    }
}
