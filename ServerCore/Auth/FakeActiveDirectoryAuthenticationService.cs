using ServerCore.Response;
using System.Collections.Generic;

namespace ServerCore.Auth
{
    /// <summary>
    /// A fake auth service which authenticates against a local in-memory database of
    /// users.
    /// </summary>
    public class FakeActiveDirectoryAuthenticationService : IAuthenticationService
    {
        private readonly List<AuthResponse> responses = new List<AuthResponse>();

        public FakeActiveDirectoryAuthenticationService()
        {
            responses.Add(new AuthResponse
            {
                UniqueId = "42",
                Username = "test.student",
                Firstname = "Vorname",
                Lastname = "Nachname",
                Groups = new List<string> { "09A", "Schüler-global" }
            });

            responses.Add(new AuthResponse
            {
                UniqueId = "L14",
                Username = "TL",
                Firstname = "Test",
                Lastname = "Lehrer",
                Groups = new List<string> { "Lehrer-global", "NFS-teachers" }
            });
        }

        public AuthResponse Login(string username, string password)
        {
            foreach (var response in responses)
            {
                if(response.Username == username)
                {
                    return response;
                }
            }

            return new AuthResponse { Success = false };
        }
    }
}
