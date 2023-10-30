using FluentValidation.Results;
using NSE.Core.Messages.Integration;
using NSE.Pagamento.API.Facade;
using NSE.Pagamento.API.Models;

namespace NSE.Pagamento.API.Services
{
    public class PagamentoService : IPagamentoService
    {
        private readonly IPagamentoFacace _pagamentoFacace;
        private readonly IPagamentoRepository _pagamentoRepository;

        public PagamentoService(IPagamentoFacace pagamentoFacace, IPagamentoRepository pagamentoRepository)
        {
            _pagamentoFacace = pagamentoFacace;
            _pagamentoRepository = pagamentoRepository;
        }

        public async Task<ResponseMessage> AutorizarPagamento(Models.Pagamento pagamento)
        {
            var transacao = await _pagamentoFacace.AutorizarPagamento(pagamento);

            ValidationResult validationResult = new();

            if (transacao.Status != StatusTransacao.Autorizado) 
            {
                validationResult.Errors.Add(new ValidationFailure("Pagamento", "Pagamento recusado, entre em contato com a sua operadora de cartao"));

                return new ResponseMessage(validationResult); 
            }

            pagamento.Transacoes.Add(transacao);

            _pagamentoRepository.AdicionarPagamento(pagamento);

            if(!await _pagamentoRepository.UnitOfWork.Commit())
            {
                validationResult.Errors.Add(new ValidationFailure("Pagamento", "Houve um erro ao realizar o pagamento"));

                //TODO: Comunicar com o gateway para realizar o estorno

                return new ResponseMessage(validationResult);
            }

            return new ResponseMessage(validationResult);
        }
    }
}
