using System;
using System.Threading;
using agsXMPP;
using agsXMPP.net;
using agsXMPP.protocol.client;
using agsXMPP.Xml.Dom;
using ChatsworthLib.Entity;
using log4net;

namespace ChatsworthLib
{
    public class XMPPCommunicator : ICommunicator
    {
        private const int MAX_RECONNECT_TIME = 120000; // 120s max timer delay 
        private static readonly ILog log = LogManager.GetLogger("ChatsworthLib.Logger");
        private Timer _reconnectTimer;
        private int _timerDelay = 15000; // 15s start delay
        private XmppClientConnection xmpp;

        public void Configure(ServerConfiguration configuration)
        {
            xmpp = new XmppClientConnection
                       {
                           Server = configuration.Server,
                           Username = configuration.Username,
                           Password = configuration.Password,
                           KeepAlive = true,
                           UseStartTLS = true,
                           SocketConnectionType = SocketConnectionType.Direct,
                           AutoResolveConnectServer = true
                       };

            xmpp.OnMessage += xmpp_OnMessage;
            xmpp.OnReadXml += xmpp_OnReadXml;
            xmpp.OnWriteXml += xmpp_OnWriteXml;
            xmpp.OnLogin += xmpp_OnLogin;
            xmpp.OnAuthError += xmpp_OnAuthError;
            xmpp.OnSocketError += xmpp_OnError;
            xmpp.OnError += xmpp_OnError;
            xmpp.OnXmppConnectionStateChanged += xmpp_OnXmppConnectionStateChanged;
            xmpp.OnClose += xmpp_OnClose;
        }

        public void OpenConnection()
        {
            if (log.IsDebugEnabled)
                log.Debug("Attempting to open connection.");
            xmpp.Open();
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

        private void xmpp_OnClose(object sender)
        {
            if (log.IsDebugEnabled)
                log.Debug("OnClose : Attempting Re-connection.");

            DelayedReconnect();
        }

        private void DelayedReconnect()
        {
            _reconnectTimer = new Timer(AttemptReconnection, null, 0, _timerDelay);
        }

        private void AttemptReconnection(object state)
        {
            OpenConnection();
            UpdateReconnectTimer();
        }

        private void UpdateReconnectTimer()
        {
            _timerDelay = _timerDelay*2;

            if (_timerDelay <= MAX_RECONNECT_TIME)
            {
                if (log.IsDebugEnabled)
                    log.DebugFormat("Reconnection timer being set to {0} seconds.", _timerDelay/1000);
                _reconnectTimer.Change(0, _timerDelay);
            }
            else
            {
                if (log.IsDebugEnabled)
                    log.Debug("Reconnection error.  Server or network is down.");
                DisposeConnectionTimer();
            }
        }

        private void xmpp_OnXmppConnectionStateChanged(object sender, XmppConnectionState state)
        {
            if (log.IsDebugEnabled)
                log.Debug(string.Format("Connection state changed. Connection State: {0}", state));
        }

        private void xmpp_OnLogin(object sender)
        {
            if (log.IsDebugEnabled)
                log.Debug("Connection established and sending prescence");

            xmpp.SendMyPresence();

            DisposeConnectionTimer();
        }

        private void DisposeConnectionTimer()
        {
            if (_reconnectTimer != null)
            {
                _reconnectTimer.Dispose();
                _reconnectTimer = null;
            }
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

        private void xmpp_OnError(object sender, Exception ex)
        {
            if (log.IsErrorEnabled)
                log.Debug(string.Format("Error Occurred. {0} - {1}", ex.Message, ex.StackTrace));
        }

        private void xmpp_OnAuthError(object sender, Element e)
        {
            if (log.IsDebugEnabled)
                log.Debug("Authorization Error");
        }

        private bool ShouldHandleMessage(Message msg)
        {
            return msg.Type == MessageType.chat && !string.IsNullOrEmpty(msg.Body);
        }
    }
}