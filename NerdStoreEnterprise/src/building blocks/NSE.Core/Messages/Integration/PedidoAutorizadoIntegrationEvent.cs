namespace NSE.Core.Messages.Integration
{
    public class PedidoAutorizadoIntegrationEvent : IntegrationEvent
    {
        public Guid ClienteId { get; private set; }
        public Guid Id { get; private set; }
        public IDictionary<Guid, int> Itens { get; private set; }

        public PedidoAutorizadoIntegrationEvent(Guid clienteId, Guid id, IDictionary<Guid, int> itens)
        {
            ClienteId = clienteId;
            Id = id;
            Itens = itens;
        }
    }
}
