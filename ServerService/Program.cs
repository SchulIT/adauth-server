using System.ServiceProcess;

namespace ServerService
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new AdAuthServer()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
