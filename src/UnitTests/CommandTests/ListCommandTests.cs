using System.Collections.Generic;
using Chatsworth.Core.Commands;
using Chatsworth.Core.Entity;
using Machine.Specifications;
using NUnit.Framework;
using Rhino.Mocks;

namespace Chatsworth.UnitTests.CommandTests
{
    public abstract class with_list_command : with_test_message
    {
        protected static ListCommand listCommand;
        Establish context = () => { listCommand = new ListCommand(mockCommunicator, mockDirectory); };
    }

    [Subject(typeof (ListCommand))]
    public class when_asked_for_list_command_name : with_list_command
    {
        static string commandName = "";
        Because of = () => { commandName = listCommand.CommandName; };
        It should_return_list_in_lower_case = () => Assert.IsTrue(commandName.Equals("list"));
    }

    [Subject(typeof (ListCommand))]
    public class when_execute_called_and_member_not_found_does_not_send_messages : with_list_command
    {
        Because of = () =>
            {
                mockDirectory.Stub(x => x.LookUp(TEST_FROM_JID)).Return(null);
                listCommand.Execute(testMessage);
            };

        It should_not_return_message = () => mockCommunicator.SendMessage("", "");
    }

    [Subject(typeof (ListCommand))]
    public class when_executed : with_list_command
    {
        static readonly List<ChatMember> memberList = new List<ChatMember>
                                                          {
                                                              new ChatMember(TEST_FROM_JID, TEST_ALIAS),
                                                              new ChatMember(TEST_FROM_JID, TEST_ALIAS),
                                                              new ChatMember(TEST_FROM_JID, TEST_ALIAS)
                                                          };

        Because of = () =>
            {
                mockDirectory.Stub(x => x.LookUp(TEST_FROM_JID)).Return(new ChatMember(TEST_FROM_JID, TEST_ALIAS));
                mockDirectory.Stub(x => x.GetAllSubscribers()).Return(memberList);
                mockCommunicator.Expect(x => x.SendMessage("", "")).IgnoreArguments();
                listCommand.Execute(testMessage);
            };

        It should_send_a_message_with_list_of_members = () => { mockCommunicator.VerifyAllExpectations(); };
    }
}