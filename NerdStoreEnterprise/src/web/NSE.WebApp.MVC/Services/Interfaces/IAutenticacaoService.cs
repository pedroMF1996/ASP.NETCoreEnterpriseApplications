using NSE.WebApp.MVC.Models;

namespace NSE.WebApp.MVC.Services.Interfaces
{
    public interface IAutenticacaoService
    {
        Task<LoginResponseViewModel> Login(LoginUserViewModel usuarioLogin);
        Task<LoginResponseViewModel> Registro(RegisterUserViewModel usuarioRegistro);
    }
}
