using CPI.DirectoryServices;
using ServerCore.Log;
using ServerCore.Response;
using ServerCore.Settings;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;

namespace ServerCore.Auth
{
    /// <summary>
    /// Authentication service which authenticates a user against the active directory
    /// </summary>
    public class ActiveDirectoryAuthenticationService : IAuthenticationService
    {
        private ISettings settings;
        private ILogger logger;

        public ActiveDirectoryAuthenticationService(ISettings settings, ILogger logger)
        {
            this.settings = settings;
            this.logger = logger;
        }

        public AuthResponse Login(string username, string password)
        {
            var container = "DC=" + settings.Ldap.Domain.Replace(".", ",DC=");

            try
            {
                using (var principal = new PrincipalContext(ContextType.Domain, settings.Ldap.DC, container, settings.Ldap.Username, settings.Ldap.Password))
                {
                    var response = new AuthResponse();

                    var loginSuccessful = false;

                    try
                    {
                        loginSuccessful = principal.ValidateCredentials(username, password);
                    }
                    catch { }

                    if (!loginSuccessful)
                    {
                        response.Success = false;
                        return response;
                    }

                    var findUser = new UserPrincipal(principal);
                    findUser.SamAccountName = username;

                    using (var searcher = new PrincipalSearcher(findUser))
                    {
                        var user = searcher.FindOne() as UserPrincipal;

                        if (user != null)
                        {
                            response.Success = true;
                            response.Username = user.SamAccountName;
                            response.Firstname = user.GivenName;
                            response.Lastname = user.Surname;
                            response.DispalyName = user.DisplayName;
                            response.UniqueId = GetValue(user, settings.UniqueIdAttributeName);
                            response.OU = GetOU(user);
                            response.Groups = GetGroups(user);
                        }
                    }

                    return response;
                }
            }
            catch (Exception e)
            {
                logger.WriteException(e);
            }

            return new AuthResponse
            {
                Success = false
            };
        }

        private string GetValue(UserPrincipal principal, string propertyName)
        {
            if(propertyName == null)
            {
                return null;
            }

            var entry = principal.GetUnderlyingObject() as DirectoryEntry;
            
            if(entry == null)
            {
                return null;
            }

            if(!entry.Properties.Contains(propertyName))
            {
                return null;
            }

            var property = entry.Properties[propertyName];

            return property.Value?.ToString();
        }

        private List<string> GetGroups(UserPrincipal userPrincipal)
        {
            var groups = userPrincipal.GetGroups();
            return groups.Select(x => x.DistinguishedName).ToList();
        }

        private string GetOU(UserPrincipal userPrincipal)
        {
            var dn = new DN(userPrincipal.DistinguishedName);
            return dn.Parent.ToString();
        }
    }
}
