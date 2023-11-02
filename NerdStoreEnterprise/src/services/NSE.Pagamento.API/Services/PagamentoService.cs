using FluentValidation.Results;
using NSE.Core.DomainObjects;
using NSE.Core.Messages.Integration;
using NSE.Pagamento.API.Facade;
using NSE.Pagamento.API.Models;

namespace NSE.Pagamento.API.Services
{
    public class PagamentoService : IPagamentoService
    {
        private readonly IPagamentoFacace _pagamentoFacade;
        private readonly IPagamentoRepository _pagamentoRepository;

        public PagamentoService(IPagamentoFacace pagamentoFacade, IPagamentoRepository pagamentoRepository)
        {
            _pagamentoFacade = pagamentoFacade;
            _pagamentoRepository = pagamentoRepository;
        }

        public async Task<ResponseMessage> AutorizarPagamento(Models.Pagamento pagamento)
        {
            var transacao = await _pagamentoFacade.AutorizarPagamento(pagamento);

            ValidationResult validationResult = new();

            if (transacao.Status != StatusTransacao.Autorizado)
            {
                validationResult.Errors.Add(new ValidationFailure("Pagamento", "Pagamento recusado, entre em contato com a sua operadora de cartao"));

                return new ResponseMessage(validationResult);
            }

            pagamento.Transacoes.Add(transacao);
            _pagamentoRepository.AdicionarTransacao(transacao);
            _pagamentoRepository.AdicionarPagamento(pagamento);

            if (!await _pagamentoRepository.UnitOfWork.Commit())
            {
                validationResult.Errors.Add(new ValidationFailure("Pagamento", "Houve um erro ao realizar o pagamento"));

                //TODO: Comunicar com o gateway para realizar o estorno

                return new ResponseMessage(validationResult);
            }

            return new ResponseMessage(validationResult);
        }

        public async Task<ResponseMessage> CancelarPagamento(Guid pedidoId)
        {
            var transacoes = await _pagamentoRepository.ObterTransacaoesPorPedidoId(pedidoId);
            var transacaoAutorizada = transacoes?.FirstOrDefault(t => t.Status == StatusTransacao.Autorizado);
            var validationResult = new ValidationResult();

            if (transacaoAutorizada == null) throw new DomainException($"Transação não encontrada para o pedido {pedidoId}");

            var transacao = await _pagamentoFacade.CancelarAutorizacao(transacaoAutorizada);

            if (transacao.Status != StatusTransacao.Cancelado)
            {
                validationResult.Errors.Add(new ValidationFailure("Pagamento",
                    $"Não foi possível cancelar o pagamento do pedido {pedidoId}"));

                return new ResponseMessage(validationResult);
            }

            transacao.PagamentoId = transacaoAutorizada.PagamentoId;
            _pagamentoRepository.AdicionarTransacao(transacao);

            if (!await _pagamentoRepository.UnitOfWork.Commit())
            {
                validationResult.Errors.Add(new ValidationFailure("Pagamento",
                    $"Não foi possível persistir o cancelamento do pagamento do pedido {pedidoId}"));

                return new ResponseMessage(validationResult);
            }

            return new ResponseMessage(validationResult);
        }

        public async Task<ResponseMessage> CapturarPagamento(Guid pedidoId)
        {
            var transacoes = await _pagamentoRepository.ObterTransacaoesPorPedidoId(pedidoId);
            var transacaoAutorizada = transacoes?.FirstOrDefault(t => t.Status == StatusTransacao.Autorizado);
            ValidationResult validationResult = new();

            if (transacaoAutorizada == null) throw new DomainException($"Transação não encontrada para o pedido {pedidoId}");

            var transacao = await _pagamentoFacade.CapturarPagamento(transacaoAutorizada);

            if (transacao.Status != StatusTransacao.Pago)
            {
                validationResult.Errors.Add(new ValidationFailure("Pagamento",
                    $"Não foi possível capturar o pagamento do pedido {pedidoId}"));

                return new ResponseMessage(validationResult);
            }

            transacao.PagamentoId = transacaoAutorizada.PagamentoId;
            _pagamentoRepository.AdicionarTransacao(transacao);

            if (!await _pagamentoRepository.UnitOfWork.Commit())
            {
                validationResult.Errors.Add(new ValidationFailure("Pagamento",
                    $"Não foi possível persistir a captura do pagamento do pedido {pedidoId}"));

                return new ResponseMessage(validationResult);
            }

            return new ResponseMessage(validationResult);
        }
    }
}
