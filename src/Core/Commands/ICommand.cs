using agsXMPP.protocol.client;

namespace Chatsworth.Core.Commands
{
    public interface ICommand
    {
        void Execute(Message message);
        string CommandName { get; }
    }
}