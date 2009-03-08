using agsXMPP.protocol.client;

namespace Chatsworth.Core
{
    public interface IMessageProcessor
    {
        void Process(Message message);
    }
}