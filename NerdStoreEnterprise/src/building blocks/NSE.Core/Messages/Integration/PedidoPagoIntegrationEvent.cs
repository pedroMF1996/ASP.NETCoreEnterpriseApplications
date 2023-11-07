namespace NSE.Core.Messages.Integration
{
    public class PedidoPagoIntegrationEvent : IntegrationEvent
    {
        public PedidoPagoIntegrationEvent(Guid id, Guid clienteId)
        {
            Id = id;
            ClienteId = clienteId;
        }

        public Guid Id { get; }
        public Guid ClienteId { get; }
    }
}
