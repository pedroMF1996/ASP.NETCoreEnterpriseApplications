using Microsoft.AspNetCore.Mvc;
using NSE.WebApp.MVC.Models;

namespace NSE.WebApp.MVC.Controllers
{
    public class MainController : Controller
    {
        public bool ResponsePossuiErros(ResponseResult result)
        {
            if(result != null && result.Errors.Mensagens.Any())
            {
                foreach(var mensagem in result.Errors.Mensagens)
                {
                    ModelState.AddModelError(string.Empty, mensagem);
                }

                return true;
            }

            return false;
        }
    }
}
