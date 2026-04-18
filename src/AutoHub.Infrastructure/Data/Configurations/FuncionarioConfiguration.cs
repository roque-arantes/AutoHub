using AutoHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoHub.Infrastructure.Data.Configurations;

public class FuncionarioConfiguration : IEntityTypeConfiguration<Funcionario>
{
    public void Configure(EntityTypeBuilder<Funcionario> builder)
    {
        builder.ToTable("Funcionarios");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Nome)
            .HasColumnType("VARCHAR(50)")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Matricula)
            .HasColumnType("VARCHAR(50)")
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(x => x.Matricula)
            .IsUnique();

        builder.Property(x => x.Cargo)
            .HasColumnType("VARCHAR(30)")
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(x => x.Cpf)
            .HasColumnType("VARCHAR(11)")
            .HasMaxLength(11)
            .IsRequired();

        builder.Property(x => x.Telefone)
            .HasColumnType("VARCHAR(20)")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.Email)
            .HasColumnType("VARCHAR(100)")
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(x => x.DataAdmissao)
            .IsRequired();

        builder.Property(x => x.Salario)
            .HasColumnType("decimal(10,2)")
            .IsRequired();

        builder.Property(x => x.Status)
            .HasColumnType("VARCHAR(20)")
            .HasMaxLength(20)
            .IsRequired();
    }
}
