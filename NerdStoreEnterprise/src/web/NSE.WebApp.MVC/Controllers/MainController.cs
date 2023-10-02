using Microsoft.AspNetCore.Mvc;
using NSE.WebApp.MVC.Models;

namespace NSE.WebApp.MVC.Controllers
{
    public abstract class MainController : Controller
    {
        protected bool ResponsePossuiErros(ResponseResult result)
        {
            if (result != null && result.Errors.Mensagens.Any())
            {
                foreach (var mensagem in result.Errors.Mensagens)
                {
                    ModelState.AddModelError(string.Empty, mensagem);
                }

                return true;
            }

            return false;
        }

        protected void AdicionarErroValidacao(string erro)
        {
            ModelState.AddModelError(string.Empty, erro);
        }

        protected bool OperacaoValida()
        {
            return ModelState.ErrorCount == 0;
        }
    }
}
