namespace ChatsworthLib.Entity
{
    public class ServerConfiguration
    {
        public ServerConfiguration(string server, string username, string password)
        {
            Server = server;
            Username = username;
            Password = password;
        }

        public string Server { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}