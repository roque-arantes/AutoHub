using AutoHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoHub.Infrastructure.Data.Configurations;

public class PecaConfiguration : IEntityTypeConfiguration<Peca>
{
    public void Configure(EntityTypeBuilder<Peca> builder)
    {
        builder.ToTable("Pecas");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Nome)
            .HasColumnType("VARCHAR(100)")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.CodigoFabricante)
            .HasColumnType("VARCHAR(60)")
            .HasMaxLength(60)
            .IsRequired();

        builder.Property(x => x.PrecoUnitario)
            .HasColumnType("decimal(8,2)")
            .IsRequired();

        builder.Property(x => x.EstoqueQuantidade)
            .IsRequired();
    }
}
