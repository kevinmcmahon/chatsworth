namespace ChatsworthLib
{
    public interface ICommunicator
    {
        void SendMessage(string to, string message);
        event OnRequestMessageHandler OnRequestMessage;
        void OpenConnection();
    }

    public delegate void OnRequestMessageHandler(object sender, RequestMessageHandlerArgs args);
}
