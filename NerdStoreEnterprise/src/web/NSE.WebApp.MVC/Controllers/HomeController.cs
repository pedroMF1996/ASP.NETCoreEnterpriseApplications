﻿using Microsoft.AspNetCore.Mvc;
using NSE.WebApp.MVC.Models;

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

        //Sistema Indisponivel
        [Route("sistema-indisponivel")]
        public IActionResult SistemaIndisponivel()
        {
            var modelError = new ErrorViewModel()
            {
                Mensagem = "O sistema esta temporariamente indisponivel, isso pode ocorrer em momentos de sobre carga de usuarios.",
                Titulo = "Sistema Indisponivel",
                ErroCode = 500
            };

            return View("Error", modelError);
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