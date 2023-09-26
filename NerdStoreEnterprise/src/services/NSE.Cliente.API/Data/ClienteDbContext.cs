using Microsoft.EntityFrameworkCore;
using NSE.Cliente.API.Models;
using NSE.Core.Data;
using NSE.Core.DomainObjects;
using NSE.Core.MediatR;

namespace NSE.Cliente.API.Data
{
    public class ClienteDbContext : DbContext, IUnitOfWork
    {
        public DbSet<ClienteEntity> Clientes { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }

        private readonly IMediatorHandler _mediatorHandler;

        public ClienteDbContext(DbContextOptions<ClienteDbContext> opt, IMediatorHandler mediatorHandler) : base(opt)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<bool> Commit()
        {
            var sucesso = await this.SaveChangesAsync() > 0;

            if (sucesso)
            {
                await _mediatorHandler.PublicarEventos(this);
            }

            return sucesso;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
                e => e.GetProperties().Where(p => p.ClrType == typeof(string)))) 
            {
                property.SetColumnType("varchar(160)");
            }
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys())) 
                relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ClienteDbContext).Assembly);
        }
    }

    public static class MediatorExtension
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
