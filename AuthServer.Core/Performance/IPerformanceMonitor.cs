namespace AuthServer.Core.Performance
{
    /// <summary>
    /// Interface for performance monitoring
    /// </summary>
    public interface IPerformanceMonitor
    {
        void WriteOpenConnections(int openConnections);

        void WriteDeadConnections(int deadConnections);
    }
}
