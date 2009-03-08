using System.ServiceProcess;

namespace Chatsworth
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main()
        {
            var ServicesToRun = new ServiceBase[]
                                    {
                                        new ChatsworthService()
                                    };
            ServiceBase.Run(ServicesToRun);
        }
    }
}