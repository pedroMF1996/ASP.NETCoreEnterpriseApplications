using NSE.Core.Data;

namespace NSE.Pedido.Domain.Vouchers.Interface
{
    public interface IVoucherRepository : IRepository<Voucher>
    {
        void Atualizar(Voucher voucher);
        Task<Voucher> ObterVoucherPorCodigo(string codigo);
    }
}
