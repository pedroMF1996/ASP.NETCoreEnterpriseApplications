using NSE.Core.Communication;
using NSE.WebApp.MVC.Models;

namespace NSE.WebApp.MVC.Services.Interfaces
{
    public interface IComprasBffService
    {
        Task<CarrinhoViewModel> ObterCarrinho();
        Task<int> ObterQuantidadeCarrinho();
        Task<ResponseResult> AdicionarItemCarrinho(ItemCarrinhoViewModel item);
        Task<ResponseResult> AtualizarIemCarrinho(Guid idItemCarrinho, ItemCarrinhoViewModel itemCarrinho);
        Task<ResponseResult> RemoverItemCarrinho(Guid idItemCarrinho);
    }
}
