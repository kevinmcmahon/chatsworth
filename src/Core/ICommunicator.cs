using Chatsworth.Core.Entity;
using log4net;

namespace Chatsworth.Core
{
    public interface ICommunicator
    {
        void SendMessage(string to, string message);
        event OnRequestMessageHandler OnMessage;
        void OpenConnection();
        void Configure(ServerConfiguration configuration);
        ILog Log { get; set; }
    }

    public delegate void OnRequestMessageHandler(object sender, OnMessageHandlerArgs args);
}
