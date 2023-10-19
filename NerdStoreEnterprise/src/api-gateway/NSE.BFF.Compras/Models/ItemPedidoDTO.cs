namespace NSE.BFF.Compras.Models
{
    public class ItemPedidoDTO
    {
        public Guid ProdutoId { get; set; }
        public string Nome { get; set; }
        public decimal ValorUnitario { get; set; }
        public string Imagem { get; set; }
        public int Quantidade { get; set; }
    }
}
