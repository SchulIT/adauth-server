using AuthServer.Core.Provider;
using AuthServer.Core.Request;
using AuthServer.Core.Response;

namespace AuthServer.Core.Handler
{
    public class ResetPasswordHandler : IRequestHandler
    {
        private readonly IUserProvider userProvider;

        public ResetPasswordHandler(IUserProvider userProvider)
        {
            this.userProvider = userProvider;
        }

        public bool CanHandle(IRequest request)
        {
            return request is ResetPasswordRequest;
        }

        public IResponse Handle(IRequest request)
        {
            var passwordResetRequest = request as ResetPasswordRequest;

            if (string.IsNullOrEmpty(passwordResetRequest.Username) || string.IsNullOrEmpty(passwordResetRequest.NewPassword) || string.IsNullOrEmpty(passwordResetRequest.AdminUsername) || string.IsNullOrEmpty(passwordResetRequest.AdminPassword))
            {
                return new PasswordResponse
                {
                    Success = false,
                    Result = "InvalidRequest"
                };
            }
            
            var result = userProvider.ResetPassword(passwordResetRequest.Username, passwordResetRequest.NewPassword, passwordResetRequest.AdminUsername, passwordResetRequest.AdminPassword);

            return new PasswordResponse
            {
                Success = result == PasswordResult.Success,
                Result = result.ToString()
            };
        }
    }
}