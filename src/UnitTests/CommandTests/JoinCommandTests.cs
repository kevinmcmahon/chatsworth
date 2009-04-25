using Chatsworth.Core.Commands;
using Chatsworth.Core.Entity;
using Machine.Specifications;
using NUnit.Framework;
using Rhino.Mocks;

namespace Chatsworth.UnitTests
{
    public abstract class with_join_command : with_test_message
    {
        protected static JoinCommand joinCommand;
        Establish context = () => { joinCommand = new JoinCommand(mockCommunicator, mockDirectory); };
    }

    [Subject(typeof (JoinCommand))]
    public class when_asked_for_command_name : with_join_command
    {
        static string commandName = "";

        Because of = () => { commandName = joinCommand.CommandName; };

        It should_return_lower_case_join = () => Assert.IsTrue(commandName.Equals("join"));
    }

    [Subject(typeof (JoinCommand))]
    public class when_execute_called : with_join_command
    {
        Because of = () =>
            {
                mockDirectory.Stub(x => x.LookUp(TEST_FROM_JID)).Return(new ChatMember(TEST_FROM_JID,TEST_ALIAS));
                mockCommunicator.Stub(x => x.SendMessage(TEST_FROM_JID.Bare, "You've already joined the chat."));
            };
        It should_not_call_add_subscriber = () => mockDirectory.AssertWasNotCalled(x=>x.AddSubscriber(null));
    }
    
    [Subject(typeof(JoinCommand))]
    public class when_execute_called_and_member_in_directory : with_join_command
    {
        Because of = () =>
        {
            mockDirectory.Stub(x => x.LookUp(TEST_FROM_JID.Bare)).Return(new ChatMember(TEST_FROM_JID.Bare, TEST_ALIAS));
            joinCommand = new JoinCommand(mockCommunicator, mockDirectory);
            joinCommand.Execute(testMessage);
        };

        It should_not_call_add_subscriber = () => mockDirectory.AssertWasNotCalled(x => x.AddSubscriber(null));
        It should_send_response_message = () => mockCommunicator.AssertWasCalled(x => x.SendMessage(TEST_FROM_JID.Bare, joinCommand.ALREADY_JOINED_MESSAGE));
    }
 
    [Subject(typeof(JoinCommand))]
    public class when_execute_called_and_member_not_in_directory : with_join_command
    {
        Because of = () =>
        {
            mockDirectory.Stub(x => x.LookUp(TEST_TO_JID.Bare)).Return(null);
            joinCommand = new JoinCommand(mockCommunicator, mockDirectory);
            joinCommand.Execute(testMessage);
        };

        It should_call_add_subscriber = () => mockDirectory.AssertWasCalled(x => x.AddSubscriber(TEST_FROM_JID.Bare));
        It should_send_response_message = () => mockCommunicator.AssertWasCalled(x => x.SendMessage(TEST_FROM_JID.Bare, joinCommand.JOINED_MESSAGE));
    }
}