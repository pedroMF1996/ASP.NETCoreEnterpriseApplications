using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using NSE.WebApp.MVC.Models;
using NSE.WebApp.MVC.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace NSE.WebApp.MVC.Controllers
{
    public class IdentidadeController : MainController
    {
        private readonly IAutenticacaoService _autenticationService;

        public IdentidadeController(IAutenticacaoService autenticationService)
        {
            _autenticationService = autenticationService;
        }

        [HttpGet]
        [Route("nova-conta")]
        public IActionResult Registro() {
            ViewBag.Title = "Nova conta";
            return View(); 
        }
        
        [HttpPost]
        [Route("nova-conta")]
        public async Task<IActionResult> Registro(RegisterUserViewModel usuarioRegistro) { 
            if(!ModelState.IsValid) return View(usuarioRegistro);

            var resposta = await _autenticationService.Registro(usuarioRegistro);

            if (ResponsePossuiErros(resposta.ResponseResult)) return View(usuarioRegistro);

            await RealizarLogin(resposta);

            return RedirectToAction("Index", controllerName: "Home");
        }

        [HttpGet]
        [Route("login")]
        public IActionResult Login()
        {
            ViewBag.Title = "Login";
            return View();
        }
        
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginUserViewModel usuarioLogin)
        {
            if (!ModelState.IsValid) return View(usuarioLogin);

            var resposta = await _autenticationService.Login(usuarioLogin);

            if (ResponsePossuiErros(resposta.ResponseResult)) return View(usuarioLogin);
            
            await RealizarLogin(resposta);

            return RedirectToAction("Index", controllerName: "Home");
        }

        [HttpGet]
        [Route("sair")]
        public async Task<IActionResult> Logout()
        {
            return RedirectToAction("Index", "Home");
        }

        private async Task RealizarLogin(LoginResponseViewModel usuarioLogin)
        {
            var token = ObterTokenFormatado(usuarioLogin.AccessToken);

            var claims = new List<Claim>();

            claims.Add(new Claim("jwt", usuarioLogin.AccessToken));
            claims.AddRange(token.Claims);

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties()
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60),
                IsPersistent = true,
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, 
                new ClaimsPrincipal(claimsIdentity), 
                authProperties);
        }

        private static JwtSecurityToken ObterTokenFormatado(string jwtToken)
        {
            return new JwtSecurityTokenHandler().ReadToken(jwtToken) as JwtSecurityToken;
        }
    }
}
