using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
