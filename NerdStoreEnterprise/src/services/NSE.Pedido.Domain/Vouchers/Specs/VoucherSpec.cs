using NetDevPack.Specification;
using System.Linq.Expressions;

namespace NSE.Pedido.Domain.Vouchers.Specs
{
    public class VoucherDataSpecification : Specification<Voucher>
    {
        public override Expression<Func<Voucher, bool>> ToExpression()
        {
            return voucher => voucher.DataValidade >= DateTime.Now;
        }
    }

    public class VoucherQuantidadeSpecification : Specification<Voucher>
    {
        public override Expression<Func<Voucher, bool>> ToExpression()
        {
            return voucher => voucher.Quantidade > 0;
        }
    }

    public class VoucherAtivoSpecification : Specification<Voucher>
    {
        public override Expression<Func<Voucher, bool>> ToExpression()
        {
            return voucher => voucher.Ativo && !voucher.Utilizado;
        }
    }
}
