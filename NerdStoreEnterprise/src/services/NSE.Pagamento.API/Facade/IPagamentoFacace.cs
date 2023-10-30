using NSE.Pagamento.API.Models;

namespace NSE.Pagamento.API.Facade
{
    public interface IPagamentoFacace
    {
        Task<Transacao> AutorizarPagamento(Models.Pagamento pagamento);
    }
}
