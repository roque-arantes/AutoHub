using System.Linq.Expressions;
using AutoHub.Application.DTOs.VeiculosEstoque;
using AutoHub.Application.Interfaces;
using AutoHub.Application.Services;
using AutoHub.Domain.Entities;
using AutoHub.Domain.Exceptions;
using Moq;
using Xunit;

namespace AutoHub.Application.Tests;

public class VeiculoEstoqueServiceTests
{
    private readonly Mock<IRepository<VeiculoEstoque>> _veiculoRepositoryMock;
    private readonly Mock<IRepository<Modelo>> _modeloRepositoryMock;
    private readonly VeiculoEstoqueService _service;

    public VeiculoEstoqueServiceTests()
    {
        _veiculoRepositoryMock = new Mock<IRepository<VeiculoEstoque>>();
        _modeloRepositoryMock = new Mock<IRepository<Modelo>>();
        _service = new VeiculoEstoqueService(_veiculoRepositoryMock.Object, _modeloRepositoryMock.Object);
    }

    [Fact]
    public async Task CreateAsync_QuandoModeloNaoExiste_DeveLancarResourceNotFoundExceptionENaoChamarAddNemSalvar()
    {
        // Arrange
        var modeloIdInexistente = Guid.NewGuid();
        var dto = new VeiculoEstoqueRequestDto
        {
            AnoFabricacao = 2023,
            Cor = "Branco",
            Kilometragem = 10000,
            Preco = 120000.00m,
            Status = "Disponivel",
            Chassis = "9BWZZZ377VT004251",
            ModeloId = modeloIdInexistente
        };

        _modeloRepositoryMock
            .Setup(r => r.ExistsByIdAsync(modeloIdInexistente))
            .ReturnsAsync(false);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ResourceNotFoundException>(() => _service.CreateAsync(dto));
        Assert.Contains(modeloIdInexistente.ToString(), ex.Message);

        _veiculoRepositoryMock.Verify(r => r.AddAsync(It.IsAny<VeiculoEstoque>()), Times.Never);
        _veiculoRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_QuandoChassisJaExiste_DeveLancarConflictExceptionENaoChamarAddNemSalvar()
    {
        // Arrange
        var modeloId = Guid.NewGuid();
        var dto = new VeiculoEstoqueRequestDto
        {
            AnoFabricacao = 2023,
            Cor = "Cinza",
            Kilometragem = 20000,
            Preco = 110000.00m,
            Status = "Disponivel",
            Chassis = "9BWZZZ377VT004251",
            ModeloId = modeloId
        };

        _modeloRepositoryMock
            .Setup(r => r.ExistsByIdAsync(modeloId))
            .ReturnsAsync(true);

        _veiculoRepositoryMock
            .Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<VeiculoEstoque, bool>>>()))
            .ReturnsAsync(new VeiculoEstoque { Id = Guid.NewGuid(), Chassis = dto.Chassis });

        // Act & Assert
        await Assert.ThrowsAsync<ConflictException>(() => _service.CreateAsync(dto));

        _veiculoRepositoryMock.Verify(r => r.AddAsync(It.IsAny<VeiculoEstoque>()), Times.Never);
        _veiculoRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_QuandoDadosSaoValidos_DevePersistirComSucessoERetornarDto()
    {
        // Arrange
        var modeloId = Guid.NewGuid();
        var dto = new VeiculoEstoqueRequestDto
        {
            AnoFabricacao = 2024,
            Cor = "Azul",
            Kilometragem = 5000,
            Preco = 145000.00m,
            Status = "Disponivel",
            Chassis = "9BWZZZ377VT004251",
            ModeloId = modeloId
        };

        _modeloRepositoryMock
            .Setup(r => r.ExistsByIdAsync(modeloId))
            .ReturnsAsync(true);

        _veiculoRepositoryMock
            .Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<VeiculoEstoque, bool>>>()))
            .ReturnsAsync((VeiculoEstoque?)null);

        _veiculoRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<VeiculoEstoque>()))
            .Returns(Task.CompletedTask);

        _veiculoRepositoryMock
            .Setup(r => r.SaveChangesAsync())
            .ReturnsAsync(1);

        // Act
        var result = await _service.CreateAsync(dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(dto.Chassis, result.Chassis);
        Assert.Equal(dto.Preco, result.Preco);
        Assert.Equal(dto.ModeloId, result.ModeloId);

        _veiculoRepositoryMock.Verify(r => r.AddAsync(It.IsAny<VeiculoEstoque>()), Times.Once);
        _veiculoRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }
}
