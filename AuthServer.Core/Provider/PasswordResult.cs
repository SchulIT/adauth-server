namespace AuthServer.Core.Provider
{
    public enum PasswordResult
    {
        /// <summary>
        /// Something went wrong
        /// </summary>
        UnknownError,

        /// <summary>
        /// Password change/reset was successfull
        /// </summary>
        Success,

        /// <summary>
        /// Provided credentials (either old user password or admin credentials)
        /// are not valid
        /// </summary>
        InvalidCredentials,

        /// <summary>
        /// User was not found (only when resetting passwords)
        /// </summary>
        UserNotFound,

        /// <summary>
        /// User must change the password before
        /// </summary>
        MustChangePassword
    }
}
