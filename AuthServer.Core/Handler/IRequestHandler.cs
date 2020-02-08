using AuthServer.Core.Request;
using AuthServer.Core.Response;

namespace AuthServer.Core.Handler
{
    public interface IRequestHandler
    {
        bool CanHandle(IRequest request);

        IResponse Handle(IRequest request);
    }
}
