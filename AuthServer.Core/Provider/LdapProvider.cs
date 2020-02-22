using AuthServer.Core.Settings;
using CPI.DirectoryServices;
using Microsoft.Extensions.Logging;
using Novell.Directory.Ldap;
using System;
using System.Collections.Generic;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace AuthServer.Core.Provider
{
    public class LdapProvider : IUserProvider
    {
        private const string MemberOfAttribute = "memberOf";
        private const string EmailAttribute = "mail";
        private const string DisplayNameAttribute = "displayName";
        private const string FirstnameAttribute = "givenName";
        private const string LastnameAttribute = "sn";
        private const string UsernameAttribute = "sAMAccountName";
        private const string AccountControlAttribute = "UserAccountControl";
        private const int IsActiveAttributeValue = 0x2;

        private const string UsernameRegexp = @"([A-Za-z0-9_\-\.])*";
        private const string SearchFilter = "({key}={username})";

        private ISettings settings;
        private ILogger<LdapProvider> logger;

        public LdapProvider(ISettings settings, ILogger<LdapProvider> logger)
        {
            this.settings = settings;
            this.logger = logger;
        }

        public User GetInformation(string username)
        {
            var regexp = new Regex(UsernameRegexp);

            if (!regexp.IsMatch(username))
            {
                logger.LogInformation($"Invalid username detected: '{username}'");

                return null;
            }

            using (var ldapConnection = new LdapConnection())
            {
                try
                {
                    ConnectLdapConnection(ldapConnection);
                    ldapConnection.Bind(settings.Ldap.Username, settings.Ldap.Password);

                    var container = "DC=" + settings.Ldap.DomainFQDN.Replace(".", ",DC=");
                    var attributes = new List<string>() { MemberOfAttribute, EmailAttribute, DisplayNameAttribute, FirstnameAttribute, LastnameAttribute, UsernameAttribute, AccountControlAttribute };

                    if (!string.IsNullOrEmpty(settings.UniqueIdAttributeName))
                    {
                        attributes.Add(settings.UniqueIdAttributeName);
                    }

                    var results = ldapConnection.Search(container, LdapConnection.SCOPE_SUB, SearchFilter.Replace("{key}", UsernameAttribute).Replace("{username}", username), attributes.ToArray(), false);

                    if (results.HasMore())
                    {
                        var entry = results.Next();

                        string uniqueId = null;

                        if (!string.IsNullOrEmpty(settings.UniqueIdAttributeName))
                        {
                            uniqueId = entry.getAttribute(settings.UniqueIdAttributeName).StringValue;
                        }

                        var isActive = false;
                        var accountControlValue = entry.getAttribute(AccountControlAttribute)?.StringValue;

                        if (accountControlValue != null)
                        {
                            var accountControlIntValue = int.Parse(accountControlValue);
                            isActive = !((accountControlIntValue & IsActiveAttributeValue) == IsActiveAttributeValue);
                        }

                        return new User
                        {
                            IsActive = isActive,
                            Username = entry.getAttribute(UsernameAttribute)?.StringValue,
                            Firstname = entry.getAttribute(FirstnameAttribute)?.StringValue,
                            Lastname = entry.getAttribute(LastnameAttribute)?.StringValue,
                            DisplayName = entry.getAttribute(DisplayNameAttribute)?.StringValue,
                            Email = entry.getAttribute(EmailAttribute)?.StringValue,
                            UniqueId = uniqueId,
                            Groups = entry.getAttribute(MemberOfAttribute)?.StringValueArray,
                            OU = GetOU(entry.DN)
                        };
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (LdapException e)
                {
                    logger.LogDebug(e, $"LDAP-Error while searching for user '{username}'");
                }
                catch (Exception e)
                {
                    logger.LogError(e, $"Non-LDAP-Error while searching for user '{username}'");
                }
            }

            return null;
        }

        private void ConnectLdapConnection(LdapConnection ldapConnection)
        {
            ldapConnection.UserDefinedServerCertValidationDelegate += CheckCertificateCallback;

            /**
             * CONFIGURE SSL
             */
            if (settings.Ldap.UseSSL)
            {
                logger.LogDebug("Starting SSL session.");
                ldapConnection.SecureSocketLayer = true;
            }

            /**
             * CONNECT
             */
            ldapConnection.Connect(settings.Ldap.Server, settings.Ldap.Port);

            /**
             * CONFIGURE TLS
             */
            if (settings.Ldap.UseTLS)
            {
                logger.LogDebug("Starting TLS session.");
                ldapConnection.StartTls();
            }
        }

        private bool CheckCertificateCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            foreach (var cert in chain.ChainElements)
            {
                if (cert.Certificate.Thumbprint.ToLower() == settings.Ldap.CertificateThumbprint.ToLower())
                {
                    return true;
                }
            }

            return false;
        }

        public User Authenticate(string username, string password)
        {
            var regexp = new Regex(UsernameRegexp);

            if (!regexp.IsMatch(username))
            {
                logger.LogInformation($"Invalid username detected: '{username}'");

                return null;
            }

            using (var ldapConnection = new LdapConnection())
            {
                try
                {
                    ConnectLdapConnection(ldapConnection);
                    var usernameDn = settings.Ldap.DomainNetBIOS + @"\" + username;
                    ldapConnection.Bind(usernameDn, password);

                    return GetInformation(username);
                }
                catch (LdapException e)
                {
                    logger.LogDebug(e, "LDAP-Error while authenticating");
                }
                catch (Exception e)
                {
                    logger.LogError(e, "Non-LDAP-Error while authenticating");
                }
            }

            return null;
        }

        private string GetOU(string dn)
        {
            return (new DN(dn).Parent.ToString());
        }
    }
}
