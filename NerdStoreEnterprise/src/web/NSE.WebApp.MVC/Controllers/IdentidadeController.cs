using Microsoft.AspNetCore.Mvc;
using NSE.WebApp.MVC.Models;
using NSE.WebApp.MVC.Services.Interfaces;

namespace NSE.WebApp.MVC.Controllers
{
    public class IdentidadeController : MainController
    {
        private readonly IAutenticacaoService _autenticationService;

        public IdentidadeController(IAutenticacaoService autenticationService)
        {
            _autenticationService = autenticationService;
        }

        [HttpGet]
        [Route("nova-conta")]
        public IActionResult Registro()
        {
            ViewData["Title"] = "Nova conta";
            return View();
        }

        [HttpPost]
        [Route("nova-conta")]
        public async Task<IActionResult> Registro(RegisterUserViewModel usuarioRegistro)
        {
            if (!ModelState.IsValid) return View(usuarioRegistro);

            var resposta = await _autenticationService.Registro(usuarioRegistro);

            if (ResponsePossuiErros(resposta.ResponseResult)) return View(usuarioRegistro);

            await _autenticationService.RealizarLogin(resposta);

            return RedirectToAction("Index", controllerName: "Catalogo");
        }

        [HttpGet]
        [Route("login")]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["Title"] = "Login";
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginUserViewModel usuarioLogin, string? returnUrl = null)
        {
            if (string.IsNullOrEmpty(ViewData["ReturnUrl"]?.ToString()))
                ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid) return View(usuarioLogin);

            var resposta = await _autenticationService.Login(usuarioLogin);

            if (ResponsePossuiErros(resposta.ResponseResult)) return View(usuarioLogin);

            await _autenticationService.RealizarLogin(resposta);

            if (string.IsNullOrEmpty(returnUrl)) return RedirectToAction("Index", controllerName: "Catalogo");

            return LocalRedirect(returnUrl);
        }

        [HttpGet]
        [Route("sair")]
        public async Task<IActionResult> Logout()
        {
            await _autenticationService.Logout();
            return RedirectToAction("Index", "Catalogo");
        }
    }
}
