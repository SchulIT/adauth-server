namespace ServerCore.Settings
{
    /// <summary>
    /// Server settings
    /// </summary>
    public interface IServerSettings
    {
        /// <summary>
        /// Port to bind to 
        /// </summary>
        int Port { get; }

        /// <summary>
        /// Bind to :: (IPv6) instead of 0.0.0.0 (IPv4)
        /// </summary>
        bool IPv6 { get; }
    }
}
