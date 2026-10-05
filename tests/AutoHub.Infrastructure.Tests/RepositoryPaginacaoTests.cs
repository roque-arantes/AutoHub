using AutoHub.Domain.Entities;
using AutoHub.Infrastructure.Data;
using AutoHub.Infrastructure.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AutoHub.Infrastructure.Tests;

public class RepositoryPaginacaoTests : IAsyncLifetime
{
    private readonly SqliteConnection _connection = new("Data Source=:memory:");
    private ApplicationDbContext _context = null!;
    private Repository<Marca> _repository = null!;

    public async Task InitializeAsync()
    {
        await _connection.OpenAsync();
        _context = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(_connection).Options);
        await _context.Database.EnsureCreatedAsync();
        _repository = new Repository<Marca>(_context);
    }

    private async Task SeedAsync()
    {
        // Inserção fora da ordem para verificar a ordenação executada pelo repositório.
        foreach (var index in new[] { 5, 2, 4, 1, 3 })
            _context.Marcas.Add(new Marca
            {
                Id = Guid.Parse($"00000000-0000-0000-0000-{index:D12}"),
                Nome = $"Marca {index}"
            });
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();
    }

    [Fact]
    public async Task GetPagedAsync_DeveOrdenarECortarPaginasNoSqlite()
    {
        await SeedAsync();
        var first = await _repository.GetPagedAsync(1, 2);
        var second = await _repository.GetPagedAsync(2, 2);
        var last = await _repository.GetPagedAsync(3, 2);

        Assert.Equal(5, first.TotalCount);
        Assert.Equal(5, second.TotalCount);
        Assert.Equal(5, last.TotalCount);
        Assert.Equal(new[] { "Marca 1", "Marca 2" }, first.Items.Select(x => x.Nome));
        Assert.Equal(new[] { "Marca 3", "Marca 4" }, second.Items.Select(x => x.Nome));
        Assert.Equal("Marca 5", Assert.Single(last.Items).Nome);
        Assert.Empty(first.Items.Select(x => x.Id).Intersect(second.Items.Select(x => x.Id)));
        Assert.Empty(_context.ChangeTracker.Entries());
    }

    [Theory]
    [InlineData(4, 2)]
    [InlineData(999, 20)]
    [InlineData(int.MaxValue, 20)]
    [InlineData(int.MaxValue, 100)]
    public async Task GetPagedAsync_AlemDoTotal_DeveRetornarVazioSemOverflow(int page, int pageSize)
    {
        await SeedAsync();
        var result = await _repository.GetPagedAsync(page, pageSize);
        Assert.Equal(5, result.TotalCount);
        Assert.Empty(result.Items);
    }

    [Fact]
    public async Task GetPagedAsync_BancoVazio_DeveRetornarTotalZero()
    {
        var result = await _repository.GetPagedAsync(1, 20);
        Assert.Equal(0, result.TotalCount);
        Assert.Empty(result.Items);
    }

    public async Task DisposeAsync()
    {
        await _context.DisposeAsync();
        await _connection.DisposeAsync();
    }
}
