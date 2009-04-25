using Chatsworth.Core.Commands;
using Machine.Specifications;
using NUnit.Framework;
using Rhino.Mocks;

namespace Chatsworth.UnitTests.CommandTests
{
    public abstract class with_leave_command : with_test_message
    {
        protected static LeaveCommand leaveCommand;

        Establish context = () => { leaveCommand = new LeaveCommand(mockCommunicator, mockDirectory); };
    }

    public class when_calling_CommandName_property : with_leave_command
    {
        It should_return_leave_in_lower_case = () => Assert.IsTrue(leaveCommand.CommandName.Equals("leave"));
    }

    public class when_executing_and_successfully_removed_from_member_directory : with_leave_command
    {
        Because of = () =>
            {
                mockDirectory.Stub(x => x.RemoveSubscriber(testMessage.From.Bare)).Return(true);
                leaveCommand.Execute(testMessage);
            };

        It should_not_return_removed_confirmation_message =
            () => mockCommunicator.AssertWasCalled(x => x.SendMessage(testMessage.From.Bare, LeaveCommand.REMOVED_CONFIRMATION_MESSAGE));
    }

    public class when_executing_and_unsuccessfully_removed_from_member_directory : with_leave_command
    {
        Because of = () =>
            {
                mockDirectory.Stub(x => x.RemoveSubscriber(testMessage.From.Bare)).Return(false);
                leaveCommand.Execute(testMessage);
            };

        It should_return_removed_confirmation_message =
            () => mockCommunicator.AssertWasNotCalled(x => x.SendMessage(testMessage.From.Bare, LeaveCommand.REMOVED_CONFIRMATION_MESSAGE));
    }
}