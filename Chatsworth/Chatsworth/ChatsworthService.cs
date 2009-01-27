﻿using System.ServiceProcess;
using Chatsworth.Properties;
using ChatsworthLib;

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
            ConfigureCommunicator();
            StartServer();
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