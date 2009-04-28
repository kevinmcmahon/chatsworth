using System.Collections.Generic;
using agsXMPP.protocol.client;
using Chatsworth.Core.Commands;
using Chatsworth.Core.Entity;
using Machine.Specifications;
using NUnit.Framework;
using Rhino.Mocks;

namespace Chatsworth.UnitTests.CommandTests
{
    public class with_alias_command : with_test_message
    {
        protected static AliasCommand aliasCommand;
        protected static Message aliasEmptyMessage = new Message(TEST_TO_JID, TEST_FROM_JID, "/alias");
        protected static Message aliasMessage = new Message(TEST_TO_JID, TEST_FROM_JID, "/alias " + TEST_ALIAS);
        protected static ChatMember chatMember;

        Establish context = () =>
            {
                aliasCommand = new AliasCommand(mockCommunicator, mockDirectory);
                chatMember = new ChatMember(TEST_FROM_JID);
            };
    }

    [Subject(typeof (AliasCommand))]
    public class when_asked_for_command_name : with_alias_command
    {
        static string commandName = "";

        Because of = () => { commandName = aliasCommand.CommandName; };

        It should_return_lower_case_alias = () => Assert.IsTrue(commandName.Equals("alias"));
    }

    [Subject(typeof (AliasCommand))]
    public class when_execute_called_and_member_is_not_found_in_directory : with_alias_command
    {
        Because of = () =>
            {
                mockDirectory.Stub(x => x.LookUp(TEST_FROM_JID)).Return(null);
                aliasCommand.Execute(testMessage);
            };

        It should_not_send_any_messages_back = () => mockCommunicator.AssertWasNotCalled(x => x.SendMessage("", ""));
    }

    [Subject(typeof (AliasCommand))]
    public class when_execute_called_with_message_that_has_no_alias_specified : with_alias_command
    {
        Because of = () =>
            {
                mockDirectory.Stub(x => x.LookUp(TEST_FROM_JID)).Return(chatMember);
                mockCommunicator.Stub(x => x.SendMessage(chatMember.Jid, AliasCommand.USAGE_MESSAGE));
                aliasCommand.Execute(aliasEmptyMessage);
            };

        It should_not_change_the_chat_member_alias = () => Assert.IsTrue(chatMember.Alias == TEST_FROM_JID);

        It should_send_usage_messages_back =
            () => mockCommunicator.AssertWasCalled(x => x.SendMessage(chatMember.Jid, AliasCommand.USAGE_MESSAGE));
    }

    [Subject(typeof (AliasCommand))]
    public class when_execute_called_with_message_that_has_alias_specified : with_alias_command
    {
        static readonly ChatMember testParticipant = new ChatMember("testparticipant@jid.com");

        Because of = () =>
            {
                mockDirectory.Stub(x => x.LookUp(TEST_FROM_JID)).Return(chatMember);
                mockDirectory.Expect(x => x.UpdateSubscriber(chatMember));
                mockDirectory.Stub(x => x.GetToListForSubscriber(chatMember.Jid)).Return(new List<ChatMember>
                                                                                             {testParticipant});
                mockCommunicator.Stub(x => x.SendMessage(TEST_FROM_JID.Bare, ""));
                aliasCommand.Execute(aliasMessage);
            };

        It should_change_the_chat_member_alias = () => Assert.IsTrue(chatMember.Alias == TEST_ALIAS);

        It should_notify_others_that_alias_has_changed =
            () =>
                {
                    mockDirectory.AssertWasCalled(x => x.GetToListForSubscriber(chatMember.Jid));
                    mockCommunicator.AssertWasCalled(
                        x =>
                        x.SendMessage(testParticipant.Jid,
                                      string.Format("{0} is now known as {1}", TEST_FROM_JID, TEST_ALIAS)));
                };
    }

    [Subject(typeof (AliasCommand))]
    public class when_execute_called_with_message_that_has_alias_specified_but_no_other_participants :
        with_alias_command
    {
        static readonly ChatMember testParticipant = new ChatMember("testparticipant@jid.com");

        Because of = () =>
            {
                mockDirectory.Stub(x => x.LookUp(TEST_FROM_JID)).Return(chatMember);
                mockDirectory.Expect(x => x.UpdateSubscriber(chatMember));
                mockDirectory.Stub(x => x.GetToListForSubscriber(chatMember.Jid)).Return(new List<ChatMember>());
                mockCommunicator.Stub(x => x.SendMessage(TEST_FROM_JID.Bare, ""));
                aliasCommand.Execute(aliasMessage);
            };

        It should_change_the_chat_member_alias = () => Assert.IsTrue(chatMember.Alias == TEST_ALIAS);

        It should_not_try_to_notify_others_that_alias_has_changed =
            () =>
                {
                    mockDirectory.AssertWasCalled(x => x.GetToListForSubscriber(chatMember.Jid));
                    mockCommunicator.AssertWasNotCalled(
                        x =>
                        x.SendMessage(testParticipant.Jid,
                                      string.Format("{0} is now known as {1}", TEST_FROM_JID, TEST_ALIAS)));
                };

        It should_update_the_chat_member_in_data_store = () => mockDirectory.VerifyAllExpectations();
    }
}