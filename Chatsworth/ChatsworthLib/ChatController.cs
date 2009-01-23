namespace ChatsworthLib
{
    public class ChatController
    {
        private readonly IMessageProcessor _messageProcessor;
        private readonly ICommunicator _communicator;

        public ChatController(IMessageProcessor processor, ICommunicator communicator)
        {
            _messageProcessor = processor;
            _communicator = communicator;
        }

        public void Start()
        {
            _communicator.OnRequestMessage += xmpp_OnRequestMessage;
            _communicator.OpenConnection();
        }

        public void Stop()
        {
            _communicator.OnRequestMessage += xmpp_OnRequestMessage;
        }

        public void SendMessage(string jid, string message)
        {
            _communicator.SendMessage(jid, message);
        }

        private void xmpp_OnRequestMessage(object sender, RequestMessageHandlerArgs args)
        {
            if (_messageProcessor != null)
            {
                _messageProcessor.Process(args.Message);
            }
        }
    }
}