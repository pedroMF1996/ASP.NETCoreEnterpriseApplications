using NSE.Core.Messages.Integration;

namespace NSE.Pagamento.API.Services
{
    public interface IPagamentoService
    {
        Task<ResponseMessage> AutorizarPagamento(Models.Pagamento pagamento);
        Task<ResponseMessage>  CancelarPagamento(Guid id);
        Task<ResponseMessage> CapturarPagamento(Guid pedidoId);
    }
}
