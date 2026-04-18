using AutoHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoHub.Infrastructure.Data.Configurations;

public class VeiculoClienteConfiguration : IEntityTypeConfiguration<VeiculoCliente>
{
    public void Configure(EntityTypeBuilder<VeiculoCliente> builder)
    {
        builder.ToTable("VeiculosCliente");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.PlacaVeiculo)
            .HasColumnType("VARCHAR(7)")
            .HasMaxLength(7)
            .IsRequired();

        builder.HasIndex(x => x.PlacaVeiculo)
            .IsUnique();

        builder.Property(x => x.Ano)
            .IsRequired();

        builder.Property(x => x.Observacoes)
            .HasColumnType("VARCHAR(300)")
            .HasMaxLength(300)
            .IsRequired(false);

        builder.HasOne(x => x.Cliente)
            .WithMany(x => x.VeiculosCliente)
            .HasForeignKey(x => x.ClienteId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Modelo)
            .WithMany(x => x.VeiculosCliente)
            .HasForeignKey(x => x.ModeloId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
