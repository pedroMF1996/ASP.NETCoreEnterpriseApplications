using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NSE.Core.Messages.Integration;
using NSE.Identity.API.Models;
using NSE.Identity.API.Services;
using NSE.MessageBus;
using NSE.WebAPI.Core.Controllers;
using System.Security.Claims;

namespace NSE.Identity.API.Controllers
{
    [Route("api/identidade")]
    public class AuthController : MainController
    {
        private readonly SignInManager<IdentityUser> _signInManager;

        private readonly UserManager<IdentityUser> _userManager;

        private readonly IMessageBus _bus;


        private readonly AuthenticationService _authenticationService;

        public AuthController(SignInManager<IdentityUser> signInManager,
                              UserManager<IdentityUser> userManager,
                              IMessageBus bus)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _bus = bus;
        }

        [HttpPost("nova-conta")]
        public async Task<IActionResult> Registrar(RegisterUserViewModel model)
        {
            if (!ModelState.IsValid)
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
                var clienteResult = await RegistrarCliente(model);

                if (!clienteResult.ValidationResult.IsValid)
                {
                    await _userManager.DeleteAsync(user);
                    return CustomResponse(clienteResult.ValidationResult);
                }

                await _userManager.AddClaimAsync(user, new Claim("catalogo", "Ler"));

                return CustomResponse(await _authenticationService.GerarJwt(model.Email));
            }

            foreach (var error in result.Errors)
            {
                AdicionarErroProcessamento(error.Description);
            }

            return CustomResponse();
        }

        [HttpPost("autenticar")]
        public async Task<IActionResult> Login(LoginUserViewModel model)
        {

            if (!ModelState.IsValid) { return CustomResponse(ModelState); }

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, true);

            if (result.Succeeded)
            {
                return CustomResponse(await _authenticationService.GerarJwt(model.Email));
            }

            if (result.IsLockedOut)
            {
                AdicionarErroProcessamento("Usuario temporariamente bloqueado por tentativas invalidas");
                return CustomResponse();
            }

            AdicionarErroProcessamento("Usuario ou senha incorretos");

            return CustomResponse();
        }


        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                AdicionarErroProcessamento("Refresh token invalido");
                return CustomResponse();
            }

            var token = await _authenticationService.ObterRefreshToken(Guid.Parse(refreshToken));

            if(token is null)
            {
                AdicionarErroProcessamento("Refresh Token expirado");
                return CustomResponse();
            }

            return CustomResponse(await _authenticationService.GerarJwt(token.Username));
        }

        private async Task<ResponseMessage> RegistrarCliente(RegisterUserViewModel usuarioRegistro)
        {
            var usuario = await _userManager.FindByEmailAsync(usuarioRegistro.Email);

            var usuarioRegistrado = new UsuarioRegistradoIntegrationEvent(
                Guid.Parse(usuario.Id), usuarioRegistro.Nome, usuarioRegistro.Email, usuarioRegistro.Cpf);

            try
            {
                return await _bus.RequestAsync<UsuarioRegistradoIntegrationEvent, ResponseMessage>(usuarioRegistrado);
            }
            catch
            {
                await _userManager.DeleteAsync(usuario);
                throw;
            }
        }
    }
}
