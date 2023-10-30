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

        public static Transacao ParaTransacao(Transaction transaction) =>
            new Transacao()
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
    }
}
