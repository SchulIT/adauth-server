using AuthServer.Core.Provider;
using AuthServer.Core.Request;
using AuthServer.Core.Response;

namespace AuthServer.Core.Handler
{
    public class StatusRequestHandler : IRequestHandler
    {

        private IUserProvider userProvider;

        public StatusRequestHandler(IUserProvider userProvider)
        {
            this.userProvider = userProvider;
        }

        public bool CanHandle(IRequest request)
        {
            return request is StatusRequest;
        }

        public IResponse Handle(IRequest request)
        {
            var statusRequest = request as StatusRequest;

            var response = new StatusResponse();

            foreach(var username in statusRequest.Users)
            {
                response.Users.Add(username, FromUser(userProvider.GetInformation(username)));
            }

            return response;
        }

        private UserStatusResponse FromUser(User user)
        {
            if (user == null)
            {
                return null;
            }

            return new UserStatusResponse
            {
                IsActive = user.IsActive,
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
