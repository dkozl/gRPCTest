using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Logging;
using Workshop;

namespace gRPCService
{
    public class MessageInterceptor: Interceptor
    {
        private readonly ILogger<MessageInterceptor> _logger;

        public MessageInterceptor(ILogger<MessageInterceptor> logger)
        {
            _logger = logger;
        }
        public override Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
        {
            switch (request)
            {
                case HelloRequest helloRequest:
                    _logger.LogInformation($"'{context.Method}' method called for '{helloRequest.Name}'");
                    break;
            }
            return continuation(request, context);
        }
    }
}
