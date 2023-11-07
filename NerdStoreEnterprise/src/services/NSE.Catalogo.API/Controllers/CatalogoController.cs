using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSE.Catalogo.API.Models;
using NSE.WebAPI.Core.Controllers;
using NSE.WebAPI.Core.Identidade;

namespace NSE.Catalogo.API.Controllers
{

    [Authorize]
    public class CatalogoController : MainController
    {
        private readonly IProdutoRepository _produtoRepository;

        public CatalogoController(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        [AllowAnonymous]
        [HttpGet("catalogo/produtos")]
        public async Task<PagedResult<Produto>> Index([FromQuery] int ps = 8, [FromQuery] int page = 1, [FromQuery] string q = null)
        {
            return await _produtoRepository.ObterTodos(ps, page, q);
        }

        [ClaimsAuthorize("catalogo", "Ler")]
        [HttpGet("catalogo/produtos/{id}")]
        public async Task<Produto> ProdutoDetalhe(Guid id)
        {
            return await _produtoRepository.ObterPorId(id);
        }

        [HttpGet("catalogo/produtos/lista/{ids}")]
        public async Task<IEnumerable<Produto>> ObterProdutosPorId(string ids)
        {
            return await _produtoRepository.ObterProdutosPorId(ids);
        }
    }
}
