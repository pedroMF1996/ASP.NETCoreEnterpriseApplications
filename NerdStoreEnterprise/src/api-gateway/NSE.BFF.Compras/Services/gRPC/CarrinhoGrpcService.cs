using NSE.BFF.Compras.Models;
using NSE.Carrinho.API.gRPC;

namespace NSE.BFF.Compras.Services.gRPC
{
    public class CarrinhoGrpcService : ICarrinhoGrpcService
    {
        private readonly CarrinhoCompras.CarrinhoComprasClient _carrinhoCompras;

        public CarrinhoGrpcService(CarrinhoCompras.CarrinhoComprasClient carrinhoCompras)
        {
            _carrinhoCompras = carrinhoCompras;
        }

        public async Task<CarrinhoDTO> ObterCarrinho()
        {
            var response = await _carrinhoCompras.ObterCarrinhoAsync(new ObterCarrinhoRequest());
            return MapCarrinhoDTOFromProtoResponse(response);
        }
        
        private static CarrinhoDTO MapCarrinhoDTOFromProtoResponse(CarrinhoClienteResponse carrinho)
        {
            var carrinhoProto = new CarrinhoDTO
            {
                ValorTotal = (decimal)carrinho.ValorTotal,
                Desconto = (decimal)carrinho.Desconto,
                VoucherUtilizado = carrinho.Voucherutilizado,
            };

            if (carrinho.Voucher != null)
            {
                carrinhoProto.Voucher = new VoucherDTO
                {
                    Codigo = carrinho.Voucher.Codigo,
                    Percentual = (decimal?)carrinho.Voucher.Percentual,
                    ValorDesconto = (decimal?)carrinho.Voucher.Valordesconto,
                    TipoDesconto = carrinho.Voucher.Tipodesconto
                };
            }

            foreach (var item in carrinho.Itens)
            {
                carrinhoProto.Itens.Add(new ItemCarrinhoDTO
                {
                    Nome = item.Nome,
                    Imagem = item.Imagem,
                    ProdutoId = Guid.Parse(item.Produtoid),
                    Quantidade = item.Quantidade,
                    Valor = (decimal)item.Valor
                });
            }

            return carrinhoProto;
        }

    }
}
