using agsXMPP.protocol.client;

namespace Chatsworth.Core.Commands
{
    /// <summary>
    /// The command interface.
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="message"></param>
        void Execute(Message message);

        /// <summary>
        /// The name of the command.
        /// </summary>
        string CommandName { get; }
    }
}