namespace NSE.Core.Messages.Integration
{
    public class PedidoBaixadoEstoqueIntegrationEvent : IntegrationEvent
    {
        public Guid ClienteId { get; }
        public Guid Id { get; }

        public PedidoBaixadoEstoqueIntegrationEvent(Guid clienteId, Guid id)
        {
            ClienteId = clienteId;
            Id = id;
        }
    }
}
