using Microsoft.AspNetCore.Mvc;
using NSE.WebApp.MVC.Models;
using NSE.WebApp.MVC.Services;

namespace NSE.WebApp.MVC.Controllers
{
    public class IdentidadeController : Controller
    {
        private readonly IAutenticacaoService _autenticationService;

        public IdentidadeController(IAutenticacaoService autenticationService)
        {
            _autenticationService = autenticationService;
        }

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

            var resposta = await _autenticationService.Registro(usuarioRegistro);

            if (false) return View(usuarioRegistro);

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
        public async Task<IActionResult> Login(LoginUserViewModel usuarioLogin)
        {
            if (!ModelState.IsValid) return View(usuarioLogin);

            //API - Registro

            var resposta = await _autenticationService.Login(usuarioLogin);



            if (false) return View(usuarioLogin);

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
