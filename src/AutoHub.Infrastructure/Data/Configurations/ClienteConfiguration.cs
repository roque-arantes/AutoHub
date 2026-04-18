using AutoHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoHub.Infrastructure.Data.Configurations;

public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
{
    public void Configure(EntityTypeBuilder<Cliente> builder)
    {
        builder.ToTable("Clientes");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Nome)
            .HasColumnType("VARCHAR(50)")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Cpf)
            .HasColumnType("VARCHAR(11)")
            .HasMaxLength(11)
            .IsRequired();

        builder.HasIndex(x => x.Cpf)
            .IsUnique();

        builder.Property(x => x.Telefone)
            .HasColumnType("VARCHAR(20)")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.Email)
            .HasColumnType("VARCHAR(100)")
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(x => x.Logradouro)
            .HasColumnType("VARCHAR(100)")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Numero)
            .HasColumnType("VARCHAR(10)")
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(x => x.Cidade)
            .HasColumnType("VARCHAR(60)")
            .HasMaxLength(60)
            .IsRequired();

        builder.Property(x => x.Estado)
            .HasColumnType("VARCHAR(2)")
            .HasMaxLength(2)
            .IsRequired();

        builder.Property(x => x.Cep)
            .HasColumnType("VARCHAR(9)")
            .HasMaxLength(9)
            .IsRequired();
    }
}
