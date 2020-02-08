using System.Diagnostics.Tracing;

namespace AuthServer.Core.Performance
{
    [EventSource(Name = SourceName)]
    public class EventCountersPerformanceMonitor : EventSource, IPerformanceMonitor
    {
        private const string SourceName = "AuthServer.Connections";

        private EventCounter deadConnectionsCounter;
        private EventCounter openConnectionsCounter;

        public EventCountersPerformanceMonitor()
        {
            deadConnectionsCounter = new EventCounter("dead-connections", this);
            openConnectionsCounter = new EventCounter("open-connections", this);
        }

        public void WriteDeadConnections(int deadConnections)
        {
            deadConnectionsCounter.WriteMetric(deadConnections);
        }

        public void WriteOpenConnections(int openConnections)
        {
            openConnectionsCounter.WriteMetric(openConnections);
        }
    }
}
