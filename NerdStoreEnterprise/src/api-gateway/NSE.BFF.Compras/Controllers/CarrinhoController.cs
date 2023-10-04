using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSE.WebAPI.Core.Controllers;
using NSE.WebAPI.Core.Usuario;

namespace NSE.BFF.Compras.Controllers
{
    [Authorize]
    [Route("compras")]
    public class CarrinhoController : MainController
    {
        private readonly IAspNetUser _aspNetUser;

        public CarrinhoController( IAspNetUser aspNetUser)
        {
            
            _aspNetUser = aspNetUser;
        }

        [HttpGet("carrinho")]
        public async Task<IActionResult> ObterCarrinho()
        {
            return CustomResponse();
        }
        
        [HttpGet("carrinho-quantidade")]
        public async Task<IActionResult> ObterQauntidadeCarrinho()
        {
            return CustomResponse();
        }

        [HttpPost("carrinho/items")]
        public async Task<IActionResult> AdicionarItemCarrinho()
        {
            return CustomResponse();
        }

        [HttpPut("carrinho/items/{produtoId}")]
        public async Task<IActionResult> AtualizarItemCarrinho()
        {
            return CustomResponse();
        }

        [HttpDelete("carrinho/items/{produtoId}")]
        public async Task<IActionResult> RemoverItemCarrinho(Guid produtoId)
        {
            return CustomResponse();
        }         
    }
}
