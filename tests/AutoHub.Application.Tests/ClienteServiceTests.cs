using System.Linq.Expressions;
using AutoHub.Application.DTOs.Clientes;
using AutoHub.Application.Interfaces;
using AutoHub.Application.Services;
using AutoHub.Domain.Entities;
using AutoHub.Domain.Exceptions;
using Moq;
using Xunit;

namespace AutoHub.Application.Tests;

public class ClienteServiceTests
{
    private readonly Mock<IRepository<Cliente>> _clienteRepositoryMock;
    private readonly ClienteService _service;

    public ClienteServiceTests()
    {
        _clienteRepositoryMock = new Mock<IRepository<Cliente>>();
        _service = new ClienteService(_clienteRepositoryMock.Object);
    }

    [Fact]
    public async Task CreateAsync_QuandoCpfJaCadastrado_DeveLancarConflictExceptionENaoPersistir()
    {
        // Arrange
        var dto = new ClienteRequestDto
        {
            Nome = "Joao Silva",
            Cpf = "12345678901",
            Telefone = "11988887777",
            Email = "joao@email.com",
            Logradouro = "Rua A",
            Numero = "10",
            Cidade = "Sao Paulo",
            Estado = "SP",
            Cep = "01000-000"
        };

        _clienteRepositoryMock
            .Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<Cliente, bool>>>()))
            .ReturnsAsync(new Cliente { Id = Guid.NewGuid(), Cpf = dto.Cpf, Nome = "Outro Cliente" });

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ConflictException>(() => _service.CreateAsync(dto));
        Assert.Contains(dto.Cpf, ex.Message);

        _clienteRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Cliente>()), Times.Never);
        _clienteRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_QuandoDadosValidos_DevePersistirERetornarDto()
    {
        // Arrange
        var dto = new ClienteRequestDto
        {
            Nome = "Fernanda Costa",
            Cpf = "98765432100",
            Telefone = "11977776666",
            Email = "fernanda@email.com",
            Logradouro = "Av. Brasil",
            Numero = "500",
            Cidade = "Campinas",
            Estado = "SP",
            Cep = "13000-000"
        };

        _clienteRepositoryMock
            .Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<Cliente, bool>>>()))
            .ReturnsAsync((Cliente?)null);

        _clienteRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Cliente>()))
            .Returns(Task.CompletedTask);

        _clienteRepositoryMock
            .Setup(r => r.SaveChangesAsync())
            .ReturnsAsync(1);

        // Act
        var result = await _service.CreateAsync(dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(dto.Nome, result.Nome);
        Assert.Equal(dto.Cpf, result.Cpf);
        Assert.Equal("SP", result.Estado);

        _clienteRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Cliente>()), Times.Once);
        _clienteRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_QuandoIdInexistente_DeveLancarResourceNotFoundException()
    {
        // Arrange
        var idInexistente = Guid.NewGuid();

        _clienteRepositoryMock
            .Setup(r => r.GetByIdAsync(idInexistente))
            .ReturnsAsync((Cliente?)null);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ResourceNotFoundException>(() => _service.GetByIdAsync(idInexistente));
        Assert.Contains(idInexistente.ToString(), ex.Message);
    }
}
