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
                return true;
            }

            return false;
        }
    }
}
