using System;

namespace ServerCore.Performance
{
    public class ConsolePerformanceMonitor : IPerformanceMonitor
    {
        public void WriteDeadConnections(int deadConnections)
        {
            Console.WriteLine($"Dead connections: {deadConnections}");
        }

        public void WriteOpenConnections(int openConnections)
        {
            Console.WriteLine($"Open connections: {openConnections}");
        }
    }
}
