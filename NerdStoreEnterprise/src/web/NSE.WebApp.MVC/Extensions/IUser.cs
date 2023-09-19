using System.Security.Claims;

namespace NSE.WebApp.MVC.Extensions
{
    public interface IUser
    {
        string Name { get; }

        Guid ObterUserId();

        string ObterUserEmail();

        string ObterUserToken(string token);

        bool EstahAutenticado();

        bool PossuiRole(string role);

        IEnumerable<Claim> ObterClaims();

        HttpContext ObterHttpContext();
    }

    public class AspNetUser : IUser
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public AspNetUser(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public string Name => _contextAccessor.HttpContext.User.Identity.Name;

        public bool EstahAutenticado()
        {
            return _contextAccessor.HttpContext.User.Identity.IsAuthenticated;
        }

        public IEnumerable<Claim> ObterClaims()
        {
            return _contextAccessor.HttpContext.User.Claims;
        }

        public HttpContext ObterHttpContext()
        {
            return _contextAccessor.HttpContext;
        }

        public string ObterUserEmail()
        {
            return EstahAutenticado() ? _contextAccessor.HttpContext.User.GetUserEmail() : "";
        }

        public Guid ObterUserId()
        {
            return EstahAutenticado() ? _contextAccessor.HttpContext.User.GetUserId() : Guid.Empty;
        }

        public string ObterUserToken(string token)
        {
            return EstahAutenticado() ? _contextAccessor.HttpContext.User.GetUserToken() : "";
        }

        public bool PossuiRole(string role)
        {
            return _contextAccessor.HttpContext.User.IsInRole(role);
        }
    }

    public static class ClaimsPrincipalExtensions{
        public static Guid GetUserId(this ClaimsPrincipal principal)
        {
            if(principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }

            var claims = principal.FindFirst("sub");

            return Guid.Parse(claims?.Value);
        }

        public static string GetUserEmail(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }

            var claims = principal.FindFirst("email");

            return claims?.Value;
        }

        public static string GetUserToken(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }

            var claims = principal.FindFirst("jwt");

            return claims?.Value;
        }
    }
}
