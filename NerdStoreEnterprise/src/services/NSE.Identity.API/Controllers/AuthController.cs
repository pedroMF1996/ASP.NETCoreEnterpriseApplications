﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NSE.Identity.API.Extensions;
using NSE.Identity.API.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NSE.Identity.API.Controllers
{
    [Route("api/identidade")]
    public class AuthController : MainController
    {
        private readonly SignInManager<IdentityUser> _signInManager;

        private readonly UserManager<IdentityUser> _userManager;

        private readonly AppSettings _appSettings;

        public AuthController(SignInManager<IdentityUser> signInManager,
                              UserManager<IdentityUser> userManager,
                              IOptions<AppSettings> appSettings)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _appSettings = appSettings.Value;
        }

        [HttpPost("nova-conta")]
        public async Task<IActionResult> Registrar(RegisterUserViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }

            var user = new IdentityUser()
            {
                UserName = model.Email,
                Email = model.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return CustomResponse(await GerarJWT(model.Email));
            }

            foreach(var error in result.Errors)
            {
                AdicionarErroProcessamento(error.Description);
            }

            return CustomResponse();
        }

        [HttpPost("autenticar")]
        public async Task<IActionResult> Login(LoginUserViewModel model)
        {
            if(!ModelState.IsValid) { return CustomResponse(ModelState); }

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, true);

            if (result.Succeeded)
            {
                return CustomResponse(await GerarJWT(model.Email));
            }

            if(result.IsLockedOut) {
                AdicionarErroProcessamento("Usuario temporariamente bloqueado por tentativas invalidas");
                return CustomResponse();
            }

            AdicionarErroProcessamento("Usuario ou senha incorretos");

            return CustomResponse();
        }

        private async Task<LoginResponseViewModel> GerarJWT(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            var claims = await _userManager.GetClaimsAsync(user);

            var roles = await _userManager.GetRolesAsync(user);

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString())); //quando vai espirar 
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64)); //quando foi emitido

            foreach(var role in roles)
            {
                claims.Add(new Claim("role", role));
            }

            var identityClaims = new ClaimsIdentity(claims);

            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor()
            {
                Issuer = _appSettings.Emissor,
                Audience = _appSettings.ValidoEm,
                Subject = identityClaims,
                Expires = DateTime.UtcNow.AddHours(_appSettings.ExpiracaoHoras),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            var encodedToken = tokenHandler.WriteToken(token);

            var response = new LoginResponseViewModel()
            {
                AccessToken = encodedToken,
                ExpiresIn = TimeSpan.FromHours(_appSettings.ExpiracaoHoras).TotalSeconds,
                UserToken = new UserTokenViewModel()
                {
                    Id = user.Id,
                    Email = user.Email,
                    Claims = claims.Select(c => new ClaimViewModel() { Type = c.Type, Value = c.Value})
                }
            };

            return response;
        }

        private static long ToUnixEpochDate(DateTime utcNow)
        {
            return (long)Math.Round((utcNow.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
        }
    }
}
