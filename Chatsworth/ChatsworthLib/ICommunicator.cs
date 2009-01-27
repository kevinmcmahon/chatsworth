namespace ChatsworthLib
{
    public interface ICommunicator
    {
        void SendMessage(string to, string message);
        event OnRequestMessageHandler OnMessage;
        void OpenConnection();
        void Configure(ServerConfiguration configuration);
    }

    public delegate void OnRequestMessageHandler(object sender, OnMessageHandlerArgs args);
}
