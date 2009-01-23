using System.ServiceProcess;
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
            controller = ServiceLocator.Retrieve<ChatController>();
            controller.Start();
        }

        protected override void OnStop()
        {
            if (controller != null)
                controller.Stop();
        }
    }
}