using System;
using agsXMPP.protocol.client;
using ChatsworthLib.Commands;

namespace ChatsworthLib.MessageHandlers
{
    public class CommandMessageHandler : IMessageHandler
    {
        private ICommand[] _commands;

        public CommandMessageHandler(ICommand[] commands)
        {
            _commands = commands;
        }

        public void ProcessMessage(Message message)
        {
            if (!CanProcess(message))
                return;

            string commandString = GetCommandFromMessage(message.Body);
            ICommand command = Array.Find(_commands, c => c.CommandName == commandString.ToLower()); 
            if(command != null)
                command.Execute(message);
        }

        public bool CanProcess(Message message)
        {
            if (message == null)
                return false;

            return !string.IsNullOrEmpty(GetCommandFromMessage(message.Body));
        }

        public string GetCommandFromMessage(string messageBody)
        {
            string firstWord = ExtractFirstWord(messageBody);
            return firstWord.StartsWith("/") ? firstWord.Replace("/", "") : string.Empty; 
        }

        public string ExtractFirstWord(string messageBody)
        {
            if (string.IsNullOrEmpty(messageBody))
                return "";
            string[] words = TokenizeMessage(messageBody);
        
            return words[0];
        }

        private string[] TokenizeMessage(string messageBody)
        {
            return messageBody.Split(' ');
        }
    }
}