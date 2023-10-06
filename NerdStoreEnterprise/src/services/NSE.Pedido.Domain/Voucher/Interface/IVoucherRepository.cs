using NSE.Core.Data;

namespace NSE.Pedido.Domain.Voucher.Interface
{
    public interface IVoucherRepository : IRepository<Voucher>
    {
        Task<Voucher> ObterVoucherPorCodigo(string codigo);
    }
}
