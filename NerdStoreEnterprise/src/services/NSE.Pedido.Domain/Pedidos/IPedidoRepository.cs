using NSE.Core.Data;

namespace NSE.Pedido.Domain.Pedidos
{
    public interface IPedidoRepository : IRepository<Pedido>
    {
        Task<Pedido> ObterPorId(Guid pedidoId);
        Task<IEnumerable<Pedido>> ObterListaPorClienteId(Guid clienteId);
        Task Adicionar(Pedido pedido);
        void Atualizar(Pedido pedido);

        Task<PedidoItem> ObterItemPorId(Guid id);
        Task<PedidoItem> ObterItemPorPedido(Guid pedidoId, Guid produtoId);

    }
}
