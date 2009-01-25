using System;
using agsXMPP.protocol.client;

namespace ChatsworthLib
{
    public class OnMessageHandlerArgs : EventArgs
    {
        public OnMessageHandlerArgs(Message msg)
        {
            Message = msg;
        }

        public Message Message { get; set; }
    }
}
