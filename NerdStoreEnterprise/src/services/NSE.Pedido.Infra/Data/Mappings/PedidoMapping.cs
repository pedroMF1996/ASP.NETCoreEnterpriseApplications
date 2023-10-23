using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NSE.Pedido.Infra.Data.Mappings
{
    public class PedidoMapping : IEntityTypeConfiguration<Domain.Pedidos.Pedido>
    {
        public void Configure(EntityTypeBuilder<Domain.Pedidos.Pedido> builder)
        {
            builder.HasKey(c => c.Id);

            builder.OwnsOne(p => p.Endereco, e =>
            {
                e.Property(c => c.Logradouro)
                .HasColumnName("Logradouro");

                e.Property(c => c.Numero)
                .HasColumnName("Numero");

                e.Property(c => c.Cep)
                    .HasColumnName("Cep"); ;

                e.Property(c => c.Complemento)
                    .HasColumnName("Complemento");

                e.Property(c => c.Bairro)
                    .HasColumnName("Bairro");

                e.Property(c => c.Cidade)
                    .HasColumnName("Cidade");

                e.Property(c => c.Estado)
                    .HasColumnName("Estado");
            });

            builder.Property(c => c.Codigo)
                .HasDefaultValueSql("NEXT VALUE FOR MinhaSequencia");

            builder.HasMany(c => c.PedidoItens)
                .WithOne(c => c.Pedido)
                .HasForeignKey(c => c.PedidoId);

            builder.ToTable("Pedidos");
        }
    }
}
