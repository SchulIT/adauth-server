namespace AuthServer.Core.Provider
{
    /// <summary>
    /// An interface for an authentication service
    /// </summary>
    public interface IUserProvider
    {
        User Authenticate(string username, string password);

        User GetInformation(string username);
    }
}
