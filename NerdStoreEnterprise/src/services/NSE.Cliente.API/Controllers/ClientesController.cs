using Microsoft.AspNetCore.Mvc;
using NSE.Cliente.API.Application.Commands;
using NSE.Core.MediatR;
using NSE.WebAPI.Core.Controllers;

namespace NSE.Cliente.API.Controllers
{
    [Route("api/[controller]")]
    public class ClientesController : MainController
    {
        private readonly IMediatorHandler _mediatorHandler;

        public ClientesController(IMediatorHandler mediatorHandler)
        {
            _mediatorHandler = mediatorHandler;
        }

        [HttpGet("clientes")]
        public async Task<IActionResult> Index()
        {
            var resultado = await _mediatorHandler.EnviarComando(
                new RegistrarClienteCommand(Guid.NewGuid(), "Teste", "email@email.com", "47560841848"));

            if(!resultado.IsValid)
            {
                return CustomResponse(resultado);
            }

            return CustomResponse(resultado);
        }
    }
}
