using agsXMPP.protocol.client;
using Chatsworth.Core;
using Chatsworth.Core.MessageHandlers;
using Machine.Specifications;
using NUnit.Framework;
using Rhino.Mocks;

namespace Chatsworth.UnitTests
{
    public abstract class with_chat_message_handler
    {
        protected static ChatMessageHandler handler;

        protected static ICommunicator mockCommunicator;
        protected static IMemberDirectory mockDirectory;

        Establish context = () =>
            {
                var repository = new MockRepository();
                mockCommunicator = repository.DynamicMock<ICommunicator>();
                mockDirectory = repository.DynamicMock<IMemberDirectory>();

                handler = new ChatMessageHandler(mockCommunicator, mockDirectory);
            };
    }

    [Subject(typeof(ChatMessageHandler))]
    public class when_passed_a_null_message : with_chat_message_handler
    {
        static bool canProcessMessage;

        Because of = () => { canProcessMessage = handler.CanProcess(null); };
        It should_return_false = () => Assert.IsFalse(canProcessMessage);
    }

    [Subject(typeof(ChatMessageHandler))]
    public class when_passed_a_valid_message : with_chat_message_handler
    {
        static bool canProcessMessage;

        Because of = () =>
            {
                Message msg = new Message("foo@goo.com","Test message.");
                canProcessMessage = handler.CanProcess(msg);
            };
        It should_be_able_to_process_message = () => Assert.IsTrue(canProcessMessage);
    }

    [Subject(typeof(ChatMessageHandler))]
    public class when_passed_a_null_message_for_processing : with_chat_message_handler
    {
        Because of = () =>
        {
            handler.ProcessMessage(null);
        };

        It should_be_able_to_process_message;
    }
}