using System;
using System.ServiceProcess;

namespace Chatsworth
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main(string[] args)
        {
            bool runAsConsoleApp = false;

            if (args.Length > 0 && string.Equals("/console", args[0]))
            {
                runAsConsoleApp = true;
            }

            if (runAsConsoleApp)
            {
                using (var serviceRunner = new ChatsworthServiceRunner())
                {
                    serviceRunner.Start();
                    Console.ReadLine();
                }
            }
            else
            {
                var ServicesToRun = new ServiceBase[]
                                        {
                                            new ChatsworthService()
                                        };
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}