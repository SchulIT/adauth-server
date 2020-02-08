using Microsoft.Extensions.Logging;

namespace AuthServer.Core.Performance
{
    public class ConsolePerformanceMonitor : IPerformanceMonitor
    {
        private ILogger<ConsolePerformanceMonitor> logger;

        public ConsolePerformanceMonitor(ILogger<ConsolePerformanceMonitor> logger)
        {
            this.logger = logger;
        }


        public void WriteDeadConnections(int deadConnections)
        {
            logger.LogDebug($"Dead connections: {deadConnections}");
        }

        public void WriteOpenConnections(int openConnections)
        {
            logger.LogDebug($"Open connections: {openConnections}");
        }
    }
}
