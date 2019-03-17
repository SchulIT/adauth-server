namespace ServerCore.Settings
{
    /// <summary>
    /// Settings for TLS
    /// </summary>
    public interface ITlsSettings
    {
        bool IsEnabled { get; }

        string Certificate { get; }

        string PreSharedKey { get; }
    }
}
