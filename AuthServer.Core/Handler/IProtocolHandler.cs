using AuthServer.Core.Request;
using AuthServer.Core.Response;

namespace AuthServer.Core.Handler
{
    /// <summary>
    /// Handles incoming requests and answers with a response.
    /// </summary>
    public interface IProtocolHandler
    {
        IResponse Handle(IRequest request);
    }
}
