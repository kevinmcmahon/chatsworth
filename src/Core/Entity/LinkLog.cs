using System;

namespace Chatsworth.Core.Entity
{
    public class LinkLog
    {
        public virtual int Id { get; set;}
        public virtual DateTime DateLogged { get; set; }
        public virtual string Url { get; set; }
        public virtual ChatMember Sender { get; set; }
    }
}
