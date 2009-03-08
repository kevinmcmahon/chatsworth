using agsXMPP.protocol.client;

namespace Chatsworth.Core.MessageHandlers
{
    public interface IMessageHandler
    {
        void ProcessMessage(Message message);
        bool CanProcess(Message message);
    }
}