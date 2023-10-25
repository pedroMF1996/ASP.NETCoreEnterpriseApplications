using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using NSE.Core.Data;
using NSE.Core.MediatR;
using NSE.Core.Messages;
using NSE.Pedido.Domain.Pedidos;
using NSE.Pedido.Domain.Vouchers;

namespace NSE.Pedido.Infra.Data
{
    public class PedidosContext : DbContext, IUnitOfWork
    {
        private readonly IMediatorHandler _mediator;
        public PedidosContext(DbContextOptions<PedidosContext> opt, IMediatorHandler mediator) : base(opt)
        {
            _mediator = mediator;
        }

        public DbSet<Domain.Pedidos.Pedido> Pedidos { get; set; }
        public DbSet<PedidoItem> PedidoItems { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }

        public async Task<bool> Commit()
        {

            foreach (var entry in ChangeTracker.Entries()
                .Where(entry => entry.Entity.GetType().GetProperty("DataCadastro") != null))
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property("DataCadastro").CurrentValue = DateTime.Now;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Property("DataCadastro").IsModified = false;
                }
            }

            var sucesso = await SaveChangesAsync() > 0;

            if (sucesso) await _mediator.PublicarEventos(this);

            return sucesso;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<ValidationResult>();
            modelBuilder.Ignore<Event>();

            foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
                e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
            {
                property.SetColumnType("varchar(160)");
            }

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PedidosContext).Assembly);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys())) relationship.DeleteBehavior = DeleteBehavior.Restrict;

            modelBuilder.HasSequence<int>("MinhaSequencia").StartsAt(1000).IncrementsBy(1);
            base.OnModelCreating(modelBuilder);
        }
    }
}
