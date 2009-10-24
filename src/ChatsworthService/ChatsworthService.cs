using System.ServiceProcess;

namespace Chatsworth
{
    /// <summary>
    /// Manages the Chatsworth bot service
    /// </summary>
    public partial class ChatsworthService : ServiceBase
    {
        private readonly ChatsworthServiceRunner _serviceRunner;

        /// <summary>
        /// Ctor
        /// </summary>
        public ChatsworthService()
        {
            InitializeComponent();

            _serviceRunner = new ChatsworthServiceRunner();
        }

        /// <summary>
        /// Start method for the service
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            _serviceRunner.Start();
        }

        /// <summary>
        /// Stop method for the service.
        /// </summary>
        protected override void OnStop()
        {
            _serviceRunner.Stop();
        }
    }
}