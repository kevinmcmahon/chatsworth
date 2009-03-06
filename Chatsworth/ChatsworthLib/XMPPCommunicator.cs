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
        private XmppClientConnection _xmpp;
        private ILog _logger = new NullLogger();

        public ILog Log
        {
            get { return _logger;}
            set { _logger = value; }
        }

        public void Configure(ServerConfiguration configuration)
        {
            _xmpp = new XmppClientConnection
                       {
                           Server = configuration.Server,
                           Username = configuration.Username,
                           Password = configuration.Password,
                           KeepAlive = true,
                           UseStartTLS = true,
                           SocketConnectionType = SocketConnectionType.Direct,
                           AutoResolveConnectServer = true
                       };

            RegisterXmppEventHandlers();
        }

        private void RegisterXmppEventHandlers()
        {
            _xmpp.OnMessage += xmpp_OnMessage;
            _xmpp.OnReadXml += xmpp_OnReadXml;
            _xmpp.OnWriteXml += xmpp_OnWriteXml;
            _xmpp.OnLogin += xmpp_OnLogin;
            _xmpp.OnAuthError += xmpp_OnAuthError;
            _xmpp.OnSocketError += xmpp_OnSocketError;
            _xmpp.OnError += xmpp_OnError;
            _xmpp.OnXmppConnectionStateChanged += xmpp_OnXmppConnectionStateChanged;
            _xmpp.OnClose += xmpp_OnClose;
        }

        public void OpenConnection()
        {
            if (Log.IsDebugEnabled)
                Log.Debug("Attempting to open connection.");

            _xmpp.SocketDisconnect();
            _xmpp.Open();
        }

        public event OnRequestMessageHandler OnMessage;

        public void SendMessage(string jid, string message)
        {
            var to = new Jid(jid);
            var msg = new Message(to, MessageType.chat, message);

            if (Log.IsDebugEnabled)
                Log.DebugFormat("msg being sent to: {0} msg text : {1}", msg.To, msg.Body);

            _xmpp.Send(msg);
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
                if (Log.IsFatalEnabled)
                    Log.Fatal("Reconnection error.  Server or network is down.");
                DisposeConnectionTimer();
            }
        }

        private void xmpp_OnXmppConnectionStateChanged(object sender, XmppConnectionState state)
        {
            if (Log.IsDebugEnabled)
                Log.DebugFormat("OnXmppConnectionStateChanged : Connection State - {0}", state);
        }


        private void xmpp_OnLogin(object sender)
        {
            if (Log.IsDebugEnabled)
                Log.Debug("OnLogin : Connection established and sending prescence");

            _xmpp.SendMyPresence();

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
            if (Log.IsInfoEnabled)
                Log.InfoFormat("OnWriteXml : {0}", xml);
        }

        private void xmpp_OnReadXml(object sender, string xml)
        {
            if (Log.IsInfoEnabled)
                Log.InfoFormat("OnReadXml : {0}", xml);
        }

        private void xmpp_OnMessage(object sender, Message msg)
        {
            if (Log.IsInfoEnabled)
                Log.InfoFormat("OnMessage : {0} - {1}", msg.Type, msg.Body);

            if (ShouldHandleMessage(msg))
            {
                OnMessage(this, new OnMessageHandlerArgs(msg));
            }
        }

        private void xmpp_OnError(object sender, Exception ex)
        {
            if (Log.IsErrorEnabled)
                Log.ErrorFormat("OnError : {0} - {1}", ex.Message, ex.StackTrace);
        }

        private void xmpp_OnSocketError(object sender, Exception ex)
        {
            if(Log.IsErrorEnabled)
                Log.ErrorFormat("OnSocketError : message = {0} - stack trace = {1}", ex.Message, ex.StackTrace);
        }

        private void xmpp_OnAuthError(object sender, Element e)
        {
            if (Log.IsErrorEnabled)
                Log.Error("OnAuthError : Authorization Error");
        }

        private bool ShouldHandleMessage(Message msg)
        {
            return msg.Type == MessageType.chat && !string.IsNullOrEmpty(msg.Body);
        }
    }
}