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
        private Timer _reconnectTimer;
        private int _timerDelay = 15000; // 15s start delay
        private XmppClientConnection xmpp;
        private ILog _logger = new NullLogger();

        public ILog Log
        {
            get { return _logger;}
            set { _logger = value; }
        }

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
            if (Log.IsDebugEnabled)
                Log.Debug("Attempting to open connection.");
            xmpp.Open();
        }

        public event OnRequestMessageHandler OnMessage;

        public void SendMessage(string jid, string message)
        {
            var to = new Jid(jid);
            var msg = new Message(to, MessageType.chat, message);

            if (Log.IsDebugEnabled)
                Log.DebugFormat("msg being sent to: {0} msg text : {1}", msg.To, msg.Body);

            xmpp.Send(msg);
        }

        private void xmpp_OnClose(object sender)
        {
            if (Log.IsDebugEnabled)
                Log.Debug("OnClose : Attempting Re-connection.");

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
                if (Log.IsDebugEnabled)
                    Log.DebugFormat("Reconnection timer being set to {0} seconds.", _timerDelay / 1000);
                _reconnectTimer.Change(0, _timerDelay);
            }
            else
            {
                if (Log.IsDebugEnabled)
                    Log.Debug("Reconnection error.  Server or network is down.");
                DisposeConnectionTimer();
            }
        }

        private void xmpp_OnXmppConnectionStateChanged(object sender, XmppConnectionState state)
        {
            if (Log.IsDebugEnabled)
                Log.DebugFormat("Connection state changed. Connection State: {0}", state);
        }

        private void xmpp_OnLogin(object sender)
        {
            if (Log.IsDebugEnabled)
                Log.Debug("Connection established and sending prescence");

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
            if (Log.IsDebugEnabled)
                Log.DebugFormat("OnWriteXml : {0}", xml);
        }

        private void xmpp_OnReadXml(object sender, string xml)
        {
            if (Log.IsDebugEnabled)
                Log.DebugFormat("OnReadXml : {0}", xml);
        }

        private void xmpp_OnMessage(object sender, Message msg)
        {
            if (Log.IsDebugEnabled)
                Log.DebugFormat("{0} - {1}", msg.Type, msg.Body);

            if (ShouldHandleMessage(msg))
            {
                OnMessage(this, new OnMessageHandlerArgs(msg));
            }
        }

        private void xmpp_OnError(object sender, Exception ex)
        {
            if (Log.IsErrorEnabled)
                Log.DebugFormat("Error Occurred. {0} - {1}", ex.Message, ex.StackTrace);
        }

        private void xmpp_OnAuthError(object sender, Element e)
        {
            if (Log.IsDebugEnabled)
                Log.Debug("Authorization Error");
        }

        private bool ShouldHandleMessage(Message msg)
        {
            return msg.Type == MessageType.chat && !string.IsNullOrEmpty(msg.Body);
        }
    }
}