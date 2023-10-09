using NSE.Core.Data;

namespace NSE.Pedido.Domain.Vouchers.Interface
{
    public interface IVoucherRepository : IRepository<Voucher>
    {
        Task<Voucher> ObterVoucherPorCodigo(string codigo);
    }
}
