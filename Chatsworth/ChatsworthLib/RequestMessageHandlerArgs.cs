using System;
using agsXMPP.protocol.client;

namespace ChatsworthLib
{
    public class RequestMessageHandlerArgs : EventArgs
    {
        public RequestMessageHandlerArgs(Message msg)
        {
            Message = msg;
        }

        public Message Message { get; set; }
    }
}
