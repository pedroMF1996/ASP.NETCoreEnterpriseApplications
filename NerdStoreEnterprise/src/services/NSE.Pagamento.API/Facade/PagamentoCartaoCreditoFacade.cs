using Microsoft.Extensions.Options;
using NSE.Pagamento.API.Models;
using NSE.Pagamentos.NerdsPag;

namespace NSE.Pagamento.API.Facade
{
    public class PagamentoCartaoCreditoFacade : IPagamentoFacace
    {
        private readonly PagamentoConfig _config;

        public PagamentoCartaoCreditoFacade(IOptions<PagamentoConfig> config)
        {
            _config = config.Value;
        }

        public async Task<Transacao> AutorizarPagamento(Models.Pagamento pagamento)
        {
            NerdsPagService nerdsPag = new(_config.DefaultApiKey, _config.DefaultEncriptionKey);

            CardHash cardHashGen = new(nerdsPag)
            {
                CardNumber = pagamento.CartaoCredito.NumeroCartao,
                CardHolderName = pagamento.CartaoCredito.NomeCartao,
                CardExpirationDate = pagamento.CartaoCredito.MesAnoVencimento,
                CardCvv = pagamento.CartaoCredito.CVV
            };

            var cardHash = cardHashGen.Generate();

            Transaction transacao = new(nerdsPag)
            {
                CardHash = cardHash,
                CardNumber = pagamento.CartaoCredito.NumeroCartao,
                CardHolderName = pagamento.CartaoCredito.NomeCartao,
                CardExpirationDate = pagamento.CartaoCredito.MesAnoVencimento,
                CardCvv = pagamento.CartaoCredito.CVV,
                PaymentMethod = PaymentMethod.CreditCard,
                Amount = pagamento.Valor
            };

            return ParaTransacao(await transacao.AuthorizeCardTransaction());
        }

        public async Task<Transacao> CapturarPagamento(Transacao transacao)
        {
            NerdsPagService nerdsPag = new(_config.DefaultApiKey, _config.DefaultEncriptionKey);

            var transaction = ParaTransaction(transacao, nerdsPag);

            return ParaTransacao(await transaction.CaptureCardTransaction());
        }

        public async Task<Transacao> CancelarPagamento(Transacao transacao)
        {
            NerdsPagService nerdsPagSvc = new(_config.DefaultApiKey, _config.DefaultEncriptionKey);

            var transaction = ParaTransaction(transacao, nerdsPagSvc);

            return ParaTransacao(await transaction.CancelAuthorization());
        }

        public static Transacao ParaTransacao(Transaction transaction) =>
            new()
            {
                Id = Guid.NewGuid(),
                Status = (StatusTransacao)transaction.Status,
                ValorTotal = transaction.Amount,
                BandeiraCartao = transaction.CardBrand,
                CodigoAutorizacao = transaction.AuthorizationCode,
                CustoTransacao = transaction.Cost,
                DataTransacao = transaction.TransactionDate,
                NSU = transaction.Nsu,
                TID = transaction.Tid,
            };

        private Transaction ParaTransaction(Transacao transacao, NerdsPagService nerdsPag)
            => new(nerdsPag)
            {
                Status = (TransactionStatus)transacao.Status,
                Amount = transacao.ValorTotal,
                CardBrand = transacao.BandeiraCartao,
                AuthorizationCode = transacao.CodigoAutorizacao,
                Cost = transacao.CustoTransacao,
                Nsu = transacao.NSU,
                Tid = transacao.TID
            };

        public Task<Transacao> CancelarAutorizacao(object transacaoAutorizada)
        {
            throw new NotImplementedException();
        }
    }
}
