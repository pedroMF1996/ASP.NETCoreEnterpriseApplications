using NSE.Core.Messages;
using NSE.Pedido.API.Application.DTO;
using System.Text.Json.Serialization;

namespace NSE.Pedido.API.Application.Commands
{
    public class AdicionarPedidoCommand : Command
    {
        [JsonIgnore]
        public Guid? ClienteId { get; set; }
        public decimal ValorTotal { get; set; }
        public List<PedidoItemDTO> PedidoItems { get; set; }


        public string VoucherCodigo { get; set; }
        public bool VoucherUtilizado { get; set; }
        public decimal Desconto { get; set; }


        public EnderecoDTO Endereco { get; set; }


        public string NumeroCartao { get; set; }
        public string NomeCartao { get; set; }
        public string ExpiracaoCartao { get; set; }
        public string CvvCartao { get; set; }

        public override bool EhValido()
        {
            ValidationResult = new AdicionarPedidoCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
