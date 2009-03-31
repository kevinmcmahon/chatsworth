using agsXMPP.protocol.client;
using Chatsworth.Core;
using Chatsworth.Core.Commands;
using Chatsworth.Core.MessageHandlers;
using Machine.Specifications;
using NUnit.Framework;
using Rhino.Mocks;

namespace Chatsworth.UnitTests
{
    public abstract class with_command_message_handlers
    {
        protected static CommandMessageHandler handler;
        protected static Message msg;
        protected static bool canProcessMessage;

        Establish context = () =>
            {
                MockRepository mocks = new MockRepository();

                ICommunicator communicator = mocks.DynamicMock<ICommunicator>();
                IMemberDirectory directory = mocks.DynamicMock<IMemberDirectory>();

                handler = new CommandMessageHandler(new ICommand[] {new JoinCommand(communicator, directory)});
            };
    }
 
    [Subject(typeof(CommandMessageHandler))]
    public class when_given_a_message_that_has_command_formatting : with_command_message_handlers
    {
        Because of = () => { msg = new Message {Body = "/command hello internets"}; };
        It should_return_true_when_canprocess_called = () => Assert.IsTrue(handler.CanProcess(msg));
    }

    [Subject(typeof(CommandMessageHandler))]
    public class when_given_a_message_that_does_not_have_command_formatting : with_command_message_handlers
    {
        Because of = () =>
            {
                msg = new Message {Body = "command hello internets"};
                canProcessMessage = handler.CanProcess(msg);
            };

        It should_return_false_when_canprocess_called = () => Assert.IsFalse(canProcessMessage);
    }

    [Subject(typeof(CommandMessageHandler))]
    public class when_passed_null : with_command_message_handlers
    {
        Because of = () => { canProcessMessage = handler.CanProcess(null); };
        It should_return_false_when_canprocess_called =()=> Assert.IsFalse(canProcessMessage);
    }
}