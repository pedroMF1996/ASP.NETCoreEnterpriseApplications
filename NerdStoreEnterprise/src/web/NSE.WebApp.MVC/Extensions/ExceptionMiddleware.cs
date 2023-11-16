using Grpc.Core;
using NSE.WebApp.MVC.Services.Interfaces;
using Polly.CircuitBreaker;
using Refit;
using System.Net;

namespace NSE.WebApp.MVC.Extensions
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private static IAutenticacaoService _autenticacaoService;
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, IAutenticacaoService autenticacaoService)
        {
            try
            {
                _autenticacaoService = autenticacaoService;
                await _next(httpContext);
            }
            catch (CustomHttpRequestException ex)
            {
                HandleRequestExceptionAsync(httpContext, ex.StatusCode);
            }
            catch (ValidationApiException ex)
            {
                HandleRequestExceptionAsync(httpContext, ex.StatusCode);
            }
            catch (ApiException ex)
            {
                HandleRequestExceptionAsync(httpContext, ex.StatusCode);
            }
            catch (BrokenCircuitException)
            {
                HandleCirquitBreakerException(httpContext);
            }
            catch (RpcException ex)
            {
                var statusCode = ex.StatusCode switch
                {
                    StatusCode.Internal => 400,
                    StatusCode.Unauthenticated => 401,
                    StatusCode.PermissionDenied => 403,
                    StatusCode.Unimplemented => 404,
                    _ => 500
                };

                var httpStatusCode = (HttpStatusCode)System.Enum.Parse(typeof(HttpStatusCode), statusCode.ToString());

                HandleRequestExceptionAsync(httpContext, httpStatusCode);
            }
        }

        private void HandleRequestExceptionAsync(HttpContext context, HttpStatusCode statusCode)
        {
            if (statusCode == HttpStatusCode.Unauthorized)
            {
                if (_autenticacaoService.TokenExpirado() && _autenticacaoService.RefreshTokenValido().Result)
                {
                    context.Response.Redirect(context.Request.Path);
                    return;
                }

                _autenticacaoService.Logout();

                context.Response.Redirect($"/login?ReturnUrl={context.Request.Path}");
                return;
            }

            context.Response.StatusCode = (int)statusCode;
        }

        private void HandleCirquitBreakerException(HttpContext httpContext)
        {
            httpContext.Response.Redirect("/sistema-indisponivel");
        }
    }
}
