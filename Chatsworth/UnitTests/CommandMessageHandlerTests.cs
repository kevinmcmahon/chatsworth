using agsXMPP.protocol.client;
using ChatsworthLib;
using ChatsworthLib.Commands;
using ChatsworthLib.MessageHandlers;
using NUnit.Framework;
using Rhino.Mocks;

namespace UnitTests
{
    [TestFixture]
    public class CommandMessageHandlerTests
    {
        CommandMessageHandler handler;
        ICommunicator communicator;
        IMemberDirectory directory;
        MockRepository mocks = new MockRepository();

        [SetUp]
        public void SetUp()
        {
            communicator = mocks.DynamicMock<ICommunicator>();
            directory = mocks.DynamicMock<IMemberDirectory>();
            handler = new CommandMessageHandler(new ICommand[] {new JoinCommand(communicator, directory)});
        }

        [Test]
        public void can_extract_first_word()
        {
            string result = handler.ExtractFirstWord("/this is a test");
            Assert.IsTrue(result == "/this");      
        }

        [Test]
        public void can_handle_null_passed_to_extract_message()
        {
            string result = handler.ExtractFirstWord(null);
            Assert.IsTrue(result == string.Empty);           
        }

        [Test]
        public void can_recognize_commands()
        {
            Message msg = new Message {Body = "/command hello internets"};
            Assert.IsTrue(handler.CanProcess(msg));
        }

        [Test]
        public void can_recognize_not_a_command()
        {
            Message msg = new Message {Body = "command hello internets"};
            Assert.IsFalse(handler.CanProcess(msg));
        }
        
        [Test]
        public void can_recognize_not_a_command_with_null()
        {
            Assert.IsFalse(handler.CanProcess(null));
        }
    }
}