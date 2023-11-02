using NSE.Core.Messages.Integration;
using NSE.Pagamento.API.Models;

namespace NSE.Pagamento.API.Facade
{
    public interface IPagamentoFacace
    {
        Task<Transacao> AutorizarPagamento(Models.Pagamento pagamento);

        Task<Transacao> CapturarPagamento(Transacao transacao);

        Task<Transacao> CancelarPagamento(Transacao transacao);
        Task<Transacao> CancelarAutorizacao(object transacaoAutorizada);
    }
}
