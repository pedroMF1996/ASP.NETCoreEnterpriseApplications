namespace NSE.Core.Messages.Integration
{
    public class CancelarPedidoSemEstoqueIntegrationEvent : IntegrationEvent
    {
        public Guid ClienteId { get; private set; }
        public Guid Id { get; private set; }

        public CancelarPedidoSemEstoqueIntegrationEvent(Guid clienteId, Guid id)
        {
            ClienteId = clienteId;
            Id = id;
        }
    }
}