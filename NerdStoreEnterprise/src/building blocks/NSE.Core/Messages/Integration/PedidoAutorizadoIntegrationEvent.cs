namespace NSE.Core.Messages.Integration
{
    public class PedidoAutorizadoIntegrationEvent : IntegrationEvent
    {
        private object clienteId;
        private Guid id;
        private Dictionary<Guid, int> dictionary;

        public PedidoAutorizadoIntegrationEvent(object clienteId, Guid id, Dictionary<Guid, int> dictionary)
        {
            this.clienteId = clienteId;
            this.id = id;
            this.dictionary = dictionary;
        }
    }
}
