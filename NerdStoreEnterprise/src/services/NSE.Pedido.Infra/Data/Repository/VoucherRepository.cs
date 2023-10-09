using Microsoft.EntityFrameworkCore;
using NSE.Core.Data;
using NSE.Pedido.Domain.Vouchers;
using NSE.Pedido.Domain.Vouchers.Interface;

namespace NSE.Pedido.Infra.Data.Repository
{
    public class VoucherRepository : IVoucherRepository
    {
        private readonly PedidosContext _context;

        public VoucherRepository(PedidosContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<Voucher> ObterVoucherPorCodigo(string codigo)
        {
            return await _context.Vouchers.FirstOrDefaultAsync(v => v.Codigo == codigo);
        }
    }
}
