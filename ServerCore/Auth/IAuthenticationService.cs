using ServerCore.Response;

namespace ServerCore.Auth
{
    /// <summary>
    /// An interface for an authentication service
    /// </summary>
    public interface IAuthenticationService
    {
        AuthResponse Login(string username, string password);
    }
}
