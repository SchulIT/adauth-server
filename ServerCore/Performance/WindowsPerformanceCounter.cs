using ServerCore.Log;
using System;
using System.Diagnostics;

namespace ServerCore.Performance
{
    /// <summary>
    /// Logs performace into Windows Performance Counters
    /// </summary>
    public class WindowsPerformanceCounter : IPerformanceMonitor
    {
        private const string CategoryName = "AD Auth Server";
        private const string OpenConnectionsCounterName = "Open connections";
        private const string DeadConnectionsCounterName = "Dead connections";

        private ILogger logger;

        public WindowsPerformanceCounter(ILogger logger)
        {
            this.logger = logger;

            if(!PerformanceCounterCategory.Exists(CategoryName))
            {
                logger.WriteLog("Performance counters are not installed, thus no performance monitoring will happen.");
            }
        }

        public void WriteDeadConnections(int deadConnections)
        {
            if(!PerformanceCounterCategory.Exists(CategoryName))
            {
                return;
            }

            try
            {
                var counter = new PerformanceCounter(CategoryName, DeadConnectionsCounterName, false);
                counter.RawValue = deadConnections;
            }
            catch (Exception)
            {
                logger.WriteDebug("Cannot write PerformanceCounter (maybe not setup correctly?)");
            }
        }

        public void WriteOpenConnections(int openConnections)
        {
            if (!PerformanceCounterCategory.Exists(CategoryName))
            {
                return;
            }

            try
            {
                var counter = new PerformanceCounter(CategoryName, OpenConnectionsCounterName, false);
                counter.RawValue = openConnections;
            }
            catch (Exception)
            {
                logger.WriteDebug("Cannot write PerformanceCounter (maybe not setup correctly?)");
            }
        }
    }
}
