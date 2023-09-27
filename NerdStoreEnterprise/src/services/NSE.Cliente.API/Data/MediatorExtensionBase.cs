using Microsoft.EntityFrameworkCore;
using NSE.Core.DomainObjects;
using NSE.Core.MediatR;

namespace NSE.Cliente.API.Data
{
    public static class MediatorExtensionBase
    {
        public static async Task PublicarEventos<T>(this IMediatorHandler mediatorHandler, T ctx) where T : DbContext
        {
            var domainEntities = ctx.ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.Notificacoes != null && x.Entity.Notificacoes.Any());

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.Notificacoes)
                .ToList();

            domainEntities.ToList()
                .ForEach(e =>
                {
                    e.Entity.LimparEventos();
                });

            var task = domainEvents.
                Select(async domainEvent =>
                {
                    await mediatorHandler.PublicarEvento(domainEvent);
                });

            await Task.WhenAll(task);
        }
    }
}