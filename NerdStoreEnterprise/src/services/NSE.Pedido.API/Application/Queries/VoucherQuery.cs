using NSE.Pedido.API.Application.DTO;

namespace NSE.Pedido.API.Application.Queries
{
    public interface IVoucherQuery
    {
        Task<VoucherDTO> ObterVoucherPorCodigo(string codigo);
    }
    public class VoucherQuery : IVoucherQuery
    {
        private readonly IVoucherRepository _voucherRepository;

        public VoucherQuery(IVoucherRepository voucherRepository)
        {
            _voucherRepository = voucherRepository;
        }

        public async Task<VoucherDTO> ObterVoucherPorCodigo(string codigo)
        {
            var voucher = await _voucherRepository.ObterVoucherPorCodigo(codigo);

            if (voucher == null)
            {
                return null;
            }

            if (!voucher.EstaValidoParaUso())
                return null;

            return new VoucherDTO
            {
                Codigo = voucher.Codigo,
                Percentual = voucher.Percentual,
                TipoDesconto = (int)voucher.TipoDesconto,
                ValorDesconto = voucher.ValorDesconto,
            };
        }
    }
}
