﻿using FluentValidation.Results;
using MediatR;
using NSE.Core.Messages;
using NSE.Core.Messages.Integration;
using NSE.MessageBus;
using NSE.Pedido.API.Application.DTO;
using NSE.Pedido.API.Application.Events;
using NSE.Pedido.Domain.Pedidos;
using NSE.Pedido.Domain.Vouchers.Interface;
using NSE.Pedido.Domain.Vouchers.Specs;

namespace NSE.Pedido.API.Application.Commands
{
    public class PedidoCommandHandler : CommandHandler,
        IRequestHandler<AdicionarPedidoCommand, ValidationResult>
    {

        private readonly IVoucherRepository _voucherRepository;
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IMessageBus _messageBus;

        public PedidoCommandHandler(IVoucherRepository voucherRepository, IPedidoRepository pedidoRepository, IMessageBus messageBus)
        {
            _voucherRepository = voucherRepository;
            _pedidoRepository = pedidoRepository;
            _messageBus = messageBus;
        }

        public async Task<ValidationResult> Handle(AdicionarPedidoCommand message, CancellationToken cancellationToken)
        {
            if (!message.EhValido()) return message.ValidationResult;

            var pedido = MapearPedido(message);

            if (!await AplicarVoucher(message, pedido)) return ValidationResult;

            if (!ValidarPedido(pedido)) return ValidationResult;

            if (!await ProcessarPagamento(pedido, message)) return ValidationResult;

            pedido.AutorizarPedido();

            pedido.AdicionarEvento(new PedidoRealizadoEvent(pedido.Id, pedido.ClienteId));

            await _pedidoRepository.Adicionar(pedido);

            return await PersistirDados(_pedidoRepository.UnitOfWork);
        }

        private bool ValidarPedido(Domain.Pedidos.Pedido pedido)
        {
            var pedidoValorOriginal = pedido.ValorTotal;
            var desconto = pedido.Desconto;

            pedido.CalcularValorPedido();

            if (pedido.ValorTotal != pedidoValorOriginal)
            {
                AdicionarErro("O valor total do pedido nao confere com o calculo do pedido");
                return false;
            }

            if (pedido.Desconto != desconto)
            {
                AdicionarErro("O valor do desconto nao confere com o desconto");
                return false;
            }

            return true;
        }

        private async Task<bool> AplicarVoucher(AdicionarPedidoCommand message, Domain.Pedidos.Pedido pedido)
        {
            if (!message.VoucherUtilizado) return true;

            var voucher = await _voucherRepository.ObterVoucherPorCodigo(message.VoucherCodigo);

            if (voucher == null)
            {
                AdicionarErro("O voucher informado nao existe");
                return false;
            }

            var voucherValidation = new VoucherValidation().Validate(voucher);

            if (!voucherValidation.IsValid)
            {
                voucherValidation.Errors.ToList().ForEach(m => AdicionarErro(m.ErrorMessage));
                return false;
            }

            pedido.AtribuirVoucher(voucher);
            voucher.DebitarQuantidade();

            _voucherRepository.Atualizar(voucher);

            return true;
        }

        private Domain.Pedidos.Pedido MapearPedido(AdicionarPedidoCommand message)
        {
            var endereco = new Endereco()
            {
                Logradouro = message.Endereco.Logradouro,
                Numero = message.Endereco.Numero,
                Complemento = message.Endereco.Complemento,
                Bairro = message.Endereco.Bairro,
                Cep = message.Endereco.Cep,
                Cidade = message.Endereco.Cidade,
                Estado = message.Endereco.Estado,
            };

            var clienteId = message.ClienteId.Value;

            var pedido = new Domain.Pedidos.Pedido(clienteId,
                                                   message.ValorTotal,
                                                   message.PedidoItems.Select(PedidoItemDTO.ParaPedidoItem).ToList(),
                                                   message.VoucherUtilizado,
                                                   message.Desconto);

            pedido.AtribuirEndereco(endereco);

            return pedido;
        }

        private async Task<bool> ProcessarPagamento(Domain.Pedidos.Pedido pedido, AdicionarPedidoCommand message)
        {
            PedidoIniciadoIntegrationEvent pedidoIniciadoIntegrationEvent = new()
            {
                PedidoId = pedido.Id,
                ClienteId = pedido.ClienteId,
                Valor = pedido.ValorTotal,
                TipoPagamento = 1,

                NomeCartao = message.NomeCartao,
                NumeroCartao = message.NumeroCartao,
                CVV = message.CvvCartao,
                MesAnoVencimento = message.ExpiracaoCartao,
            };

            var result = await _messageBus.RequestAsync<PedidoIniciadoIntegrationEvent, ResponseMessage>(pedidoIniciadoIntegrationEvent);

            if (!result.ValidationResult.IsValid)
                result.ValidationResult.Errors.ForEach(error => AdicionarErro(error.ErrorMessage));

            return result.ValidationResult.IsValid;
        }
    }
}
