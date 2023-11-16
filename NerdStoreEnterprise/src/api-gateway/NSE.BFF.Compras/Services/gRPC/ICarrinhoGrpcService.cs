using NSE.BFF.Compras.Models;

namespace NSE.BFF.Compras.Services.gRPC
{
    public interface ICarrinhoGrpcService
    {
        Task<CarrinhoDTO> ObterCarrinho();
    }
}