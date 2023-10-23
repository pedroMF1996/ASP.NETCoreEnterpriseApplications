using System.Text.Json.Serialization;

namespace NSE.Pedido.API.Application.DTO
{
    public class PedidoItemQueryDTO
    {
        public string Nome { get; set; }
        public string Imagem { get; set; }
        public int Quantidade { get; set; }
        public Guid ProdutoId { get; set; }
        public decimal Valor { get; set; }

        [JsonIgnore]
        public Guid? PedidoId { get; set; }
    }
}
