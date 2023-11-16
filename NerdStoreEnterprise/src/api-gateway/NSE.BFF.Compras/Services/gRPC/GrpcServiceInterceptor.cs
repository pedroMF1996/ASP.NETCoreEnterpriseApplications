using Grpc.Core;
using Grpc.Core.Interceptors;

namespace NSE.BFF.Compras.Services.gRPC
{
    public class GrpcServiceInterceptor : Interceptor
    {
        private readonly ILogger<GrpcServiceInterceptor> _logger;
        private readonly IHttpContextAccessor _contextAccessor;

        public GrpcServiceInterceptor(ILogger<GrpcServiceInterceptor> logger, 
                                      IHttpContextAccessor contextAccessor)
        {
            _logger = logger;
            _contextAccessor = contextAccessor;
        }

        public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(TRequest request, 
                                                                                      ClientInterceptorContext<TRequest, TResponse> context, 
                                                                                      AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
        {
            var token = _contextAccessor.HttpContext.Request.Headers["Authorization"];

            var headers = new Metadata()
            {
                { "Authorization", token }
            };

            var options = context.Options.WithHeaders(headers);

            context = new ClientInterceptorContext<TRequest, TResponse>(context.Method, context.Host, options);

            return base.AsyncUnaryCall(request, context, continuation);
        }
    }
}
