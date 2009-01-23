using agsXMPP.protocol.client;

namespace ChatsworthLib
{
    public interface IMessageProcessor
    {
        void Process(Message message);
    }
}