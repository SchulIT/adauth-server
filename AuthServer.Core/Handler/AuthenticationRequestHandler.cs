using AuthServer.Core.Provider;
using AuthServer.Core.Request;
using AuthServer.Core.Response;

namespace AuthServer.Core.Handler
{
    public class AuthenticationRequestHandler : IRequestHandler
    {

        private IUserProvider userProvider;

        public AuthenticationRequestHandler(IUserProvider userProvider)
        {
            this.userProvider = userProvider;
        }

        public bool CanHandle(IRequest request)
        {
            return request is AuthenticationRequest;
        }

        public IResponse Handle(IRequest request)
        {
            var authenticationRequest = request as AuthenticationRequest;
            var user = userProvider.Authenticate(authenticationRequest.Username, authenticationRequest.Password);

            if (user == null)
            {
                return new AuthenticationResponse { Success = false };
            }

            return new AuthenticationResponse
            {
                Success = true,
                Username = user.Username,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                DisplayName = user.DisplayName,
                UniqueId = user.UniqueId,
                OU = user.OU,
                Email = user.Email,
                Groups = user.Groups
            };
        }
    }
}
