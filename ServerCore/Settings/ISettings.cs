namespace ServerCore.Settings
{
    /// <summary>
    /// Settings
    /// </summary>
    public interface ISettings
    {
        IServerSettings Server { get; }

        ITlsSettings Tls { get; }

        ILdapSettings Ldap { get; }

        /// <summary>
        /// Attribute in which an unique ID is stored in
        /// </summary>
        string UniqueIdAttributeName { get; }
    }
}
