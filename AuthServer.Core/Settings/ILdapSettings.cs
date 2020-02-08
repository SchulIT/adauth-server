namespace AuthServer.Core.Settings
{
    /// <summary>
    /// Settings for LDAP connection
    /// </summary>
    public interface ILdapSettings
    {
        /// <summary>
        /// The Hostname or IP of the domain controller
        /// </summary>
        string Server { get; }

        /// <summary>
        /// Port of the LDAP server
        /// </summary>
        int Port { get; }

        /// <summary>
        /// The name of the domain including 
        /// </summary>
        string DomainFQDN { get; }

        /// <summary>
        /// The domain name (NetBIOS)
        /// </summary>
        string DomainNetBIOS { get; }

        /// <summary>
        /// The username of a read-only ldap user
        /// </summary>
        string Username { get; }

        /// <summary>
        /// The password of a read-only ldap user
        /// </summary>
        string Password { get; }
    }
}
