using System.Collections.Generic;
using Chatsworth.Core.Entity;
using Chatsworth.Core.MessageHandlers;
using Machine.Specifications;
using NUnit.Framework;
using Rhino.Mocks;

namespace Chatsworth.UnitTests
{
    public abstract class with_chat_message_handler : with_test_message
    {
        protected static ChatMessageHandler handler;

        Establish context = () => { handler = new ChatMessageHandler(mockCommunicator, mockDirectory); };
    }

    [Subject(typeof (ChatMessageHandler))]
    public class when_passed_a_null_message : with_chat_message_handler
    {
        static bool canProcessMessage;

        Because of = () => { canProcessMessage = handler.CanProcess(null); };
        It should_return_false = () => Assert.IsFalse(canProcessMessage);
    }

    [Subject(typeof (ChatMessageHandler))]
    public class when_passed_a_valid_message : with_chat_message_handler
    {
        static bool canProcessMessage;

        It can_process_message_should_return_true = () => Assert.IsTrue(canProcessMessage);

        Because of = () =>
            {
                mockDirectory.Stub(x => x.LookUp(TEST_FROM_JID)).Return(new ChatMember(TEST_FROM_JID.Bare));
                canProcessMessage = handler.CanProcess(testMessage);
            };
    }

    [Subject(typeof (ChatMessageHandler))]
    public class when_passed_a_valid_message_but_sender_not_in_directory : with_chat_message_handler
    {
        static bool canProcessMessage;

        It can_process_message_should_return_false =
            () => mockCommunicator.AssertWasNotCalled(x => x.SendMessage("", ""));

        Because of = () =>
            {
                mockDirectory.Stub(x => x.LookUp(TEST_FROM_JID)).Return(null);
                mockCommunicator.Expect(x => x.SendMessage("", "")).IgnoreArguments();
                handler.ProcessMessage(testMessage);
            };
    }

    [Subject(typeof (ChatMessageHandler))]
    public class when_passed_a_null_message_for_processing : with_chat_message_handler
    {
        Because of = () => handler.ProcessMessage(null);

        It should_do_nothing = () =>
            {
                mockDirectory.AssertWasNotCalled(x => x.LookUp(""));
                mockCommunicator.AssertWasNotCalled(x => x.SendMessage("", ""));
            };
    }

    [Subject(typeof (ChatMessageHandler))]
    public class when_passed_a_valid_message_for_processing_with_active_in_chat_recipients : with_chat_message_handler
    {
        Because of = () =>
            {
                mockDirectory.Stub(x => x.LookUp(TEST_FROM_JID)).Repeat.Twice().Return(new ChatMember(TEST_FROM_JID));
                mockDirectory.Stub(x => x.GetToListForSubscriber(TEST_FROM_JID)).Return(new List<ChatMember>
                                                                                            {
                                                                                                new ChatMember(
                                                                                                    TEST_TO_JID)
                                                                                                    {
                                                                                                        ActiveInChat =
                                                                                                            true
                                                                                                    }
                                                                                            });
                mockCommunicator.Stub(x => x.SendMessage(TEST_TO_JID, "[\"from@jid.com\"] Test message."));
                handler.ProcessMessage(testMessage);
            };

        It should_send_message_to_subscribers = () => mockCommunicator.AssertWasCalled(x => x.SendMessage(
                                                                                                TEST_TO_JID,
                                                                                                "[\"from@jid.com\"] Test message."));
    }

    [Subject(typeof (ChatMessageHandler))]
    public class when_passed_a_valid_message_for_processing_and_recipient_not_active_in_chat : with_chat_message_handler
    {
        Because of = () =>
            {
                mockDirectory.Stub(x => x.LookUp(TEST_FROM_JID)).Repeat.Twice().Return(new ChatMember(TEST_FROM_JID));
                mockDirectory.Stub(x => x.GetToListForSubscriber(TEST_FROM_JID)).Return(new List<ChatMember>
                                                                                            {
                                                                                                new ChatMember(
                                                                                                    TEST_TO_JID)
                                                                                                    {
                                                                                                        ActiveInChat =
                                                                                                            false
                                                                                                    }
                                                                                            });
                mockCommunicator.Expect(x => x.SendMessage("", "")).IgnoreArguments();
                handler.ProcessMessage(testMessage);
            };

        It should_not_send_message_to_subscribers =
            () =>
            mockCommunicator.AssertWasNotCalled(x => x.SendMessage(TEST_TO_JID, "[\"from@jid.com\"] Test message."));
    }
}