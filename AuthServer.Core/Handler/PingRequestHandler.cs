using AuthServer.Core.Request;
using AuthServer.Core.Response;

namespace AuthServer.Core.Handler
{
    class PingRequestHandler : IRequestHandler
    {
        public bool CanHandle(IRequest request)
        {
            return request is PingRequest;
        }

        public IResponse Handle(IRequest request)
        {
            return new PongResponse();
        }
    }
}
