namespace AuthServer.Core.Settings
{
    /// <summary>
    /// Settings for TLS
    /// </summary>
    public interface ITlsSettings
    {
        string Certificate { get; }

        string PreSharedKey { get; }
    }
}
