using AutoHub.Domain.Entities;
using AutoHub.Domain.Exceptions;
using Xunit;

namespace AutoHub.Domain.Tests;

public class ClienteTests
{
    [Fact]
    public void Validar_ComDadosValidos_NaoDeveLancarExcecao()
    {
        // Arrange
        var cliente = new Cliente
        {
            Nome = "Carlos Eduardo",
            Cpf = "12345678901",
            Telefone = "11999998888",
            Email = "carlos@email.com",
            Logradouro = "Av. Paulista",
            Numero = "1000",
            Cidade = "Sao Paulo",
            Estado = "SP",
            Cep = "01310-100"
        };

        // Act
        var exception = Record.Exception(() => cliente.Validar());

        // Assert
        Assert.Null(exception);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Validar_ComNomeInvalido_DeveLancarDomainException(string? nomeInvalido)
    {
        // Arrange
        var cliente = new Cliente
        {
            Nome = nomeInvalido!,
            Cpf = "12345678901",
            Telefone = "11999998888"
        };

        // Act & Assert
        var ex = Assert.Throws<DomainException>(() => cliente.Validar());
        Assert.Equal("O nome do cliente é obrigatório.", ex.Message);
    }

    [Theory]
    [InlineData("")]
    [InlineData("123")]
    [InlineData("123456789012")]
    [InlineData("abcdefghijk")]
    public void Validar_ComCpfInvalido_DeveLancarDomainException(string cpfInvalido)
    {
        // Arrange
        var cliente = new Cliente
        {
            Nome = "Maria Santos",
            Cpf = cpfInvalido,
            Telefone = "11999998888"
        };

        // Act & Assert
        Assert.Throws<DomainException>(() => cliente.Validar());
    }

    [Theory]
    [InlineData("emailsemarroba.com")]
    [InlineData("usuario.provedor")]
    public void Validar_ComEmailInvalido_DeveLancarDomainException(string emailInvalido)
    {
        // Arrange
        var cliente = new Cliente
        {
            Nome = "Maria Santos",
            Cpf = "12345678901",
            Telefone = "11999998888",
            Email = emailInvalido
        };

        // Act & Assert
        var ex = Assert.Throws<DomainException>(() => cliente.Validar());
        Assert.Equal("O formato do e-mail informado é inválido.", ex.Message);
    }
}
