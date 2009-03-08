using System.Configuration;
using System.ServiceProcess;
using Chatsworth.Properties;
using Chatsworth.Core;
using Chatsworth.Core.DataAccess;
using Chatsworth.Core.Entity;
using log4net;

namespace Chatsworth
{
    public partial class ChatsworthService : ServiceBase
    {
        private static readonly ILog _log = LogManager.GetLogger("Chatsworth.Logger");
        private ChatController _controller;

        public ChatsworthService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            System.IO.Directory.SetCurrentDirectory(System.AppDomain.CurrentDomain.BaseDirectory);
            PerformConfiguration();
            StartServer();
        }

        private void PerformConfiguration()
        {
            ConfigureMemberDirectory(_log);
            ConfigureCommunicator(_log);
        }

        private void ConfigureMemberDirectory(ILog log)
        {
            ChatMemberRespository respository = CreateChatMemberRepository();
            var directory = ServiceLocator.Retrieve<IMemberDirectory>();
            directory.Log = log;
            directory.AttachRepository(respository);
        }

        private ChatMemberRespository CreateChatMemberRepository()
        {
            //TODO: Need to clean this up.  Major abstraction leakage.
            var sessionConfig = new NHibernateSessionConfig
                                              {
                                                  ConnectionString =
                                                      ConfigurationManager.ConnectionStrings["default"].ConnectionString,
                                                  MappingAssembly = Settings.Default.MappingAssembly
                                              };
            var sessionManager = new NHibernateSessionManager(sessionConfig);
            return new ChatMemberRespository(sessionManager);
        }

        protected override void OnStop()
        {
            if (_controller != null)
                _controller.Stop();
        }

        private void ConfigureCommunicator(ILog log)
        {
            var configuration = new ServerConfiguration(Settings.Default.Server, Settings.Default.Username,
                                                        Settings.Default.Password);

            var communicator = ServiceLocator.Retrieve<ICommunicator>();
            communicator.Log = log;
            communicator.Configure(configuration);
        }

        private void StartServer()
        {
            _controller = ServiceLocator.Retrieve<ChatController>();
            _controller.Start();
        }
    }
}