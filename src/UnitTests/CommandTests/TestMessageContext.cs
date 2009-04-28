using agsXMPP;
using agsXMPP.protocol.client;
using Chatsworth.Core;
using Machine.Specifications;
using Rhino.Mocks;

namespace Chatsworth.UnitTests
{
    public abstract class with_test_message
    {
        protected static ICommunicator mockCommunicator;
        protected static IMemberDirectory mockDirectory;
        protected static string TEST_ALIAS = "TestJidAlias";
        protected static Jid TEST_FROM_JID = "from@jid.com";
        protected static string TEST_MESSAGE = "Test message.";
        protected static Jid TEST_TO_JID = "to@jid.com";

        protected static Message testMessage;

        Establish context = () =>
            {
                mockCommunicator = MockRepository.GenerateStub<ICommunicator>();
                mockDirectory = MockRepository.GenerateStub<IMemberDirectory>();
                testMessage = new Message(TEST_TO_JID, TEST_FROM_JID, TEST_MESSAGE);
            };
    }
}