using Microsoft.AspNetCore.Mvc;
using NSE.WebApp.MVC.Models;
using System.Diagnostics;

namespace NSE.WebApp.MVC.Controllers
{
    public class HomeController : Controller
    {
        

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Route("erro/{id:length(3,3)}")]
        public IActionResult Error(int id)
        {
            var modelError = new ErrorViewModel();

            if (id == 500)
            {
                modelError.Mensagem = "Ocorreu um erro; Tente novamente mais tarde ou contate nosso suporte";
                modelError.Titulo = "Ocorreu um erro!";
                modelError.ErroCode = id;
            }
            else if (id == 404)
            {
                modelError.Mensagem = "A pagina que esta procurando nao existe! <br /> Em caso de duvidas entre em contato com nosso suporte";
                modelError.Titulo = "Ops! Pagina nao encontrada.";
                modelError.ErroCode = id;
            }
            else if (id == 403)
            {
                modelError.Mensagem = "Voce nao tem permissao para fazer isso";
                modelError.Titulo = "Acesso negado!";
                modelError.ErroCode = id;
            }
            else
                return StatusCode(404);

            return View("Error", modelError);
        }
    }
}