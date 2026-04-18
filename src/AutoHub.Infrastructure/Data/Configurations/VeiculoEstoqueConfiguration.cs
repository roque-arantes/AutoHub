using AutoHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoHub.Infrastructure.Data.Configurations;

public class VeiculoEstoqueConfiguration : IEntityTypeConfiguration<VeiculoEstoque>
{
    public void Configure(EntityTypeBuilder<VeiculoEstoque> builder)
    {
        builder.ToTable("VeiculosEstoque");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.AnoFabricacao)
            .IsRequired();

        builder.Property(x => x.Cor)
            .HasColumnType("VARCHAR(30)")
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(x => x.Kilometragem)
            .IsRequired();

        builder.Property(x => x.Preco)
            .HasColumnType("decimal(11,2)")
            .IsRequired();

        builder.Property(x => x.Status)
            .HasColumnType("VARCHAR(20)")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.Chassis)
            .HasColumnType("VARCHAR(17)")
            .HasMaxLength(17)
            .IsRequired();

        builder.HasIndex(x => x.Chassis)
            .IsUnique();

        builder.HasOne(x => x.Modelo)
            .WithMany(x => x.VeiculosEstoque)
            .HasForeignKey(x => x.ModeloId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
