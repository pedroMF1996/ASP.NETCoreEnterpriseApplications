using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using NSE.Core.Data;
using NSE.Core.MediatR;
using NSE.Core.Messages;
using NSE.Pedido.Domain.Voucher;

namespace NSE.Pedido.Infra.Data
{
    public class PedidosContext : DbContext, IUnitOfWork
    {
        private readonly IMediatorHandler _mediator;
        public PedidosContext(DbContextOptions<PedidosContext> opt, IMediatorHandler mediator) : base(opt)
        {
            _mediator = mediator;
        }

        public DbSet<Voucher> Vouchers { get; set; }

        public async Task<bool> Commit()
        {
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
        }
    }
}
