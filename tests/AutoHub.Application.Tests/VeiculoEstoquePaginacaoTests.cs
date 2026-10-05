using System.Linq.Expressions;
using AutoHub.Application.DTOs;
using AutoHub.Application.DTOs.VeiculosEstoque;
using AutoHub.Application.Interfaces;
using AutoHub.Application.Services;
using AutoHub.Domain.Entities;
using Moq;
using Xunit;

namespace AutoHub.Application.Tests;

public class VeiculoEstoquePaginacaoTests
{
    private readonly Mock<IRepository<VeiculoEstoque>> _veiculoRepositoryMock;
    private readonly Mock<IRepository<Modelo>> _modeloRepositoryMock;
    private readonly VeiculoEstoqueService _service;

    public VeiculoEstoquePaginacaoTests()
    {
        _veiculoRepositoryMock = new Mock<IRepository<VeiculoEstoque>>();
        _modeloRepositoryMock = new Mock<IRepository<Modelo>>();
        _service = new VeiculoEstoqueService(_veiculoRepositoryMock.Object, _modeloRepositoryMock.Object);
    }

    [Theory]
    [InlineData(0, 10)]   // page < 1
    [InlineData(-1, 10)]  // page negativo
    [InlineData(-5, 20)]  // page muito negativo
    public async Task GetPagedAsync_QuandoPageInvalido_DeveLancarArgumentException(int page, int pageSize)
    {
        // Act & Assert
        var ex = await Assert.ThrowsAsync<ArgumentException>(() => _service.GetPagedAsync(page, pageSize));
        Assert.Contains("page", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Theory]
    [InlineData(1, 0)]    // pageSize < 1
    [InlineData(1, -1)]   // pageSize negativo
    [InlineData(1, 101)]  // pageSize > 100
    [InlineData(1, 9999)] // pageSize muito grande
    public async Task GetPagedAsync_QuandoPageSizeInvalido_DeveLancarArgumentException(int page, int pageSize)
    {
        // Act & Assert
        var ex = await Assert.ThrowsAsync<ArgumentException>(() => _service.GetPagedAsync(page, pageSize));
        Assert.Contains("pageSize", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task GetPagedAsync_QuandoParametrosValidos_DeveRetornarEnvelopePaginado()
    {
        // Arrange
        var veiculos = new List<VeiculoEstoque>
        {
            new() { Id = Guid.NewGuid(), AnoFabricacao = 2023, Cor = "Branco", Kilometragem = 0, Preco = 100000, Status = "Disponivel", Chassis = "9BWZZZ377VT004251", ModeloId = Guid.NewGuid() },
            new() { Id = Guid.NewGuid(), AnoFabricacao = 2024, Cor = "Preto", Kilometragem = 5000, Preco = 120000, Status = "Disponivel", Chassis = "9BWZZZ377VT004252", ModeloId = Guid.NewGuid() }
        };

        _veiculoRepositoryMock
            .Setup(r => r.GetPagedAsync(1, 2))
            .ReturnsAsync((veiculos.AsEnumerable(), 5));

        // Act
        var result = await _service.GetPagedAsync(1, 2);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Page);
        Assert.Equal(2, result.PageSize);
        Assert.Equal(5, result.TotalItems);
        Assert.Equal(3, result.TotalPages); // ceil(5/2) = 3
        Assert.Equal(2, result.Items.Count());
    }

    [Fact]
    public async Task GetPagedAsync_QuandoPaginaAlemDoTotal_DeveRetornarItemsVazioComTotaisCorretos()
    {
        // Arrange
        _veiculoRepositoryMock
            .Setup(r => r.GetPagedAsync(999, 10))
            .ReturnsAsync((Enumerable.Empty<VeiculoEstoque>(), 5));

        // Act
        var result = await _service.GetPagedAsync(999, 10);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(999, result.Page);
        Assert.Equal(10, result.PageSize);
        Assert.Equal(5, result.TotalItems);
        Assert.Equal(1, result.TotalPages);
        Assert.Empty(result.Items);
    }

    [Fact]
    public async Task GetPagedAsync_QuandoPageSizeMaximo_DeveFuncionarComPageSize100()
    {
        // Arrange
        _veiculoRepositoryMock
            .Setup(r => r.GetPagedAsync(1, 100))
            .ReturnsAsync((Enumerable.Empty<VeiculoEstoque>(), 0));

        // Act
        var result = await _service.GetPagedAsync(1, 100);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Page);
        Assert.Equal(100, result.PageSize);
        Assert.Equal(0, result.TotalItems);
        Assert.Equal(0, result.TotalPages);
    }
}
