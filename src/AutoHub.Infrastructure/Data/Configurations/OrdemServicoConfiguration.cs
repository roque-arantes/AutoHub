using AutoHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoHub.Infrastructure.Data.Configurations;

public class OrdemServicoConfiguration : IEntityTypeConfiguration<OrdemServico>
{
    public void Configure(EntityTypeBuilder<OrdemServico> builder)
    {
        builder.ToTable("OrdensServico");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.DataAbertura)
            .IsRequired();

        builder.Property(x => x.DataConclusao)
            .IsRequired(false);

        builder.Property(x => x.Status)
            .HasColumnType("VARCHAR(30)")
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(x => x.ValorFinal)
            .HasColumnType("decimal(8,2)")
            .IsRequired(false);

        builder.Property(x => x.Diagnostico)
            .HasColumnType("VARCHAR(300)")
            .HasMaxLength(300)
            .IsRequired(false);

        builder.HasOne(x => x.Funcionario)
            .WithMany(x => x.OrdemServicos)
            .HasForeignKey(x => x.FuncionarioId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.VeiculoCliente)
            .WithMany(x => x.OrdemServicos)
            .HasForeignKey(x => x.VeiculoClienteId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
