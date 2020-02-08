using AuthServer.Core.Request;
using AuthServer.Core.Response;
using System.Collections.Generic;

namespace AuthServer.Core.Handler
{
    public class ProtocolHandler : IProtocolHandler
    {
        private IEnumerable<IRequestHandler> handlers;

        public ProtocolHandler(IEnumerable<IRequestHandler> handlers)
        {
            this.handlers = handlers;
        }


        public IResponse Handle(IRequest request)
        {
            foreach (var handler in handlers)
            {
                if (handler.CanHandle(request))
                {
                    return handler.Handle(request);
                }
            }

            throw new InvalidRequestException();
        }
    }
}
