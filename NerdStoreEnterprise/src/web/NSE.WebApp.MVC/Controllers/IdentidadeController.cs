using Microsoft.AspNetCore.Mvc;
using NSE.WebApp.MVC.Models;

namespace NSE.WebApp.MVC.Controllers
{
    public class IdentidadeController : Controller
    {
        [HttpGet]
        [Route("nova-conta")]
        public IActionResult Registro() {
            ViewBag.Title = "Nova conta";
            return View(); 
        }
        
        [HttpPost]
        [Route("nova-conta")]
        public async Task<IActionResult> Registro(RegisterUserViewModel usuarioRegistro) { 
            if(!ModelState.IsValid) return View(usuarioRegistro);

            //API - Registro

            
            
            if(false) return View(usuarioRegistro);

            //Realizar login na app
            return RedirectToAction("Index", controllerName: "Home");
        }

        [HttpGet]
        [Route("login")]
        public IActionResult Login()
        {
            ViewBag.Title = "Login";
            return View();
        }
        
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginUserViewModel usarioLogin)
        {
            if (!ModelState.IsValid) return View(usarioLogin);

            //API - Registro



            if (false) return View(usarioLogin);

            //Realizar login na app
            return RedirectToAction("Index", controllerName: "Home");
        }

        [HttpGet]
        [Route("sair")]
        public async Task<IActionResult> Logout()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}
