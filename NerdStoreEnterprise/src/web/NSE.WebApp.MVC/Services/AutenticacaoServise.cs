using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using NSE.Core.Communication;
using NSE.WebAPI.Core.Usuario;
using NSE.WebApp.MVC.Enum;
using NSE.WebApp.MVC.Extensions;
using NSE.WebApp.MVC.Models;
using NSE.WebApp.MVC.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace NSE.WebApp.MVC.Services
{
    public class AutenticacaoServise : Service, IAutenticacaoService
    {

        private readonly IAuthenticationService _authenticationService;
        private readonly IAspNetUser _aspNetUser;
        public AutenticacaoServise(HttpClient httpClient, IOptions<AppSettingsUrl> appSettings, 
                                   IAuthenticationService authenticationService = null, IAspNetUser aspNetUser = null)
            : base(httpClient, AppSettingsUrlEnum.Identidade, appSettings)
        {
            _authenticationService = authenticationService;
            _aspNetUser = aspNetUser;
        }

        public async Task<LoginResponseViewModel> Login(LoginUserViewModel usuarioLogin)
        {
            var loginContent = ObterConteudo(usuarioLogin);

            var response = await _httpClient.PostAsync("/api/identidade/autenticar", loginContent);

            if (!TratarErrosResponse(response))
            {
                return new LoginResponseViewModel()
                {
                    ResponseResult = await DeserializarObjetoResponse<ResponseResult>(response)
                };
            }

            return await DeserializarObjetoResponse<LoginResponseViewModel>(response);
        }

        public async Task<LoginResponseViewModel> Registro(RegisterUserViewModel usuarioRegistro)
        {
            var registroContent = ObterConteudo(usuarioRegistro);

            var response = await _httpClient.PostAsync("/api/identidade/nova-conta", registroContent);

            if (!TratarErrosResponse(response))
            {
                return new LoginResponseViewModel()
                {
                    ResponseResult = await DeserializarObjetoResponse<ResponseResult>(response)
                };
            }

            return await DeserializarObjetoResponse<LoginResponseViewModel>(response);
        }

        public async Task<LoginResponseViewModel> UtilizarRefreshToken(string refreshToken)
        {
            var refreshTokenContent = ObterConteudo(refreshToken);

            var response = await _httpClient.PostAsync("/api/identidade/refresh-token", refreshTokenContent);

            if (!TratarErrosResponse(response))
            {
                return new LoginResponseViewModel()
                {
                    ResponseResult = await DeserializarObjetoResponse<ResponseResult>(response)
                };
            }

            return await DeserializarObjetoResponse<LoginResponseViewModel>(response);
        }

        public static JwtSecurityToken ObterTokenFormatado(string jwtToken)
        {
            return new JwtSecurityTokenHandler().ReadToken(jwtToken) as JwtSecurityToken;
        }

        public async Task Logout()
        {
            await _authenticationService.SignOutAsync(_aspNetUser.ObterHttpContext(), CookieAuthenticationDefaults.AuthenticationScheme, null);
        }

        public async Task RealizarLogin(LoginResponseViewModel usuarioLogin)
        {
            var token = ObterTokenFormatado(usuarioLogin.AccessToken);

            var claims = new List<Claim>
            {
                new Claim("jwt", usuarioLogin.AccessToken),
                new Claim("RefreshToken", usuarioLogin.RefreshToken)
            };

            claims.AddRange(token.Claims);

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties()
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8),
                IsPersistent = true,
            };

            await _authenticationService.SignInAsync(
                _aspNetUser.ObterHttpContext(),
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }

        public bool TokenExpirado()
        {
            var jwt = _aspNetUser.ObterUserToken();
            if (jwt is null) return false;

            var token = ObterTokenFormatado(jwt);
            return token.ValidTo.ToLocalTime() < DateTime.Now;
        }

        public async Task<bool> RefreshTokenValido()
        {
            var response = await UtilizarRefreshToken(_aspNetUser.ObterUserRefreshToken());

            if (response.AccessToken is not null && response.ResponseResult is null) 
            {
                await RealizarLogin(response);
                return true;
            }

            return false;
        }
    }
}
