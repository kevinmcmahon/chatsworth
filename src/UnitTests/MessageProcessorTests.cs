using agsXMPP.protocol.client;
using Chatsworth.Core;
using Chatsworth.Core.MessageHandlers;
using Machine.Specifications;
using NUnit.Framework;

namespace Chatsworth.UnitTests
{
    public abstract class with_stub_message_handlers
    {
        protected static MessageProcessor messageProcessor;
        protected static StubMessageHandler stubCannotProcessHandler;
        protected static StubMessageHandler stubCanProcessHandler;

        Establish context = () =>
            {
                stubCanProcessHandler = new StubMessageHandler(true);
                stubCannotProcessHandler = new StubMessageHandler(false);
            };
    }

    public class StubMessageHandler : IMessageHandler
    {
        readonly bool _canProcessReturnValue;

        public StubMessageHandler(bool canProcessReturnValue)
        {
            ProcessMessageCalled = false;
            CanProcessCalled = false;
            _canProcessReturnValue = canProcessReturnValue;
        }

        public bool ProcessMessageCalled { get; set; }
        public bool CanProcessCalled { get; set; }

        public void ProcessMessage(Message message)
        {
            ProcessMessageCalled = true;
        }

        public bool CanProcess(Message message)
        {
            CanProcessCalled = true;
            return _canProcessReturnValue;
        }
    }

    [Subject(typeof (MessageProcessor))]
    public class when_given_a_message_handler_that_can_process_messages : with_stub_message_handlers
    {
        Because of = () =>
            {
                messageProcessor = new MessageProcessor(new IMessageHandler[] {stubCanProcessHandler});
                messageProcessor.Process(new Message("test@jid.com"));
            };
        It should_call_can_process_on_the_handler = () => Assert.IsTrue(stubCanProcessHandler.CanProcessCalled);
        It should_call_process_on_the_processor = () => Assert.IsTrue(stubCanProcessHandler.ProcessMessageCalled);
    }

    [Subject(typeof(MessageProcessor))]
    public class when_given_a_message_handler_that_cannot_process_messages : with_stub_message_handlers
    {
        Because of = () =>
        {
            messageProcessor = new MessageProcessor(new IMessageHandler[] { stubCannotProcessHandler });
            messageProcessor.Process(new Message("test@jid.com"));
        };

        It should_call_can_process_on_the_handler = () => Assert.IsTrue(stubCannotProcessHandler.CanProcessCalled);
        It should_not_call_process_on_the_processor = () => Assert.IsFalse(stubCannotProcessHandler.ProcessMessageCalled);
    }

    [Subject(typeof(MessageProcessor))]
    public class when_given_multiple_handlers: with_stub_message_handlers
    {
        Because of = () =>
        {
            messageProcessor = new MessageProcessor(new IMessageHandler[] { stubCannotProcessHandler, stubCanProcessHandler });
            messageProcessor.Process(new Message("test@jid.com"));
        };

        It should_call_can_process_on_the_handler_that_cannot_process = () => Assert.IsTrue(stubCannotProcessHandler.CanProcessCalled);
        It should_call_can_process_on_the_handler_that_can_process = () => Assert.IsTrue(stubCanProcessHandler.CanProcessCalled);
        It should_not_call_process_on_the_cannot_handler = () => Assert.IsFalse(stubCannotProcessHandler.ProcessMessageCalled);
        It should_call_process_on_the_can_handler = () => Assert.IsTrue(stubCanProcessHandler.ProcessMessageCalled);
    }

    [Subject(typeof(MessageProcessor))]
    public class when_given_multiple_handlers_where_first_handler_can_process_message : with_stub_message_handlers
    {
        Because of = () =>
        {
            messageProcessor = new MessageProcessor(new IMessageHandler[] { stubCanProcessHandler, stubCannotProcessHandler });
            messageProcessor.Process(new Message("test@jid.com"));
        };

        It should_call_can_process_on_the_first_handler = () => Assert.IsTrue(stubCanProcessHandler.CanProcessCalled);
        It should_not_call_can_process_on_the_second_handler = () => Assert.IsFalse(stubCannotProcessHandler.CanProcessCalled);
        It should_not_call_process_on_the_second_handler = () => Assert.IsFalse(stubCannotProcessHandler.ProcessMessageCalled);
        It should_call_process_on_the_first_handler = () => Assert.IsTrue(stubCanProcessHandler.ProcessMessageCalled);
    }
}