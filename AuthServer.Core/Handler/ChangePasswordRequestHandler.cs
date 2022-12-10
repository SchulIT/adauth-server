using AuthServer.Core.Provider;
using AuthServer.Core.Request;
using AuthServer.Core.Response;
using System;

namespace AuthServer.Core.Handler
{
    public class ChangePasswordRequestHandler : IRequestHandler
    {
        private readonly IUserProvider userProvider;

        public ChangePasswordRequestHandler(IUserProvider userProvider)
        {
            this.userProvider = userProvider;
        }

        public bool CanHandle(IRequest request)
        {
            return request is ChangePasswordRequest;
        }

        public IResponse Handle(IRequest request)
        {
            var passwordResetRequest = request as ChangePasswordRequest;

            if (string.IsNullOrEmpty(passwordResetRequest.Username) || string.IsNullOrEmpty(passwordResetRequest.NewPassword) || string.IsNullOrEmpty(passwordResetRequest.OldPassword))
            {
                return new PasswordResponse
                {
                    Success = false,
                    Result = "InvalidRequest"
                };
            }

            var result = userProvider.ChangePassword(passwordResetRequest.Username, passwordResetRequest.OldPassword, passwordResetRequest.NewPassword);

            return new PasswordResponse
            {
                Success = result == PasswordResult.Success,
                Result = result.ToString()
            };
        }
    }
}