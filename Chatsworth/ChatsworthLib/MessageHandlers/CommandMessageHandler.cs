using System;
using agsXMPP.protocol.client;
using ChatsworthLib.Commands;

namespace ChatsworthLib.MessageHandlers
{
    public class CommandMessageHandler : IMessageHandler
    {
        private readonly ICommand[] _commands;

        public CommandMessageHandler(ICommand[] commands)
        {
            _commands = commands;
        }

        public void ProcessMessage(Message message)
        {
            if (!CanProcess(message))
                return;

            string commandString = CommandProcessor.GetCommandFromMessage(message.Body);
            ICommand command = Array.Find(_commands, c => c.CommandName == commandString.ToLower()); 
            if(command != null)
                command.Execute(message);
        }

        public bool CanProcess(Message message)
        {
            if (message == null)
                return false;

            return !string.IsNullOrEmpty(CommandProcessor.GetCommandFromMessage(message.Body));
        }
    }
}