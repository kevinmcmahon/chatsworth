using agsXMPP.protocol.client;

namespace ChatsworthLib.Commands
{
    public interface ICommand
    {
        void Execute(Message message);
        string CommandName { get; }
    }
}