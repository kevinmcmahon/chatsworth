using System;
using agsXMPP.protocol.client;
using Chatsworth.Core.MessageHandlers;

namespace Chatsworth.Core
{
    public class MessageProcessor : IMessageProcessor
    {
        private readonly IMessageHandler[] _messageHandlers;

        public MessageProcessor(IMessageHandler[] handlers)
        {
            _messageHandlers = handlers;
        }

        public void Process(Message message)
        {
            IMessageHandler[] messageHandlers = Array.FindAll(_messageHandlers, h => h.CanProcess(message));
            
            if (messageHandlers.Length > 0)
            {
                foreach (var handler in messageHandlers)
                {
                    handler.ProcessMessage(message);
                }
            }
        }
    }
}