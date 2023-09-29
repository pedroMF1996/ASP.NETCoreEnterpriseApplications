using Microsoft.AspNetCore.Mvc;
using NSE.Carrinho.API.Data;
using NSE.Carrinho.API.Models;
using NSE.WebAPI.Core.Controllers;
using NSE.WebAPI.Core.Usuario;

namespace NSE.Carrinho.API.Controllers
{
    [Route("api/[controller]")]
    public class CarrinhoController : MainController
    {
        private readonly CarrinhoContext _carrinhoContext;
        private readonly IAspNetUser _aspNetUser;

        public CarrinhoController(CarrinhoContext carrinhoContext, IAspNetUser aspNetUser)
        {
            _carrinhoContext = carrinhoContext;
            _aspNetUser = aspNetUser;
        }

        [HttpGet]
        public async Task<IActionResult> ObterCarrinho()
        {

            return CustomResponse();
        }

        [HttpPost]
        public async Task<IActionResult> AdicionarItemCarrinho(CarrinhoItem carrinhoItem)
        {
            await _carrinhoContext.CarrinhoItems.AddAsync(carrinhoItem);
            return CustomResponse();
        }

        [HttpPut]
        public async Task<IActionResult> AtualizarItemCarrinho(Guid Id, CarrinhoItem item)
        {
            return CustomResponse();
        }

        [HttpDelete]
        public async Task<IActionResult> RemoverItemCarrinho(Guid produtoId)
        {

            
            return CustomResponse();
        }
    }
}
