using agsXMPP;
using agsXMPP.net;
using agsXMPP.protocol.client;
using agsXMPP.protocol.extensions.chatstates;
using log4net;

namespace ChatsworthLib
{
    public class XMPPCommunicator : ICommunicator
    {
        private static readonly ILog log = LogManager.GetLogger("ChatsworthLib.Logger");
        private readonly XmppClientConnection xmpp;

        public XMPPCommunicator(ServerInfo connectionInfo)
        {
            xmpp = new XmppClientConnection
                       {
                           Server = connectionInfo.Server,
                           ConnectServer = connectionInfo.ConnectServer,
                           Username = connectionInfo.Username,
                           Password = connectionInfo.Password,
                           KeepAlive = true,
                           UseStartTLS = true,
                           SocketConnectionType = SocketConnectionType.Direct
                       };
            xmpp.OnMessage += xmpp_OnMessage;
            xmpp.OnReadXml += xmpp_OnReadXml;
            xmpp.OnWriteXml += xmpp_OnWriteXml;
            xmpp.OnLogin += xmpp_OnLogin;
            xmpp.OnAuthError += xmpp_OnAuthError;
        }

        public void OpenConnection()
        {
            if (log.IsDebugEnabled)
                log.Debug("Attempting to open connection.");
            xmpp.Open();
        }

        private void xmpp_OnAuthError(object sender, agsXMPP.Xml.Dom.Element e)
        {
            if (log.IsDebugEnabled)
                log.Debug("Authorization Error");
        }

        private void xmpp_OnLogin(object sender)
        {
            if (log.IsDebugEnabled)
                log.Debug("Connection established and sending prescence");
            xmpp.SendMyPresence();
        }

        private void xmpp_OnWriteXml(object sender, string xml)
        {
            if (log.IsDebugEnabled)
                log.Debug(string.Format("OnWriteXml : {0}", xml));
        }

        private void xmpp_OnReadXml(object sender, string xml)
        {
            if (log.IsDebugEnabled)
                log.Debug(string.Format("OnReadXml : {0}", xml));
        }

        private void xmpp_OnMessage(object sender, Message msg)
        {
            if (log.IsDebugEnabled)
                log.Debug(string.Format("{0} - {1}", msg.Type, msg.Body));

            if (ShouldHandleMessage(msg))
            {
                OnMessage(this, new OnMessageHandlerArgs(msg));
            }
        }

        private bool ShouldHandleMessage(Message msg)
        {
            return msg.Type == MessageType.chat && !string.IsNullOrEmpty(msg.Body);
        }

        public event OnRequestMessageHandler OnMessage;

        public void SendMessage(string jid, string message)
        {
            var to = new Jid(jid);
            var msg = new Message(to, MessageType.chat, message);

            if (log.IsDebugEnabled)
                log.Debug(string.Format("msg being sent to: {0} msg text : {1}", msg.To, msg.Body));

            xmpp.Send(msg);
        }
    }
}