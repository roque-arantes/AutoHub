using AutoHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoHub.Infrastructure.Data.Configurations;

public class OsServicoConfiguration : IEntityTypeConfiguration<OsServico>
{
    public void Configure(EntityTypeBuilder<OsServico> builder)
    {
        builder.ToTable("OsServicos");

        builder.HasKey(x => new { x.OrdemServicoId, x.ServicoId });

        builder.Property(x => x.Quantidade)
            .IsRequired();

        builder.Property(x => x.PrecoCobrado)
            .HasColumnType("decimal(8,2)")
            .IsRequired();

        builder.HasOne(x => x.OrdemServico)
            .WithMany(x => x.OsServicos)
            .HasForeignKey(x => x.OrdemServicoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Servico)
            .WithMany(x => x.OsServicos)
            .HasForeignKey(x => x.ServicoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
