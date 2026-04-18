using AutoHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AutoHub.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Marca> Marcas => Set<Marca>();
    public DbSet<Modelo> Modelos => Set<Modelo>();
    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Funcionario> Funcionarios => Set<Funcionario>();
    public DbSet<VeiculoCliente> VeiculosCliente => Set<VeiculoCliente>();
    public DbSet<VeiculoEstoque> VeiculosEstoque => Set<VeiculoEstoque>();
    public DbSet<Servico> Servicos => Set<Servico>();
    public DbSet<Peca> Pecas => Set<Peca>();
    public DbSet<OrdemServico> OrdensServico => Set<OrdemServico>();
    public DbSet<OsServico> OsServicos => Set<OsServico>();
    public DbSet<OsPeca> OsPecas => Set<OsPeca>();
    public DbSet<Venda> Vendas => Set<Venda>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
