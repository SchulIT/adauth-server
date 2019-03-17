namespace ServerCore.Settings
{
    /// <summary>
    /// Settings for LDAP connection
    /// </summary>
    public interface ILdapSettings
    {
        /// <summary>
        /// The Hostname or IP of the domain controller
        /// </summary>
        string DC { get; }

        /// <summary>
        /// The domain name
        /// </summary>
        string Domain { get; }

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
