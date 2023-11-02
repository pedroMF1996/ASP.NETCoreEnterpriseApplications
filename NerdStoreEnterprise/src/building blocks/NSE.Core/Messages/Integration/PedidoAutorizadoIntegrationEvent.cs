namespace NSE.Core.Messages.Integration
{
    public class PedidoAutorizadoIntegrationEvent : IntegrationEvent
    {
        public Guid ClienteId { get; private set; }
        public Guid Id { get; private set; }
        public Dictionary<Guid, int> Itens { get; private set; }

        public PedidoAutorizadoIntegrationEvent(Guid clienteId, Guid id, Dictionary<Guid, int> dictionary)
        {
            ClienteId = clienteId;
            Id = id;
            Itens = dictionary;
        }
    }
}
