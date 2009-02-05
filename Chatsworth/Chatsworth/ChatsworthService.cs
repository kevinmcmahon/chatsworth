using System.Configuration;
using System.ServiceProcess;
using Chatsworth.Properties;
using ChatsworthLib;
using ChatsworthLib.DataAccess;
using ChatsworthLib.Entity;

namespace Chatsworth
{
    public partial class ChatsworthService : ServiceBase
    {
        private ChatController controller;

        public ChatsworthService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            ConfigureMemberDirectory();
            ConfigureCommunicator();
            StartServer();
        }

        private void ConfigureMemberDirectory()
        {
            ChatMemberRespository respository = CreateChatMemberRepository();
            var directory = ServiceLocator.Retrieve<IMemberDirectory>();
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
            if (controller != null)
                controller.Stop();
        }

        private void ConfigureCommunicator()
        {
            var configuration = new ServerConfiguration(Settings.Default.Server, Settings.Default.Username,
                                                        Settings.Default.Password);

            var communicator = ServiceLocator.Retrieve<ICommunicator>();
            
            communicator.Configure(configuration);
        }

        private void StartServer()
        {
            controller = ServiceLocator.Retrieve<ChatController>();
            controller.Start();
        }
    }
}