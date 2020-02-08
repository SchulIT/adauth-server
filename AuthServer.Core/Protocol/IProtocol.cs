using System.Threading.Tasks;

namespace AuthServer.Core.Protocol
{
    /// <summary>
    /// Interface for all possible request handlers (currently, only one handler is available)
    /// </summary>
    public interface IProtocol
    {
        /// <summary>
        /// Checks whether the given content is a valid request.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        Task<bool> CanHandleAsync(string content);

        /// <summary>
        /// Handles the given content and returns a response which can be sent to the client.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        Task<string> HandleAsync(string content);
    }
}
