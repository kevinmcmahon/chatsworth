using System;
using System.Configuration;
using System.IO;
using Chatsworth.Core;
using Chatsworth.Core.DataAccess;
using Chatsworth.Core.Entity;
using Chatsworth.Properties;
using log4net;

namespace Chatsworth
{
    /// <summary>
    /// Manages the starting and stopping of the chat bot.
    /// </summary>
    public class ChatsworthServiceRunner : IDisposable
    {
        private static readonly ILog _log = LogManager.GetLogger("Chatsworth.Logger");
        private ChatController _controller;

        /// <summary>
        /// Starts the chat bot.
        /// </summary>
        public void Start()
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            PerformConfiguration();
            StartServer();
        }

        /// <summary>
        /// Stops the chat bot.
        /// </summary>
        public void Stop()
        {
            if(_controller != null)
                _controller.Stop();
        }

        /// <summary>
        /// Disposes of the chat bot.
        /// </summary>
        public void Dispose()
        {
            this.Stop();
        }

        private void PerformConfiguration()
        {
            ConfigureServices(_log);
            ConfigureCommunicator(_log);
        }

        private void ConfigureServices(ILog log)
        {
            NHibernateSessionManager sessionManager = CreateSessionManager();
            var chatMemberRespository = new ChatMemberRespository(sessionManager);
            var linkLoggerRepository = new LinkLoggerRepository(sessionManager);

            var directory = ServiceLocator.Retrieve<IMemberDirectory>();
            directory.Log = log;
            directory.AttachRepository(chatMemberRespository);

            var linkLogger = ServiceLocator.Retrieve<ILinkLogger>();
            linkLogger.Log = log;
            linkLogger.AttachRepository(linkLoggerRepository);
        }

        private NHibernateSessionManager CreateSessionManager()
        {
            var sessionConfig = new NHibernateSessionConfig
                                    {
                                        ConnectionString =
                                            ConfigurationManager.ConnectionStrings["default"].ConnectionString,
                                        MappingAssembly = Settings.Default.MappingAssembly
                                    };
            return new NHibernateSessionManager(sessionConfig);
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