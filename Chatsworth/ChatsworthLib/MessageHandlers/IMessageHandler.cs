using agsXMPP.protocol.client;

namespace ChatsworthLib.MessageHandlers
{
    public interface IMessageHandler
    {
        void ProcessMessage(Message message);
        bool CanProcess(Message message);
    }
}