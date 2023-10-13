using NSE.Core.Communication;
using NSE.WebApp.MVC.Models;

namespace NSE.WebApp.MVC.Services.Interfaces
{
    public interface IClienteService
    {
        Task<EnderecoViewModel> ObterEndereco();
        Task<ResponseResult> AdicionarEndereco(EnderecoViewModel endereco);
    }
}
