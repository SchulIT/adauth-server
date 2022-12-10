namespace AuthServer.Core.Provider
{
    /// <summary>
    /// An interface for an authentication service
    /// </summary>
    public interface IUserProvider
    {
        User Authenticate(string username, string password);

        PasswordResult ChangePassword(string username, string oldPassword, string newPassword);

        PasswordResult ResetPassword(string username, string newPassword, string adminUsername, string adminPassword);

        User GetInformation(string username);
    }
}
