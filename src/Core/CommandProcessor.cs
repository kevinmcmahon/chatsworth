namespace Chatsworth.Core
{
    public static class CommandProcessor
    {
        public static string GetCommandFromMessage(string messageBody)
        {
            string firstWord = ExtractFirstWord(messageBody);
            return firstWord.StartsWith("/") ? firstWord.Replace("/", "") : string.Empty;
        }

        private static string ExtractFirstWord(string messageBody)
        {
            if (string.IsNullOrEmpty(messageBody))
                return "";
            string[] words = TokenizeMessage(messageBody);

            return words[0];
        }

        public static string[] TokenizeMessage(string messageBody)
        {
            return messageBody.Split(' ');
        }
    }
}