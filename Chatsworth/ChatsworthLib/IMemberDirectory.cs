using ChatsworthLib.Entity;

namespace ChatsworthLib
{
    public interface IMemberDirectory
    {
        ChatMemberCollection ChatSubscribers { get; set;}
    }
}