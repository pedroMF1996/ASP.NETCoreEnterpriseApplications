using NSE.WebAPI.Core.Usuario;
using NSE.WebApp.MVC.Extensions;
using System.Net.Http.Headers;

namespace NSE.WebApp.MVC.Services.Handlers
{
    public class HttpCientAuthorizationDelegateHandler : DelegatingHandler  
    {
        private readonly IAspNetUser _user;

        public HttpCientAuthorizationDelegateHandler(IAspNetUser user)
        {
            _user = user;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var authorizationHeader = _user.ObterHttpContext().Request.Headers["Authorization"];

            if(!string.IsNullOrEmpty(authorizationHeader))
            {
                request.Headers.Add("Authorization", new List<string>() { authorizationHeader });
            }

            var token = _user.ObterUserToken();

            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
