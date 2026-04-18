using AutoHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoHub.Infrastructure.Data.Configurations;

public class ServicoConfiguration : IEntityTypeConfiguration<Servico>
{
    public void Configure(EntityTypeBuilder<Servico> builder)
    {
        builder.ToTable("Servicos");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Descricao)
            .HasColumnType("VARCHAR(150)")
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(x => x.PrecoBase)
            .HasColumnType("decimal(8,2)")
            .IsRequired();
    }
}
