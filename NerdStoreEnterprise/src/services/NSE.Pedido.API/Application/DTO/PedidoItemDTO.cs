using NSE.Pedido.Domain.Pedidos;

namespace NSE.Pedido.API.Application.DTO
{
    public class PedidoItemDTO
    {
        public string Nome { get; set; }
        public string Imagem { get; set; }
        public int Quantidade { get; set; }
        public Guid ProdutoId { get; set; }
        public decimal ValorUnitario { get; set; }
        public Guid PedidoId { get; set; }

        public static PedidoItem ParaPedidoItem(PedidoItemDTO itemDTO)
        {
            return new PedidoItem(itemDTO.PedidoId, itemDTO.Nome, itemDTO.Quantidade, itemDTO.ValorUnitario, itemDTO.Imagem);
        }
    }
}
