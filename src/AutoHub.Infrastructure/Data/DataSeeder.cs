using AutoHub.Application.Interfaces;
using AutoHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AutoHub.Infrastructure.Data;

public class DataSeeder(ApplicationDbContext db) : IDataSeeder
{
    private readonly ApplicationDbContext _db = db;

    public async Task<bool> SeedAsync()
    {
        if (await _db.Marcas.AnyAsync())
        {
            return false;
        }

        var marca1 = new Marca { Id = Guid.NewGuid(), Nome = "Toyota" };
        var marca2 = new Marca { Id = Guid.NewGuid(), Nome = "Honda" };

        var modelo1 = new Modelo { Id = Guid.NewGuid(), Nome = "Corolla", MarcaId = marca1.Id };
        var modelo2 = new Modelo { Id = Guid.NewGuid(), Nome = "Civic", MarcaId = marca2.Id };

        var cliente = new Cliente
        {
            Id = Guid.NewGuid(),
            Nome = "Ana Souza",
            Cpf = "12345678901",
            Telefone = "11999999999",
            Email = "ana.souza@email.com",
            Logradouro = "Rua das Flores",
            Numero = "123",
            Cidade = "Sao Paulo",
            Estado = "SP",
            Cep = "01000-000"
        };

        var funcionario = new Funcionario
        {
            Id = Guid.NewGuid(),
            Nome = "Carlos Lima",
            Matricula = "FUNC-001",
            Cargo = "Mecanico",
            Cpf = "10987654321",
            Telefone = "11888888888",
            Email = "carlos.lima@email.com",
            DataAdmissao = DateTime.UtcNow.AddYears(-2),
            Salario = 4500.00m,
            Status = "Ativo"
        };

        var veiculoCliente = new VeiculoCliente
        {
            Id = Guid.NewGuid(),
            PlacaVeiculo = "ABC1234",
            Ano = 2020,
            Observacoes = "Revisao programada",
            ClienteId = cliente.Id,
            ModeloId = modelo1.Id
        };

        var veiculoEstoque = new VeiculoEstoque
        {
            Id = Guid.NewGuid(),
            AnoFabricacao = 2023,
            Cor = "Prata",
            Kilometragem = 15000,
            Preco = 135000.00m,
            Status = "Disponivel",
            Chassis = "9BWZZZ377VT004251",
            ModeloId = modelo1.Id
        };

        var servico = new Servico
        {
            Id = Guid.NewGuid(),
            Descricao = "Troca de oleo e filtros",
            PrecoBase = 350.00m
        };

        var peca = new Peca
        {
            Id = Guid.NewGuid(),
            Nome = "Filtro de oleo",
            CodigoFabricante = "FO-987",
            PrecoUnitario = 45.90m,
            EstoqueQuantidade = 20
        };

        var ordemServico = new OrdemServico
        {
            Id = Guid.NewGuid(),
            DataAbertura = DateTime.UtcNow,
            Status = "Aberta",
            Diagnostico = "Manutencao preventiva",
            FuncionarioId = funcionario.Id,
            VeiculoClienteId = veiculoCliente.Id
        };

        var osServico = new OsServico
        {
            OrdemServicoId = ordemServico.Id,
            ServicoId = servico.Id,
            Quantidade = 1,
            PrecoCobrado = 350.00m
        };

        var osPeca = new OsPeca
        {
            OrdemServicoId = ordemServico.Id,
            PecaId = peca.Id,
            Quantidade = 1,
            PrecoUnitario = 45.90m
        };

        await _db.Marcas.AddRangeAsync(marca1, marca2);
        await _db.Modelos.AddRangeAsync(modelo1, modelo2);
        await _db.Clientes.AddAsync(cliente);
        await _db.Funcionarios.AddAsync(funcionario);
        await _db.VeiculosCliente.AddAsync(veiculoCliente);
        await _db.VeiculosEstoque.AddAsync(veiculoEstoque);
        await _db.Servicos.AddAsync(servico);
        await _db.Pecas.AddAsync(peca);
        await _db.OrdensServico.AddAsync(ordemServico);
        await _db.OsServicos.AddAsync(osServico);
        await _db.OsPecas.AddAsync(osPeca);

        await _db.SaveChangesAsync();
        return true;
    }
}
