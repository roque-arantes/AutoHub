using AutoHub.Domain.Entities;
using AutoHub.Domain.Exceptions;
using Xunit;

namespace AutoHub.Domain.Tests;

public class VeiculoEstoqueTests
{
    [Fact]
    public void Validar_ComDadosValidos_NaoDeveLancarExcecao()
    {
        // Arrange
        var veiculo = new VeiculoEstoque
        {
            AnoFabricacao = 2022,
            Cor = "Preto",
            Kilometragem = 35000,
            Preco = 95000.00m,
            Status = "Disponivel",
            Chassis = "9BWZZZ377VT004251",
            ModeloId = Guid.NewGuid()
        };

        // Act
        var exception = Record.Exception(() => veiculo.Validar());

        // Assert
        Assert.Null(exception);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-500)]
    public void Validar_ComPrecoMenorOuIgualAZero_DeveLancarDomainException(decimal precoInvalido)
    {
        // Arrange
        var veiculo = new VeiculoEstoque
        {
            AnoFabricacao = 2022,
            Preco = precoInvalido,
            Chassis = "9BWZZZ377VT004251",
            ModeloId = Guid.NewGuid()
        };

        // Act & Assert
        var ex = Assert.Throws<DomainException>(() => veiculo.Validar());
        Assert.Equal("O preço do veículo deve ser maior que zero.", ex.Message);
    }

    [Theory]
    [InlineData(1899)]
    [InlineData(2035)]
    public void Validar_ComAnoFabricacaoInvalido_DeveLancarDomainException(int anoInvalido)
    {
        // Arrange
        var veiculo = new VeiculoEstoque
        {
            AnoFabricacao = anoInvalido,
            Preco = 50000m,
            Chassis = "9BWZZZ377VT004251",
            ModeloId = Guid.NewGuid()
        };

        // Act & Assert
        Assert.Throws<DomainException>(() => veiculo.Validar());
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("12345")]
    [InlineData("CHASSI-CURTO")]
    public void Validar_ComChassisInvalido_DeveLancarDomainException(string chassisInvalido)
    {
        // Arrange
        var veiculo = new VeiculoEstoque
        {
            AnoFabricacao = 2022,
            Preco = 50000m,
            Chassis = chassisInvalido,
            ModeloId = Guid.NewGuid()
        };

        // Act & Assert
        var ex = Assert.Throws<DomainException>(() => veiculo.Validar());
        Assert.Equal("O chassi deve possuir no mínimo 17 caracteres.", ex.Message);
    }
}
