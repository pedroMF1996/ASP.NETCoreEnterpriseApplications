using Microsoft.AspNetCore.Mvc;
using NSE.WebApp.MVC.Models;

namespace NSE.WebApp.MVC.Controllers
{
    public class CarrinhoController : Controller
    {
        [Route("carrinho")]
        public IActionResult Index()
        {
            return View();
        }
        
        [Route("carrinho/adicionar-item")]
        public IActionResult AdicionarItemCarrinho(ItemCarrinhoViewModel itemCarrinho)
        {
            return RedirectToAction("Index");
        }

        [Route("carrinho/atualizar-item")]
        public IActionResult AtualizarItemCarrinho(Guid itemCarrinhoId, int quantidade)
        {
            return RedirectToAction("Index");
        }

        [Route("carrinho/remover-item")]
        public IActionResult RemoverItemCarrinho(Guid itemCarrinhoId)
        {
            return RedirectToAction("Index");
        }
    }
}
