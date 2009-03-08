using System;
using agsXMPP.protocol.client;
using Chatsworth.Core.MessageHandlers;

namespace Chatsworth.Core
{
    public class MessageProcessor : IMessageProcessor
    {
        private IMessageHandler[] _messageHandlers;

        public MessageProcessor(IMessageHandler[] handlers)
        {
            _messageHandlers = handlers;
        }

        public void Process(Message message)
        {
            IMessageHandler handler = Array.Find(_messageHandlers, h => h.CanProcess(message));
            handler.ProcessMessage(message);
        }
    }
}