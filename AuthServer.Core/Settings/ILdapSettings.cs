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
        /// Flag whether to use SSL (note: you also need to specify the certificate fingerpint!)
        /// </summary>
        bool UseSSL { get; }

        /// <summary>
        /// Flag whether to use TLS (note: you also need to specify the certificate fingerpint!)
        /// </summary>
        bool UseTLS { get; }

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

        /// <summary>
        /// Thumbprint of a certificate which is included in the certificate
        /// chain used by the Domain Controller.
        /// </summary>
        string CertificateThumbprint { get; }

        /// <summary>
        /// Specifies which attribute is used for usernames.
        /// </summary>
        UsernameProperty UsernameProperty { get; }

        /// <summary>
        /// List of allowed UPN suffixes (only used if UsernameProperty.UserPrincipalName is used)
        /// </summary>
        string[] AllowedUpnSuffixes { get; }
    }
}
