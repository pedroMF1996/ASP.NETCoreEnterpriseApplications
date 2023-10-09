using NSE.Core.DomainObjects;
using NSE.Pedido.Domain.Vouchers;

namespace NSE.Pedido.Domain.Pedidos
{
    public class Pedido : Entity, IAggregateRoot
    {
        public int Codigo { get; private set; }
        public Guid ClienteId { get; private set; }
        public bool VoucherUtilizado { get; private set; }
        public decimal Desconto { get; private set; }
        public decimal ValorTotal { get; private set; }
        public DateTime DataCadastro { get; private set; }
        public PedidoStatus PedidoStatus { get; private set; }
        public Endereco Endereco { get; private set; }

        public Guid? VoucherId { get; private set; }
        public Voucher Voucher { get; private set; }

        private readonly List<PedidoItem> _pedidoItems;
        public IReadOnlyCollection<PedidoItem> PedidoItens => _pedidoItems;

        protected Pedido()
        {}

        public Pedido(Guid clienteId, decimal valorTotal, List<PedidoItem> pedidoItems, 
            bool voucherUtilizado = false, decimal desconto = 0, Guid? voucherId = null)
        {
            ClienteId = clienteId;
            ValorTotal = valorTotal;
            _pedidoItems = pedidoItems;

            VoucherUtilizado = voucherUtilizado;
            Desconto = desconto;
            VoucherId = voucherId;
        }

        public void AutorizarPedido()
        {
            PedidoStatus = PedidoStatus.Autorizado;
        }

        public void AtribuirVoucher(Voucher voucher)
        {
            VoucherUtilizado = true;
            Voucher = voucher;
            VoucherId = voucher.Id;
        }

        public void AtribuirEndereco(Endereco endereco)
        {
            Endereco = endereco;
        }

        public void CalcularValorPedido()
        {
            ValorTotal = PedidoItens.Sum(p => p.CalcularValor());
            CalcularValorTotalDesconto();
        }

        internal void CalcularValorTotalDesconto()
        {
            if (!VoucherUtilizado) { return; }

            decimal desconto = 0;
            var valor = ValorTotal;

            desconto = ObterDesconto(desconto, valor);

            if (desconto <= 0)
                return;

            valor -= desconto;

            ValorTotal = valor < 0 ? 0 : valor;
            Desconto = desconto;
        }

        private decimal ObterDesconto(decimal desconto, decimal valor)
        {
            if (Voucher.TipoDesconto == TipoDescontoVoucher.Porcentagem)
            {
                if (Voucher.Percentual.HasValue)
                {
                    desconto = (valor + Voucher.Percentual.Value) / 100;
                }
            }
            else
            {
                if (Voucher.ValorDesconto.HasValue)
                {
                    desconto = Voucher.ValorDesconto.Value;
                }
            }

            return desconto;
        }
    }
}
