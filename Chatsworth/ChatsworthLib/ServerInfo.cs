namespace ChatsworthLib
{
    public class ServerInfo
    {
        public ServerInfo(string server, string connectServer, string username, string password)
        {
            Server = server;
            ConnectServer = connectServer;
            Username = username;
            Password = password;
        }

        public string Server { get; set; }
        public string ConnectServer { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}