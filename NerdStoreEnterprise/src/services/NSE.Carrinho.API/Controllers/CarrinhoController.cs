using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSE.Carrinho.API.Data;
using NSE.Carrinho.API.Models;
using NSE.WebAPI.Core.Controllers;
using NSE.WebAPI.Core.Usuario;

namespace NSE.Carrinho.API.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class CarrinhoController : MainController
    {
        private readonly CarrinhoContext _carrinhoContext;
        private readonly IAspNetUser _aspNetUser;

        public CarrinhoController(CarrinhoContext carrinhoContext, IAspNetUser aspNetUser)
        {
            _carrinhoContext = carrinhoContext;
            _aspNetUser = aspNetUser;
        }

        [HttpGet("carrinho")]
        public async Task<CarrinhoCliente> ObterCarrinho()
        {
            return await ObterCarrinhoCliente() ?? new CarrinhoCliente();
        }

        [HttpPost("carrinho")]
        public async Task<IActionResult> AdicionarItemCarrinho(CarrinhoItem carrinhoItem)
        {
            var carrinho = await ObterCarrinhoCliente();
            
            if(carrinho == null)
                await ManipularNovoCarrinho(carrinhoItem);
            else
                await ManipularCarrinhoExistente(carrinho, carrinhoItem);

            ValidarCarrinho(carrinho);
            if(!OperacaoValida()) return CustomResponse();

            await PersistirDados();

            return CustomResponse();
        }

        [HttpPut("carrinho/{produtoId}")]
        public async Task<IActionResult> AtualizarItemCarrinho(Guid produtoId, CarrinhoItem item)
        {
            var carrinho = await ObterCarrinhoCliente();
            if(carrinho == null)
            {
                AdicionarErroProcessamento("Erro ao encontrar o carrinho no usuario");
                return CustomResponse();
            }

            var itemValidado = await ObterItemCarrinhoValidado(produtoId, carrinho, item);
            if(itemValidado == null) return CustomResponse();
            
            carrinho.AtualizarUnidades(itemValidado, item.Quantidade);

            ValidarCarrinho(carrinho);
            if (!OperacaoValida()) return CustomResponse();

            _carrinhoContext.CarrinhoCliente.Update(carrinho);
            _carrinhoContext.CarrinhoItems.Update(itemValidado);

            await PersistirDados();

            return CustomResponse();
        }

        [HttpDelete("carrinho/{produtoId}")]
        public async Task<IActionResult> RemoverItemCarrinho(Guid produtoId)
        {
            var carrinho = await ObterCarrinhoCliente();
            var itemCarrinho = await ObterItemCarrinhoValidado(produtoId, carrinho);
            if(itemCarrinho == null) return CustomResponse();

            carrinho.RemoverItem(itemCarrinho);

            ValidarCarrinho(carrinho);
            if (!OperacaoValida()) return CustomResponse();

            _carrinhoContext.CarrinhoItems.Remove(itemCarrinho);
            _carrinhoContext.CarrinhoCliente.Update(carrinho);

            await PersistirDados();

            return CustomResponse();
        }

        private async Task<CarrinhoCliente> ObterCarrinhoCliente()
        {
            return await _carrinhoContext.CarrinhoCliente
                .Include(c => c.Itens)
                .FirstOrDefaultAsync(c => c.ClienteId == _aspNetUser.ObterUserId());
        }

        private async Task ManipularCarrinhoExistente(CarrinhoCliente carrinho, CarrinhoItem item)
        {
            var produtoItemExistente = carrinho.CarrinhoItemExistente(item);

            carrinho.AdicionarItem(item);

            if (produtoItemExistente)
                _carrinhoContext.CarrinhoItems.Update(carrinho.ObterPorProdutoId(item.ProdutoId));
            else
                await _carrinhoContext.CarrinhoItems.AddAsync(item);

            _carrinhoContext.CarrinhoCliente.Update(carrinho);
        }

        private async Task ManipularNovoCarrinho(CarrinhoItem item)
        {
            var carrinho = new CarrinhoCliente(_aspNetUser.ObterUserId());

            carrinho.AdicionarItem(item);

            await _carrinhoContext.CarrinhoCliente.AddAsync(carrinho);
        }

        private async Task<CarrinhoItem?> ObterItemCarrinhoValidado(Guid produtoId, CarrinhoCliente carrinho, CarrinhoItem? item = null)
        {
            if (item != null && produtoId != item.ProdutoId)
            {
                AdicionarErroProcessamento("O item nao corresponde ao informado");
                return null;
            }

            if(carrinho == null)
            {
                AdicionarErroProcessamento("Carrinho nao encontrado");
                return null;
            }

            var itemCarrinho = await _carrinhoContext.CarrinhoItems
                .FirstOrDefaultAsync(i => i.CarrinhoId == carrinho.Id && i.ProdutoId == produtoId);

            if(itemCarrinho == null || !carrinho.CarrinhoItemExistente(itemCarrinho))
            {
                AdicionarErroProcessamento("O Item nao esta no carrinho");
                return null;
            }

            return itemCarrinho;
        }

        private async Task PersistirDados()
        {
            var result = await _carrinhoContext.SaveChangesAsync();
            if (result <= 0) AdicionarErroProcessamento("Nao foi possivel persistir os dados no banco");
        }

        private bool ValidarCarrinho(CarrinhoCliente carrinho)
        {
            if (carrinho.EhValido()) return true;
            
            AdicionarErrosProcessamento(carrinho.ValidationResult.Errors);
            return false;
        }
    }
}
