using AutoHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoHub.Infrastructure.Data.Configurations;

public class VendaConfiguration : IEntityTypeConfiguration<Venda>
{
    public void Configure(EntityTypeBuilder<Venda> builder)
    {
        builder.ToTable("Vendas");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.DataVenda)
            .IsRequired();

        builder.Property(x => x.PrecoOriginal)
            .HasColumnType("decimal(9,2)")
            .IsRequired();

        builder.Property(x => x.Desconto)
            .HasColumnType("decimal(9,2)")
            .IsRequired(false);

        builder.Property(x => x.ValorTotal)
            .HasColumnType("decimal(9,2)")
            .IsRequired();

        builder.Property(x => x.FormaPagamento)
            .HasColumnType("VARCHAR(30)")
            .HasMaxLength(30)
            .IsRequired();

        builder.HasOne(x => x.Cliente)
            .WithMany(x => x.Vendas)
            .HasForeignKey(x => x.ClienteId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Funcionario)
            .WithMany(x => x.Vendas)
            .HasForeignKey(x => x.FuncionarioId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.VeiculoEstoque)
            .WithOne(x => x.Venda)
            .HasForeignKey<Venda>(x => x.VeiculoEstoqueId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
