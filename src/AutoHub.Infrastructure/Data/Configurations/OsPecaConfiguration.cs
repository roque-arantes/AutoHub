using AutoHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoHub.Infrastructure.Data.Configurations;

public class OsPecaConfiguration : IEntityTypeConfiguration<OsPeca>
{
    public void Configure(EntityTypeBuilder<OsPeca> builder)
    {
        builder.ToTable("OsPecas");

        builder.HasKey(x => new { x.OrdemServicoId, x.PecaId });

        builder.Property(x => x.Quantidade)
            .IsRequired();

        builder.Property(x => x.PrecoUnitario)
            .HasColumnType("decimal(8,2)")
            .IsRequired();

        builder.HasOne(x => x.OrdemServico)
            .WithMany(x => x.OsPecas)
            .HasForeignKey(x => x.OrdemServicoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Peca)
            .WithMany(x => x.OsPecas)
            .HasForeignKey(x => x.PecaId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
