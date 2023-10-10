using Microsoft.EntityFrameworkCore;
using NSE.Core.Data;
using NSE.Pedido.Domain.Pedidos;
using System.Data.Common;

namespace NSE.Pedido.Infra.Data.Repository
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly PedidosContext _context;

        public PedidoRepository(PedidosContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task Adicionar(Domain.Pedidos.Pedido pedido)
        {
            await _context.Pedidos.AddAsync(pedido);
        }

        public void Atualizar(Domain.Pedidos.Pedido pedido)
        {
            _context.Pedidos.Update(pedido);
        }

        public async Task<PedidoItem> ObterItemPorId(Guid id)
        {
            return await _context.PedidoItems.FindAsync(id);
        }

        public async Task<PedidoItem> ObterItemPorPedido(Guid pedidoId, Guid produtoId)
        {
            return await _context.PedidoItems
                .FirstOrDefaultAsync(i => i.PedidoId == pedidoId && i.ProdutoId == produtoId);
        }

        public async Task<IEnumerable<Domain.Pedidos.Pedido>> ObterListaPorClienteId(Guid clienteId)
        {
            return await _context.Pedidos.Include(p => p.PedidoItens)
                .AsNoTracking().Where(p => p.ClienteId == clienteId).ToListAsync();
        }

        public async Task<Domain.Pedidos.Pedido> ObterPorId(Guid pedidoId)
        {
            return await _context.Pedidos.FindAsync(pedidoId);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }

        public DbConnection ObterConexao()
        {
            return _context.Database.GetDbConnection();
        }
    }
}
