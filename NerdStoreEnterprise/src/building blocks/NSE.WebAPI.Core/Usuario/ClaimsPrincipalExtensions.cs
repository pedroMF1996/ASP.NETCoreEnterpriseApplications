using System.Security.Claims;

namespace NSE.WebAPI.Core.Usuario
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }

            var claims = principal.FindFirst("sub")
                ?? principal.FindFirst(ClaimTypes.NameIdentifier);

            return Guid.Parse(claims?.Value);
        }

        public static string GetUserEmail(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }

            var claims = principal.FindFirst("email")
                ?? principal.FindFirst(ClaimTypes.Email);

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
        
        public static string GetUserRefreshToken(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }

            var claims = principal.FindFirst("RefreshToken");

            return claims?.Value;
        }
    }
}
